using System.Collections;
using UnityEngine;

public class WolftAttackTut : MonoBehaviour
{
    public float speed = 13;
    public float destroyDelay = 1.75f;
    public float erodeRate = 0.03f;
    public float erodeRefreshRate = 0.01f;
    public float erodeDelay = 1.5f;
    public SkinnedMeshRenderer erodeObject;

    void Start()
    {
        StartCoroutine(ErodeObject());
        Destroy(gameObject, destroyDelay);
    }

    IEnumerator ErodeObject()
    {
        yield return new WaitForSeconds(erodeDelay);

        float t = 0;
        while (t < 1f)
        {
            t += erodeRate;
            erodeObject.material.SetFloat("_Erode", t);
            yield return new WaitForSeconds(erodeRefreshRate);
        }

        // Đảm bảo giá trị cuối cùng là 1.0 để tránh thiếu chính xác do float
        erodeObject.material.SetFloat("_Erode", 1f);
    }
}
