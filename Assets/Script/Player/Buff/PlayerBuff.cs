using System.Collections;
using UnityEngine.VFX;
using UnityEngine;
using System.Collections.Generic;

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
    public AudioSource buffSource;
    public AudioSource fireLoop;

    public KeyCode BuffKey = KeyCode.E;

    [Header("--------CD--------")]
    public float CD;
    float _CD;
    [Header("--------Atk--------")]
    public int atkBonus;
    public int stunDamgeBonus;
    public ParticleSystem[] attackEffect;
    public VisualEffect effect;
    public Color colorEffect;
    public AudioClip atkClip;
    private Color currentColor;
    [Header("--------Def--------")]
    public int defBonus;
    public int stunDefBonus;
    public Material defMaterial;
    public GameObject targetRoot;
    public List<SkinnedMeshRenderer> skinnedMeshes;
    public AudioClip defClip;

    private void Start()
    {
        Collect();
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
                atp.stunDamgeBonus += stunDamgeBonus;
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
            case ClassPlayer.Def:
                ptd.stunResistanceBonus += stunDefBonus;
                ptd.defenseBonus += defBonus;
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
        switch (classPlayer)
        {
            case ClassPlayer.Atk:
                atp.atkBonus -= atkBonus;
                atp.stunDamgeBonus -= stunDamgeBonus;
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
                break;
            case ClassPlayer.Def:
                ptd.stunResistanceBonus -= stunDefBonus;
                ptd.defenseBonus -= defBonus;
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
                break;
            default:
                break;
        }
    }
}
