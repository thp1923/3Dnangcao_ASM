using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] Transform s_point;

    [SerializeField] Transform e_point;

    [SerializeField] LayerMask hitLayer;

    string current_wallType;

    [Header("Setting")]

    [SerializeField] int max_hitCount = 3;

    int hitCount = 3;

    [SerializeField] float max_recoverTime = 0.1f;

    float recoverTime = 0.1f;

    [Header("Addon")]

    [SerializeField] AudioSource audioSource;

    [Tooltip("Concrete ,Blood")]  [SerializeField] GameObject[] particleEffects;

    [Tooltip("Concrete ,Blood")]  [SerializeField] AudioClip[] audioClips;

    public void ResetSettings()
    {
        //recoverTime = 0;
        hitCount = max_hitCount;
    }

    public void Activate()
    {
        if(recoverTime > 0)
        {
            recoverTime -= Time.deltaTime;
            return;
        }

        ShootRay();

    }

    private void ShootRay()
    {
        RaycastHit hit;
        if(Physics.Linecast(s_point.position, e_point.position, out hit, hitLayer))
        {
            current_wallType = hit.transform.tag;

            if (current_wallType == "Untagged") current_wallType = "Concrete";

            switch(hit.transform.tag)
            {
                case "Concrete":
                    Hit(0, hit);
                    break;
                case "Blood":
                    Hit(1, hit);
                    break;
                default:
                    break;
            }
        }
    }

    private void Hit(int particleType, RaycastHit hit)
    {
        if(hitCount > 0)
        {
            PlayerAttackController.Instance.SwordContract();
            audioSource.PlayOneShot(audioClips[particleType]);
            Instantiate(particleEffects[particleType], hit.point, Quaternion.LookRotation(hit.normal));
            hitCount--;
            recoverTime = max_recoverTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(s_point.position, e_point.position);
    }
}
