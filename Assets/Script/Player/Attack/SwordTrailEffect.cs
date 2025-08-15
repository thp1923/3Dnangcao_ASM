using UnityEngine.VFX;
using UnityEngine;
using Tiny;

public class SwordTrailEffect : MonoBehaviour
{
    public VisualEffect trailEffect;

    public Trail swordTrail;

    public bool isFlame;

    public ParticleSystem[] fireTrails;

    bool damageEnabled;

    [SerializeField] MeleeWeapon currentWeapon;
    private void Start()
    {
        if(trailEffect != null) 
            trailEffect.gameObject.SetActive(false);
        if(fireTrails != null)
        {
            foreach(var fireTrail in fireTrails)
            {
                fireTrail.gameObject.SetActive(false);
                fireTrail.Stop();
            }
        }
        if(swordTrail != null)
            swordTrail.enabled = false;
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
            if (isFlame && fireTrails != null)
            {
                foreach (var fireTrail in fireTrails)
                {
                    fireTrail.Play();
                }
            }
            else
            {
                if (trailEffect != null)
                {
                    trailEffect.SetBool("UseForce", true);
                    trailEffect.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            damageEnabled = false;
            currentWeapon.ResetSettings();
            if (isFlame && fireTrails != null)
            {
                foreach (var fireTrail in fireTrails)
                {
                    fireTrail.Stop();
                }
            }
            else
            {
                if (trailEffect != null)
                {
                    trailEffect.SetBool("UseForce", false);
                    trailEffect.gameObject.SetActive(false);
                }
            }
        }
    }

    public void PlayParticalSwordSpeacial(int number)
    {
        if (number != 0)
        {
            damageEnabled = true;
            if (isFlame && fireTrails != null)
            {
                foreach (var fireTrail in fireTrails)
                {
                    fireTrail.Play();
                }
            }
            else
            {
                if (swordTrail != null)
                    swordTrail.enabled = true;
            }
        }
        else
        {
            damageEnabled = false;
            currentWeapon.ResetSettings();
            if (isFlame && fireTrails != null)
            {
                foreach (var fireTrail in fireTrails)
                {
                    fireTrail.Stop();
                }
            }
            else
            {
                if (swordTrail != null)
                    swordTrail.enabled = false;
            }
        }
    }

    public void PlayFlame(bool IsFlame)
    {
        if (isFlame)
        {
            foreach (var fireTrail in fireTrails)
            {
                fireTrail.gameObject.SetActive(IsFlame);
            }
        }
        else
        {
            foreach (var fireTrail in fireTrails)
            {
                fireTrail.gameObject.SetActive(false);
            }
        }
    }
}
