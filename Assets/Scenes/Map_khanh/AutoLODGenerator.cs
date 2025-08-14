using UnityEngine;
using UnityEditor;

public class AutoLODGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generate LOD for Selected")]
    static void GenerateLOD()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
            {
                Debug.LogWarning($"Object {obj.name} không có MeshFilter.");
                continue;
            }

            // Tạo bản sao mesh gốc
            Mesh lod0 = meshFilter.sharedMesh;

            // Giảm chi tiết để tạo LOD1 và LOD2
            Mesh lod1 = Instantiate(lod0);
            MeshUtility.Optimize(lod1);
            MeshSimplify(lod1, 0.5f); // giảm 50%

            Mesh lod2 = Instantiate(lod0);
            MeshUtility.Optimize(lod2);
            MeshSimplify(lod2, 0.2f); // giảm 80%

            // Tạo GameObject con cho mỗi LOD
            GameObject goLOD0 = CreateLODObject(obj, lod0, "LOD0");
            GameObject goLOD1 = CreateLODObject(obj, lod1, "LOD1");
            GameObject goLOD2 = CreateLODObject(obj, lod2, "LOD2");

            // Thêm LOD Group
            LODGroup lodGroup = obj.GetComponent<LODGroup>();
            if (lodGroup == null)
                lodGroup = obj.AddComponent<LODGroup>();

            Renderer[] r0 = new Renderer[] { goLOD0.GetComponent<Renderer>() };
            Renderer[] r1 = new Renderer[] { goLOD1.GetComponent<Renderer>() };
            Renderer[] r2 = new Renderer[] { goLOD2.GetComponent<Renderer>() };

            LOD[] lods = new LOD[3];
            lods[0] = new LOD(0.6f, r0);
            lods[1] = new LOD(0.3f, r1);
            lods[2] = new LOD(0.05f, r2);

            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();

            Debug.Log($"Đã tạo LOD cho {obj.name}");
        }
    }

    static GameObject CreateLODObject(GameObject parent, Mesh mesh, string name)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterials = parent.GetComponent<MeshRenderer>().sharedMaterials;

        return go;
    }

    // Giảm số lượng vertex (dùng thuật toán đơn giản)
    static void MeshSimplify(Mesh mesh, float quality)
    {
        // Lưu ý: Cách này chỉ optimize đơn giản.
        // Nếu muốn giảm polygon mạnh, nên dùng plugin Mesh Simplifier / Simplygon.
        MeshUtility.Optimize(mesh);
        // Trong Unity Editor gốc không có sẵn reduce poly nâng cao, đây là placeholder.
    }
}
