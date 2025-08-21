using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        recoverTime = 0;
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

            //if (current_wallType == "Player") current_wallType = "Concrete";

            switch(hit.transform.tag)
            {
                case "Player":
                    if(FindAnyObjectByType<PlayerDodge>().isDodging) return;
                    Hit(0, hit);
                    break;
                case "Enemy":
                    Hit(0, hit);
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

            // dùng máu dạng decal của VolumetricBloodFX
            GameObject prefab = particleEffects[Random.Range(0, particleEffects.Length)];
            Vector3 offsetPos = hit.point + hit.normal * 0.01f;
            Quaternion rot = Quaternion.LookRotation(-hit.normal);

            GameObject blood = Instantiate(prefab, offsetPos, rot);

            var settings = blood.GetComponent<BFX_BloodSettings>();
            if (settings != null)
            {
                settings.LightIntensityMultiplier = 1;
                settings.AnimationSpeed = 1;
                settings.FreezeDecalDisappearance = true;
            }

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
