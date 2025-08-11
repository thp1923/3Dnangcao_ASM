using UnityEngine;
using System.Collections.Generic;

public class SafeRuntimeOptimizer : MonoBehaviour
{
    [Header("Camera & Distance Settings")]
    public Camera targetCamera;
    public float disableDistance = 50f;
    public KeyCode toggleKey = KeyCode.O;

    [Header("Quality Settings")]
    public float optimizedLodBias = 0.5f;
    public float optimizedShadowDistance = 30f;
    public int optimizedPixelLightCount = 0;
    public int optimizedTextureLimit = 1;

    private List<SkinnedMeshRenderer> skinnedMeshes = new List<SkinnedMeshRenderer>();
    private List<Animator> animators = new List<Animator>();
    private List<LODGroup> lodGroups = new List<LODGroup>();

    // Backup settings
    private float origLodBias;
    private float origShadowDistance;
    private int origPixelLightCount;
    private int origTextureLimit;

    private CameraClearFlags origClearFlags;
    private Color origBackgroundColor;
    private int origCullingMask;

    private Camera cam;
    private bool active = true;

    void Start()
    {
        // Find camera if not assigned
        cam = targetCamera ?? Camera.main ?? FindObjectOfType<Camera>();

        if (cam != null)
        {
            origClearFlags = cam.clearFlags;
            origBackgroundColor = cam.backgroundColor;
            origCullingMask = cam.cullingMask;
        }

        // Cache scene objects
        skinnedMeshes.AddRange(FindObjectsOfType<SkinnedMeshRenderer>());
        animators.AddRange(FindObjectsOfType<Animator>());
        lodGroups.AddRange(FindObjectsOfType<LODGroup>());

        // Backup quality
        origLodBias = QualitySettings.lodBias;
        origShadowDistance = QualitySettings.shadowDistance;
        origPixelLightCount = QualitySettings.pixelLightCount;
        origTextureLimit = QualitySettings.globalTextureMipmapLimit;

        EnableOptimizer(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            EnableOptimizer(!active);

        if (active && cam != null)
        {
            Vector3 camPos = cam.transform.position;

            foreach (var smr in skinnedMeshes)
            {
                if (smr == null) continue;
                smr.enabled = Vector3.Distance(camPos, smr.transform.position) < disableDistance;
            }

            foreach (var anim in animators)
            {
                if (anim == null) continue;
                anim.enabled = Vector3.Distance(camPos, anim.transform.position) < disableDistance;
            }
        }
    }

    void EnableOptimizer(bool enable)
    {
        active = enable;

        if (enable)
        {
            QualitySettings.lodBias = optimizedLodBias;
            QualitySettings.shadowDistance = optimizedShadowDistance;
            QualitySettings.pixelLightCount = optimizedPixelLightCount;
            QualitySettings.globalTextureMipmapLimit = optimizedTextureLimit;
        }
        else
        {
            QualitySettings.lodBias = origLodBias;
            QualitySettings.shadowDistance = origShadowDistance;
            QualitySettings.pixelLightCount = origPixelLightCount;
            QualitySettings.globalTextureMipmapLimit = origTextureLimit;

            if (cam != null)
            {
                cam.clearFlags = origClearFlags;
                cam.backgroundColor = origBackgroundColor;
                cam.cullingMask = origCullingMask;
            }
        }
    }
}
