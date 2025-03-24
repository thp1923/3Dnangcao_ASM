using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.3f;
    public float dodgeCooldown = 1f;

    private bool isDodging = false;
    private bool canDodge = true;
    private Vector3 dodgeDirection;
    private Rigidbody rb;

    Animator aim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging && canDodge)
        {
            StartCoroutine(PerformDodge());
        }
    }

    private System.Collections.IEnumerator PerformDodge()
    {
        isDodging = true;
        canDodge = false;

        // Lấy hướng hiện tại (di chuyển hoặc nhìn)
        dodgeDirection = transform.forward;

        float timer = 0f;

        while (timer < dodgeDuration)
        {
            aim.SetTrigger("Dodge");
            rb.MovePosition(dodgeDirection * dodgeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDodging = false;

        // Chờ cooldown
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
}
