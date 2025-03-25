using Invector.vCharacterController;
using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    Animator anim;
    vThirdPersonController tcp;

    public bool isDodge;

    private void Start()
    {
        tcp = GetComponent<vThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDodge)
        {
            anim.SetTrigger("Dodge");
        }
    }
}
