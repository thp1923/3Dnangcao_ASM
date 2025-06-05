using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingControllerTut : MonoBehaviour
{
    public SkinnedMeshRenderer skinedMesh;
    public VisualEffect VFX_Graph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    private Material[] skinedMaterials;
    void Start()
    {
        if (VFX_Graph != null)
        {
            VFX_Graph.Stop();
        }
        if (skinedMesh != null)
            skinedMaterials = skinedMesh.materials;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    StartCoroutine(DissolvingCo());
        //}
    }

    public void DieEffect()
    {
        StartCoroutine(DissolvingCo());
    }
    IEnumerator DissolvingCo()
    {
        if(VFX_Graph != null)
        {
            // Mở rộng vùng bounds lớn để VFX không bị tắt
            VFX_Graph.SetVector3("bounds_center", Vector3.zero);
            VFX_Graph.SetVector3("bounds_size", new Vector3(99999999999999999, 99999999999999999, 99999999999999999));
            VFX_Graph.Play();
        }
        if(skinedMaterials.Length > 0)
        {
            float counter = 0;

            while (skinedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                for(int i = 0; i < skinedMaterials.Length; i++)
                {
                    skinedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}
