using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// RuntimeFullOptimizer
/// - RunOptimize(): làm mọi tối ưu "cứng" (LOD bias thấp, shadow off, pixel lights 0, enable GPU instancing)
/// - Combine static meshes per-material and disable original MeshRenderer (not gameObject)
/// - RevertOptimize(): phục hồi trạng thái ban đầu
/// NOTE: không sử dụng UnityEditor trong file này (build-safe)
/// </summary>
public class RuntimeFullOptimizer : MonoBehaviour
{
    [Header("Tune (aggressive)")]
    public float forcedLodBias = 0.3f;
    public float forcedShadowDistance = 0f; // 0 = effectively off
    public ShadowQuality forcedShadowQuality = ShadowQuality.Disable;
    public int forcedPixelLights = 0;
    public int maxVertsPerCombined = 65000;
    public bool combineStatic = true;
    public bool enableGPUInstancing = true;
    public bool disableShadowsOnRenderers = true; // also set renderer.shadowCastingMode = Off

    // bookkeeping to revert
    float origLodBias;
    float origShadowDistance;
    ShadowQuality origShadowQuality;
    int origPixelLights;
    int origTextureLimit;

    // material instancing original states
    Dictionary<Material, bool> origMaterialInstancing = new Dictionary<Material, bool>();

    // combine bookkeeping
    GameObject combinedParent;
    List<Mesh> generatedMeshes = new List<Mesh>();
    List<MeshRenderer> disabledRenderers = new List<MeshRenderer>();
    List<RendererShadowData> rendererShadowBackup = new List<RendererShadowData>();

    bool optimized = false;

    struct RendererShadowData
    {
        public Renderer renderer;
        public UnityEngine.Rendering.ShadowCastingMode castingMode;
        public bool receiveShadows;
    }

    void Awake()
    {
        // cache original quality at awake
        origLodBias = QualitySettings.lodBias;
        origShadowDistance = QualitySettings.shadowDistance;
        origShadowQuality = QualitySettings.shadows;
        origPixelLights = QualitySettings.pixelLightCount;
        origTextureLimit = QualitySettings.globalTextureMipmapLimit;
    }

    /// <summary>
    /// Call to perform full aggressive optimization.
    /// </summary>
    public void RunOptimize()
    {
        if (optimized)
        {
            Debug.LogWarning("RuntimeFullOptimizer: Already optimized.");
            return;
        }

        Debug.Log("RuntimeFullOptimizer: Running aggressive optimization...");

        // 1. Quality tweaks
        QualitySettings.lodBias = forcedLodBias;
        QualitySettings.shadowDistance = forcedShadowDistance;
        QualitySettings.shadows = forcedShadowQuality;
        QualitySettings.pixelLightCount = forcedPixelLights;
        // make textures smaller aggressively (optional)
        QualitySettings.globalTextureMipmapLimit = 1;

        // 2. GPU Instancing for materials
        if (enableGPUInstancing)
        {
            var allMaterials = FindObjectsOfType<Renderer>()
                .SelectMany(r => r.sharedMaterials)
                .Where(m => m != null)
                .Distinct()
                .ToList();

            foreach (var mat in allMaterials)
            {
                if (!origMaterialInstancing.ContainsKey(mat))
                    origMaterialInstancing[mat] = mat.enableInstancing;

                try { mat.enableInstancing = true; }
                catch { /* some built-in mats may be read-only */ }
            }
        }

        // 3. Disable shadows per renderer (and backup state)
        if (disableShadowsOnRenderers)
        {
            var renderers = FindObjectsOfType<Renderer>(true);
            foreach (var r in renderers)
            {
                // backup
                rendererShadowBackup.Add(new RendererShadowData
                {
                    renderer = r,
                    castingMode = r.shadowCastingMode,
                    receiveShadows = r.receiveShadows
                });

                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
            }
        }

        // 4. Combine static meshes per-material (disable original MeshRenderer.enabled to avoid breaking logic)
        if (combineStatic)
        {
            CombineStaticMeshes();
        }

        optimized = true;
        Debug.Log("RuntimeFullOptimizer: Done.");
    }

    /// <summary>
    /// Revert optimization (restore quality, materials, re-enable original renderers, destroy combined meshes).
    /// </summary>
    public void RevertOptimize()
    {
        if (!optimized)
        {
            Debug.LogWarning("RuntimeFullOptimizer: Not optimized yet.");
            return;
        }

        // revert quality
        QualitySettings.lodBias = origLodBias;
        QualitySettings.shadowDistance = origShadowDistance;
        QualitySettings.shadows = origShadowQuality;
        QualitySettings.pixelLightCount = origPixelLights;
        QualitySettings.globalTextureMipmapLimit = origTextureLimit;

        // revert material instancing
        foreach (var kv in origMaterialInstancing)
        {
            if (kv.Key != null)
            {
                try { kv.Key.enableInstancing = kv.Value; }
                catch { }
            }
        }
        origMaterialInstancing.Clear();

        // revert renderer shadow states
        foreach (var data in rendererShadowBackup)
        {
            if (data.renderer != null)
            {
                data.renderer.shadowCastingMode = data.castingMode;
                data.renderer.receiveShadows = data.receiveShadows;
            }
        }
        rendererShadowBackup.Clear();

        // re-enable original renderers
        foreach (var r in disabledRenderers)
        {
            if (r != null) r.enabled = true;
        }
        disabledRenderers.Clear();

        // destroy combined meshes / parent
        if (combinedParent != null)
        {
            Destroy(combinedParent);
            combinedParent = null;
        }

        foreach (var m in generatedMeshes)
        {
            if (m != null) Destroy(m);
        }
        generatedMeshes.Clear();

        optimized = false;
        Debug.Log("RuntimeFullOptimizer: Reverted.");
    }

    void CombineStaticMeshes()
    {
        // collect candidates: MeshRenderer with MeshFilter, active, not in LODGroup, not UI, not Particle, static = true
        var all = FindObjectsOfType<MeshRenderer>(true);
        var candidates = new List<MeshRenderer>();
        foreach (var mr in all)
        {
            if (mr == null) continue;
            if (!mr.gameObject.activeInHierarchy) continue;
            if (mr.GetComponentInParent<LODGroup>() != null) continue;
            if (mr.GetComponentInParent<Canvas>() != null) continue;
            if (mr.GetComponent<ParticleSystem>() != null) continue;
            if (mr.GetComponent<SkinnedMeshRenderer>() != null) continue;
            if (!mr.gameObject.isStatic) continue; // only static for safety
            // ensure has meshfilter and readable mesh
            var mf = mr.GetComponent<MeshFilter>();
            if (mf == null || mf.sharedMesh == null) continue;
            if (!mf.sharedMesh.isReadable)
            {
                Debug.LogWarning($"RuntimeFullOptimizer: Skipping non-readable mesh on {mr.gameObject.name}. Enable Read/Write in import settings to allow combine.");
                continue;
            }
            candidates.Add(mr);
        }

        if (candidates.Count == 0)
        {
            Debug.Log("RuntimeFullOptimizer: No static candidates to combine.");
            return;
        }

        // group by material (first material)
        var groups = candidates.GroupBy(x => x.sharedMaterial).ToList();
        combinedParent = new GameObject("Combined_Static_Meshes");
        combinedParent.transform.SetParent(transform, false);

        foreach (var g in groups)
        {
            var mat = g.Key;
            var list = g.ToList();

            var currentCombine = new List<CombineInstance>();
            int currentVerts = 0;
            int chunkIndex = 0;

            System.Action Flush = () =>
            {
                if (currentCombine.Count == 0) return;
                var mesh = new Mesh();
                long totalVerts = currentCombine.Sum(ci => ci.mesh != null ? ci.mesh.vertexCount : 0);
                if (totalVerts > 65000) mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                try
                {
                    mesh.CombineMeshes(currentCombine.ToArray(), true, true);
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"RuntimeFullOptimizer: Combine failed: {ex.Message}");
                    currentCombine.Clear();
                    currentVerts = 0;
                    return;
                }

                GameObject go = new GameObject($"combined_{(mat ? mat.name : "null")}_{chunkIndex}");
                go.transform.SetParent(combinedParent.transform, false);
                var mf = go.AddComponent<MeshFilter>();
                mf.sharedMesh = mesh;
                var mrNew = go.AddComponent<MeshRenderer>();
                mrNew.sharedMaterial = mat;
                mrNew.receiveShadows = false;
                mrNew.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                go.isStatic = true;

                generatedMeshes.Add(mesh);
                chunkIndex++;
                currentCombine.Clear();
                currentVerts = 0;
            };

            foreach (var mr in list)
            {
                var mf = mr.GetComponent<MeshFilter>();
                if (mf == null || mf.sharedMesh == null) continue;
                var mesh = mf.sharedMesh;
                int vcount = mesh.vertexCount;
                if (currentVerts + vcount > maxVertsPerCombined && currentCombine.Count > 0)
                {
                    Flush();
                }

                CombineInstance ci = new CombineInstance { mesh = mesh, transform = mf.transform.localToWorldMatrix };
                currentCombine.Add(ci);
                currentVerts += vcount;

                // disable original renderer (not entire GameObject)
                mr.enabled = false;
                disabledRenderers.Add(mr);
            }

            if (currentCombine.Count > 0) Flush();
        }

        Debug.Log($"RuntimeFullOptimizer: Combined groups: {groups.Count}, generated combined meshes: {generatedMeshes.Count}, disabled renderers: {disabledRenderers.Count}");
    }
}
