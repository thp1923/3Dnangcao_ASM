using System.Collections.Generic;
using UnityEngine;

public class OnBecameEx : MonoBehaviour
{
    public string layerNameToCheck = "Target"; // Đặt tên layer bạn muốn tìm
    Camera mainCam;
    public List<GameObject> objectsToCheck; // Add your objects here

    private void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        int targetLayer = LayerMask.NameToLayer(layerNameToCheck);
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
        foreach (Renderer r in allRenderers)
        {
            if (r.gameObject.layer == targetLayer)
            {
                objectsToCheck.Add(r.gameObject);
            }
        }
    }

    void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCam);

        foreach (GameObject obj in objectsToCheck)
        {
            if (obj == null) continue;

            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                bool isVisible = GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
                obj.SetActive(isVisible);
            }
        }
    }
}
