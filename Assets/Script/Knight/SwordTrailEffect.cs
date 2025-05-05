using Unity.VisualScripting;
using UnityEngine;

public class SwordTrailEffect : MonoBehaviour
{
    public ParticleSystem trailEffect;

    bool damageEnabled;

    [SerializeField] MeleeWeapon currentWeapon;
    private void Start()
    {
        trailEffect.Stop();
    }

    private void Update()
    {
        if(damageEnabled) currentWeapon.Activate();
    }

    public void PlayPartical(int number)
    {
        if (number != 0)
        {
            trailEffect.Play();
            damageEnabled = true;
        }
        else
        {
            trailEffect.Stop();
            damageEnabled = false;
            currentWeapon.ResetSettings();
        }
    }
}
