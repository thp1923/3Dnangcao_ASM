using Invector.vCharacterController;
using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{

    vThirdPersonController cp;
    public float dodgeSpeed = 10f;
    public float dodgeCooldown = 1f;

    private bool isDodging = false;
    private bool canDodge = true;
    private Vector3 dodgeDirection;

    Animator aim;

    void Start()
    {
        cp = GetComponent<vThirdPersonController>();
        aim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        
        while(Time.time < startTime + dodgeCooldown)
        {
            cp.MoveCharacter(cp.moveDirection * dodgeSpeed * Time.deltaTime);

            yield return null;
        }
    }
    
}
