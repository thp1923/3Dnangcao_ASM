using Invector.vCharacterController;
using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    Animator anim;
    vThirdPersonController tcp;

    public bool isDodge;

    public Transform positionToSpawn;

    [Header("Skinned Mesh Relashed")]
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    [Header("Shader Relashed")]
    public Material mat;

    [Header("Mesh Relashed")]
    public float meshRefreshRate = 0.1f;

    public float activeTime = 2f;
    public float meshDestroyDelay = 3f;

    private bool isActiveTrail;
    private void Start()
    {
        tcp = GetComponent<vThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDodge && !isActiveTrail)
        {
            isActiveTrail = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isDodge)
        {
            anim.SetTrigger("Dodge");
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while(timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for(int i = 0; i<skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetLocalPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;

                mr.material = mat;

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isActiveTrail = false;
    }
}
