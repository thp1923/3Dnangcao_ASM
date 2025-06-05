using Invector.vCharacterController;
using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    private Animator anim;
    internal bool isDodging = false;
    private Vector3 dodgeDirection;
    private Rigidbody rb;

    private vThirdPersonController controller; // Invector controller

    [Header("Dodge Settings")]
    public float dodgeSpeed = 6f;

    public Transform positionToSpawn;

    public KeyCode dodgeKey = KeyCode.Space;

    [Header("Skinned Mesh Relashed")]
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    [Header("Shader Relashed")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    [Header("Mesh Relashed")]
    public float meshRefreshRate = 0.1f;

    public float activeTime = 2f;
    public float meshDestroyDelay = 3f;

    private bool isActiveTrail;
    private void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<vThirdPersonController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
        if (isDodging && !isActiveTrail)
        {
            isActiveTrail = true;
            StartCoroutine(ActivateTrail(activeTime));
        }

        if (!isDodging && Input.GetKeyDown(dodgeKey))
        {
            anim.SetTrigger("Dodge");
        }
    }

    void FixedUpdate()
    {
        if (isDodging)
        {
            MoveDuringDodge();
        }
    }

    public void StartDodge()
    {
        isDodging = true;

        // Hướng cố định khi bắt đầu lăn
        dodgeDirection = transform.forward.normalized;

        // Khoá di chuyển và xoay của Invector
        controller.lockMovement = true;
        controller.lockRotation = true;
        

        // Reset lại velocity của Rigidbody
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void MoveDuringDodge()
    {
        Vector3 move = dodgeDirection * dodgeSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }


    // Gọi ở cuối animation bằng Event
    public void EndDodge()
    {
        isDodging = false;

        // Bật lại di chuyển và xoay
        controller.lockMovement = false;
        controller.lockRotation = false;

        // Reset lại Rigidbody và velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }



    IEnumerator ActivateTrail(float timeActive)
    {
        if (!isDodging && GetComponent<PlayerAttackController>().isAttacking) yield return null;
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

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isActiveTrail = false;
    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while(valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
