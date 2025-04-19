using UnityEngine;
using Invector.vCharacterController;

public class MoveManager : MonoBehaviour
{
    vThirdPersonController tcp;
    Animator anim;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        tcp = GetComponent<vThirdPersonController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckLockMove(bool Lock)
    {
        tcp.lockMovement = Lock;
        tcp.lockRotation = Lock;
        if (Lock)
            anim.SetFloat("InputMagnitude", -1f);
        else
            return;
    }

    public void CheckSleep(bool Check)
    {
        if (Check)
            rb.Sleep();
        else 
            rb.WakeUp();
    }
}
