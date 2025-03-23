using UnityEngine;

public class SwordTrailEffect : MonoBehaviour
{
    public ParticleSystem trailEffect;

    private void Start()
    {
        trailEffect.Stop();
    }
    public void PlayPartical(int number)
    {
        if (number == 0)
            trailEffect.Play();
        else
            trailEffect.Stop();
    }
}
