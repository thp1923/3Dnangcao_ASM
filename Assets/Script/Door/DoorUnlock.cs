using System.Collections;
using UnityEngine;
public class DoorUnlock : MonoBehaviour
{
    public bool isLocked = true;

    [Header("Shader Dissolve")]
    public Renderer doorRenderer;
    public string dissolveProperty = "_CutoffHeight";
    public float dissolveDuration = 1.5f;

    private Material doorMaterial;
    private bool isDissolving = false;

    void Start()
    {
        if (doorRenderer != null)
        {
            doorMaterial = doorRenderer.material;
        }
    }

    public void UnlockDoor()
    {
        if (!isLocked || isDissolving) return;

        isLocked = false;
        isDissolving = true;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (doorMaterial != null)
        {
            StartCoroutine(DissolveShader());
        }
        else
        {
            //Debug.LogWarning("No material found on door renderer.");
            Destroy(gameObject, 1f); 
        }
    }

    private IEnumerator DissolveShader()
    {
        float time = 0f;

        while (time < dissolveDuration)
        {
            float t = time / dissolveDuration;
            float cutoff = Mathf.Lerp(1f, 0f, t);
            doorMaterial.SetFloat(dissolveProperty, cutoff);
            time += Time.deltaTime;
            yield return null;
        }

        doorMaterial.SetFloat(dissolveProperty, 0f);
        Destroy(gameObject);
    }
}
