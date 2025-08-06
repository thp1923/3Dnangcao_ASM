using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StatsManager
{
    public class StatsAlive : MonoBehaviour
    {
        #region Public
        public int MaxHP; // Máu tối đa ko phải máu dùng trong thực tế
        public int Defense; // Chỉ số phòng thủ
        public int StunResistance; // Chỉ sô kháng stun
        public Slider HpSlider;
        public Slider HpLostSlider;

        public enum TypeTakeDamge
        {
            Only,
            Branch
        }
        public TypeTakeDamge type;
        #endregion

        internal int defenseBonus;
        internal int defenseBonusSkill;
        internal float damgeTake;
        internal int stunResistanceBonus;

        protected int DefenseMax = 2000; // Tối đa phòng thử đạt đc
        protected int StunResistanceMax = 2000; // Tối đa kháng stun đạt đc
        protected int currentHP;

        protected int DamPopUp;

        public float timer;
        protected float _timer;

        protected virtual void Start()
        {
            currentHP = MaxHP;
            if (HpSlider != null)
            {
                HpSlider.maxValue = MaxHP;
                HpSlider.value = currentHP;
            }
            if(HpLostSlider != null)
            {
                HpLostSlider.maxValue = MaxHP;
                HpLostSlider.value = currentHP;
            }
        }

        protected virtual void Update()
        {
            _timer -= Time.deltaTime;
            if (HpLostSlider != null)
            {
                if (HpSlider.value < HpLostSlider.value && _timer <= 0)
                {
                    _timer = timer;
                    HpLostSlider.value -= (int)(MaxHP * 0.05f);
                
                }
                if(HpSlider.value > HpLostSlider.value)
                {
                    HpLostSlider.value = HpSlider.value;
                }
            }
        }

        public virtual void TakeDamge(int damge, int stunDamge, int trueDamge)
        {
            int totalDefense = Mathf.Clamp(Defense + defenseBonus + defenseBonusSkill, 0, DefenseMax);
            int totalStunRes = Mathf.Clamp(StunResistance + stunResistanceBonus, 0, StunResistanceMax);

            // Hệ số kháng stun ảnh hưởng đến hiệu quả phòng thủ (0.0 - 1.0)
            float stunResFactor = 1f - (totalStunRes / (float)StunResistanceMax); // càng nhiều kháng stun, càng ít bị giảm hiệu quả phòng thủ

            // Hệ số giảm sát thương từ phòng thủ (tối đa giảm ~90%)
            float defenseEffectiveness = totalDefense * stunResFactor / DefenseMax; // 0 - ~1
            float defenseFactor = Mathf.Clamp01(1f - defenseEffectiveness * 0.9f); // giảm damage tối đa 90%

            // Hệ số chịu thêm damage (nếu damgeTake > 0)
            float damgeTakeFactor = Mathf.Clamp01(1f - damgeTake / 100f);

            // Tính damage cuối cùng
            int finalDamage = Mathf.FloorToInt(damge * defenseFactor * damgeTakeFactor);
            finalDamage = Mathf.Max(finalDamage, 1); // ít nhất luôn gây 1 damage

            // Trừ máu
            currentHP -= finalDamage + trueDamge;
            currentHP = Mathf.Max(currentHP, 0);

            // Cập nhật thanh máu
            if (HpSlider != null)
                HpSlider.value = currentHP;

            DamPopUp = finalDamage + trueDamge;
            //if(stunDamge > StunResistance)
            //{
            //    // sẽ chạy stun tùy theo mức độ
            //}
        }

        public virtual void UpgradeAlive(int HP_Upgrade, int Def_Upgrade)
        {
            MaxHP = HP_Upgrade;
            HpSlider.maxValue = HP_Upgrade;
            Defense = Def_Upgrade;
        }
    }

    public class StatsAttack : MonoBehaviour
    {
        #region Public
        [Header("-----Atk Stats-----")]
        public List<float> ATK; //Phần trăm damge của đòn đánh
        public int BaseATK;
        public float critRate;
        public float critDamge;
        public int[] stunDamge;
        #endregion

        internal int atkBonus;
        internal float damgeAttack;
        internal int atkBonusSkill;
        internal int stunDamgeBonus;
        internal float critRateBonus;
        internal float critDamgeBonus;
        protected int atk;

        public virtual void Attack(int attackNumber)
        {
            float totalCritRate = critRate + critRateBonus;          // Ví dụ: 120%
            float totalCritDamage = critDamge + critDamgeBonus;

            // Tính damage gốc
            int damge = Mathf.FloorToInt(((BaseATK + atkBonus + atkBonusSkill) * (ATK[attackNumber] / 100f)) * ((100f + damgeAttack) / 100f));

            // Tính toán phần critRate vượt quá 100%
            if (totalCritRate > 100f)
            {
                float overflowRate = totalCritRate - 100f;
                totalCritDamage += overflowRate * 1.5f;
                totalCritRate = 100f; // Giới hạn về 100%
            }

            // Kiểm tra có crit hay không
            if (Random.Range(0f, 1f) <= (totalCritRate / 100f))
            {
                atk = Mathf.FloorToInt(damge * (1f + totalCritDamage / 100f));
            }
            else
            {
                atk = damge;
            }
        }

        public virtual void UpgradeAttack(int BaseATK_Upgrade, float critRate_Upgrade, float critDamge_Upgrade)
        {
            BaseATK = BaseATK_Upgrade;
            critRate = critRate_Upgrade;
            critDamge = critDamge_Upgrade;
        }
    }

    public class StatsStamina : MonoBehaviour
    {
        public int StaminaMax;
        public int StaminaRecover;
        public float RecoverStaminaTime;

        protected float _recoverStaminaTime;
        internal int stamina;
        internal bool canRecover;

        public float timer;
        protected float _timer;

        public Slider staminaBar;
        public Slider staminaLostBar;

        protected virtual void Start()
        {
            stamina = StaminaMax;
            staminaBar.maxValue = StaminaMax;
            staminaBar.value = stamina;
            staminaLostBar.maxValue = StaminaMax;
            staminaLostBar.value = stamina;
        }

        public virtual void UpgradeStamina(int StaminaMax_Upgrade)
        {
            StaminaMax = StaminaMax_Upgrade;
            staminaBar.maxValue = StaminaMax_Upgrade;
        }

        public virtual void TakeStamina(int staminaLost)
        {
            canRecover = false;
            stamina -= staminaLost;
        }

        protected virtual void Update()
        {
            _timer -= Time.deltaTime;
            if(staminaBar.value < staminaLostBar.value && _timer <= 0)
            {
                _timer = timer;
                staminaLostBar.value -= (int)(StaminaMax * 0.05f);
            }
            if(staminaBar.value >  staminaLostBar.value)
            {
                staminaLostBar.value = staminaBar.value;
            }
            RecoveStamina();
            staminaBar.value = stamina;
        }

        public virtual void RecoveStamina()
        {
            if (!canRecover) return;

            _recoverStaminaTime -= Time.deltaTime;

            if (stamina < StaminaMax && _recoverStaminaTime <= 0)
            {
                stamina += StaminaRecover;
                _recoverStaminaTime = RecoverStaminaTime;
            }
            else if (stamina > StaminaMax)
            {
                stamina = StaminaMax;
            }
        }
    }
}
