using UnityEngine;
using Invector.vCharacterController;

public class MoveManager : MonoBehaviour
{
    vThirdPersonController tcp;
    Animator anim;
    Rigidbody rb;
    public int staminaLost = 3;
    public float timeLostStaminaRun;
    float _timeLostStaminaRun;
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
        _timeLostStaminaRun -= Time.deltaTime;

        if (anim.GetBool("IsSprinting") && _timeLostStaminaRun <= 0 && GetComponent<Stamina>().stamina >= staminaLost)
        {
            GetComponent<Stamina>().TakeStamina(staminaLost);
            _timeLostStaminaRun = timeLostStaminaRun;
        }
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

    public void CheckDrag(bool Drag)
    {
        if (Drag)
            rb.drag = 100f;
        else
            rb.drag = 0f;
    }
}
