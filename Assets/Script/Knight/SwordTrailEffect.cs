using UnityEngine.VFX;
using UnityEngine;

public class SwordTrailEffect : MonoBehaviour
{
    public VisualEffect trailEffect;

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
            trailEffect.gameObject.SetActive(true);
            damageEnabled = true;
        }
        else
        {
            trailEffect.gameObject.SetActive(false);
            damageEnabled = false;
            currentWeapon.ResetSettings();
        }
    }
}
