using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    public enum ClassPlayer
    {
        Atk, Def
    }

    public ClassPlayer classPlayer;

    AttackDamgePlayer atp;
    PlayerTakeDamge ptd;
    Animator animator;

    public KeyCode BuffKey;

    [Header("--------CD--------")]
    public float CD;
    float _CD;
    [Header("--------Atk--------")]
    public int atkBonus;
    public ParticleSystem[] attackEffect;
    [Header("--------Def--------")]
    public int defBonus;
    public int stunDefBonus;
    public Material defMaterial;
    public SkinnedMeshRenderer[] skin;

    private void Start()
    {
        atp = GetComponent<AttackDamgePlayer>();
        ptd = GetComponent<PlayerTakeDamge>();
        animator = GetComponent<Animator>();
        foreach (var atkEf in attackEffect)
        {
            atkEf.Stop();
            atkEf.GetComponent<Light>().enabled = false;
        }
    }

    private void Update()
    {
        _CD -= Time.deltaTime;
        if (Input.GetKeyDown(BuffKey) && _CD <= 0)
        {
            _CD = CD;
            switch (classPlayer)
            {
                case ClassPlayer.Atk:
                    animator.SetTrigger("BuffATK");
                    break;
                case ClassPlayer.Def:
                    animator.SetTrigger("BuffDEF");
                    break;
                default:
                    break;
            }
        }
    }

    public void Buff()
    {
        switch (classPlayer)
        {
            case ClassPlayer.Atk:
                atp.atkBonus += atkBonus;
                foreach (var atkEf in attackEffect)
                {
                    atkEf.Play();
                    atkEf.GetComponent<Light>().enabled = true;
                }
                break;
            case ClassPlayer.Def:
                ptd.stunResistanceBonus += stunDefBonus;
                ptd.defenseBonus += defBonus;
                foreach (var renderer in skin)
                {
                    // Kiểm tra nếu đã có thì không thêm nữa
                    if (System.Array.Exists(renderer.materials, mat => mat == defMaterial))
                        continue;

                    Material[] currentMaterials = renderer.materials;
                    Material[] newMaterials = new Material[currentMaterials.Length + 1];

                    for (int i = 0; i < currentMaterials.Length; i++)
                        newMaterials[i] = currentMaterials[i];

                    newMaterials[currentMaterials.Length] = defMaterial;
                    renderer.materials = newMaterials;
                }
                break;
            default:
                break;
        }

        StartCoroutine(EndBuff());
        _CD = CD;
    }

    IEnumerator EndBuff()
    {
        yield return new WaitForSeconds(CD/3);
        switch (classPlayer)
        {
            case ClassPlayer.Atk:
                atp.atkBonus -= atkBonus;
                foreach (var atkEf in attackEffect)
                {
                    atkEf.Stop();
                    atkEf.GetComponent<Light>().enabled = false;
                }
                break;
            case ClassPlayer.Def:
                ptd.stunResistanceBonus -= stunDefBonus;
                ptd.defenseBonus -= defBonus;
                foreach (var renderer in skin)
                {
                    Material[] mats = renderer.materials;

                    if (mats.Length == 0)
                        continue;

                    // Tạo mảng mới ngắn hơn 1
                    Material[] newMats = new Material[mats.Length - 1];

                    // Copy hết trừ cái cuối cùng
                    for (int i = 0; i < newMats.Length; i++)
                    {
                        newMats[i] = mats[i];
                    }

                    // Gán lại mảng đã rút gọn
                    renderer.materials = newMats;
                }
                break;
            default:
                break;
        }
    }
}
