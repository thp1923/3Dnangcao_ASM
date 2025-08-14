using System.Collections;
using UnityEngine.VFX;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlayerBuff : MonoBehaviour
{
    public enum BuffType
    {
        Atk = 0, Def = 1
    }

    internal bool canBuff;

    public BuffType buffTypePlayer;

    AttackDamgePlayer atp;
    PlayerTakeDamge ptd;
    Animator animator;
    public AudioSource buffSource;
    public AudioSource fireLoop;

    public KeyCode BuffKey = KeyCode.E;

    [Header("--------CD--------")]
    public float CD;
    public GameObject CD_Panal;
    public TextMeshProUGUI cdText;
    float _CD;
    [Header("--------Atk--------")]
    public float atkBonus;
    public int stunDamgeBonus;
    public ParticleSystem[] attackEffect;
    public VisualEffect effect;
    public Color colorEffect;
    public AudioClip atkClip;
    private Color currentColor;
    bool isFireSword;
    [Header("--------Def--------")]
    public float defBonus;
    public int stunDefBonus;
    public Material defMaterial;
    public GameObject targetRoot;
    public List<SkinnedMeshRenderer> skinnedMeshes;
    public AudioClip defClip;
    bool isBodyShield;

    int aBonus;
    int dBonus;

    internal int buffTypeId; // Lưu loại buff

    private void Start()
    {
        Collect();
        CD_Panal.SetActive(false);
        buffTypePlayer = (BuffType)buffTypeId;
        atp = GetComponent<AttackDamgePlayer>();
        ptd = GetComponent<PlayerTakeDamge>();
        animator = GetComponent<Animator>();
        Vector4 colorVec = effect.GetVector4("Color");
        currentColor = new Color(colorVec.x, colorVec.y, colorVec.z, colorVec.w);
        foreach (var atkEf in attackEffect)
        {
            atkEf.Stop();
            atkEf.GetComponent<Light>().enabled = false;
        }
    }

    void Collect()
    {
        if (targetRoot == null)
        {
            return;
        }

        skinnedMeshes.Clear(); // Xoá dữ liệu cũ tránh lỗi
        skinnedMeshes.AddRange(targetRoot.GetComponentsInChildren<SkinnedMeshRenderer>());
    }

    private void Update()
    {
        _CD -= Time.deltaTime;
        if(CD_Panal.activeSelf)
            cdText.text = _CD.ToString("F1");
        if(_CD > 0)
        {
            CD_Panal.SetActive(true);
        }
        else
        {
            CD_Panal.SetActive(false);
        }
        if (Input.GetKeyDown(BuffKey) && _CD <= 0 && canBuff)
        {
            _CD = CD;
            buffTypeId = (int)(buffTypePlayer);
            switch (buffTypePlayer)
            {
                case BuffType.Atk:
                    animator.SetTrigger("BuffATK");
                    break;
                case BuffType.Def:
                    animator.SetTrigger("BuffDEF");
                    break;
                default:
                    break;
            }
        }
    }

    public void Buff()
    {
        switch (buffTypePlayer)
        {
            case BuffType.Atk:
                aBonus = (int)(atp.BaseATK * (atkBonus / 100f));
                isFireSword = true;
                atp.atkBonusSkill = aBonus;
                atp.stunDamgeBonus = stunDamgeBonus;
                GetComponent<AudioPlayer>().isBuff = true;
                GetComponent<SwordTrailEffect>().isFlame = true;
                buffSource.PlayOneShot(atkClip);
                fireLoop.Play();
                effect.SetVector4("Color", (Vector4)colorEffect);
                foreach (var atkEf in attackEffect)
                {
                    atkEf.Play();
                    atkEf.GetComponent<Light>().enabled = true;
                }
                break;
            case BuffType.Def:
                dBonus = (int)(ptd.Defense * (defBonus / 100f));
                isBodyShield = true;
                ptd.stunResistanceBonus = stunDefBonus;
                ptd.defenseBonusSkill = dBonus;
                buffSource.PlayOneShot(defClip);
                foreach (var renderer in skinnedMeshes)
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
        if (isFireSword)
        {
            atp.atkBonusSkill = 0;
            atp.stunDamgeBonus = 0;
            GetComponent<AudioPlayer>().isBuff = false;
            GetComponent<SwordTrailEffect>().isFlame = false;
            buffSource.PlayOneShot(atkClip);
            fireLoop.Stop();
            effect.SetVector4("Color", (Vector4)currentColor);
            foreach (var atkEf in attackEffect)
            {
                atkEf.Stop();
                atkEf.GetComponent<Light>().enabled = false;
            }
            isFireSword = false;
        }
        else if (isBodyShield)
        {
            ptd.stunResistanceBonus = 0;
            ptd.defenseBonusSkill = 0;
            foreach (var renderer in skinnedMeshes)
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
            isBodyShield = false;
        }
    }
}
