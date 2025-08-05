using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtoriasBuffAndEffect : MonoBehaviour
{
    public ParticleSystem[] buffEffect;
    public GameObject[] fireBalls;
    public float timeFireBallShot;
    float _timeFireBallShot;

    Animator animator;

    public int buffATK;

    bool isBuff;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        foreach (var effect in buffEffect)
        {
            effect.Stop();
        }
        foreach (var effect in fireBalls)
        {
            effect.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        FireBall();
        if(!isBuff && GetComponent<EnemyTakeDamge>().isHurt)
        {
            animator.SetTrigger("Buff");
            
        }
    }

    void FireBall()
    {
        _timeFireBallShot -= Time.deltaTime;
        if(isBuff && _timeFireBallShot <= 0)
        {
            _timeFireBallShot = timeFireBallShot;
            foreach (var effect in fireBalls)
            {
                effect.SetActive(true);
            }
        }
    }

    public void Buff()
    {
        isBuff = true;
        GetComponent<ArtoriasAttackDamge>().BaseATK += buffATK;
        foreach (var effect in buffEffect)
        {
            effect.Play();
        }
    }
}
