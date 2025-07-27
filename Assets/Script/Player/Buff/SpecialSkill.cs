using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkill : MonoBehaviour
{
    
    public enum SpecialSkillTpye
    {
        GreenFire = 0, DragonFire = 1
    }
    [Header("-----Skill Controller-----")]

    public SpecialSkillTpye skillTpye;

    public KeyCode SpecialSkillKey = KeyCode.Q;

    Animator animator;
    public LayerMask attackMask;

    [Header("-----Green Fire-----")]
    public ParticleSystem[] fireEffect;

    protected int skillDamge;

    protected float damgeTakeNerf;

    public float rangeGreenFire;



    //[Header("-----Dragon Fire-----")]

    //public ParticleSystem[] fireDragonEffect;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
}
