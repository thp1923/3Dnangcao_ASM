using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingControllerTut : MonoBehaviour
{
    public GameObject targetRoot;
    public List<SkinnedMeshRenderer> skinnedMeshes;
    public VisualEffect VFX_Graph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private List<Material[]> skinnedMaterialsList = new List<Material[]>();

    void Collect()
    {
        if (targetRoot == null)
        {
            return;
        }

        skinnedMeshes.Clear(); // Xoá dữ liệu cũ tránh lỗi
        skinnedMeshes.AddRange(targetRoot.GetComponentsInChildren<SkinnedMeshRenderer>());
    }

    void Start()
    {
        Collect();
        if (VFX_Graph != null)
        {
            VFX_Graph.gameObject.SetActive(false);
        }

        // Lưu lại tất cả materials từ từng SkinnedMeshRenderer
        if (skinnedMeshes != null && skinnedMeshes.Count > 0)
        {
            foreach (var mesh in skinnedMeshes)
            {
                if (mesh != null)
                {
                    skinnedMaterialsList.Add(mesh.materials);
                }
            }
        }
    }

    public void DieEffect()
    {
        StartCoroutine(DissolvingCo());
    }

    IEnumerator DissolvingCo()
    {
        if (VFX_Graph != null)
        {
            VFX_Graph.gameObject.SetActive(true);
        }

        if (skinnedMaterialsList.Count > 0)
        {
            float counter = 0;

            while (counter < 1)
            {
                counter += dissolveRate;

                foreach (var materials in skinnedMaterialsList)
                {
                    Material[] tempMaterials = materials;

                    if (tempMaterials.Length > 1)
                    {
                        List<Material> tempList = new List<Material>(tempMaterials);
                        tempList.RemoveAt(tempList.Count - 1); // Xoá material cuối cùng

                        foreach (var mat in tempList)
                        {
                            mat.SetFloat("_DissolveAmount", counter);
                        }
                    }
                    else
                    {
                        foreach (var mat in tempMaterials)
                        {
                            mat.SetFloat("_DissolveAmount", counter);
                        }
                    }
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
