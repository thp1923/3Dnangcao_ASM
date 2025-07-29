using UnityEngine.VFX;
using UnityEngine;

public class SwordTrailEffect : MonoBehaviour
{
    public VisualEffect trailEffect;

    public bool isFlame;

    public ParticleSystem fireTrail;

    bool damageEnabled;

    [SerializeField] MeleeWeapon currentWeapon;
    private void Start()
    {
        trailEffect.gameObject.SetActive(false);
        if(fireTrail != null)
        {
            fireTrail.gameObject.SetActive(false);
            fireTrail.Stop();
        }

    }

    private void Update()
    {
        if(damageEnabled) currentWeapon.Activate();
    }

    public void PlayPartical(int number)
    {
        if (number != 0)
        {
            damageEnabled = true;
            if (isFlame && fireTrail != null)
            {
                fireTrail.Play();
            }
            else
            {
                trailEffect.SetBool("UseForce", true);
                trailEffect.gameObject.SetActive(true);
            }
        }
        else
        {
            damageEnabled = false;
            currentWeapon.ResetSettings();
            if (isFlame && fireTrail != null)
            {
                fireTrail.Stop();
            }
            else
            {
                trailEffect.SetBool("UseForce", false);
                trailEffect.gameObject.SetActive(false);
            }
        }
    }

    public void PlayFlame(bool IsFlame)
    {
        if (isFlame)
            fireTrail.gameObject.SetActive(IsFlame);
        else
            fireTrail.gameObject.SetActive(false);
    }
}
