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

        public enum TypeTakeDamge
        {
            Only,
            Branch
        }
        public TypeTakeDamge type;
        #endregion

        internal int defenseBonus;
        internal int stunResistanceBonus;

        protected int DefenseMax = 2000; // Tối đa phòng thử đạt đc
        protected int StunResistanceMax = 2000; // Tối đa kháng stun đạt đc
        protected int currentHP;

        protected int DamPopUp;

        protected virtual void Start()
        {
            currentHP = MaxHP;
            if (HpSlider != null)
            {
                HpSlider.maxValue = MaxHP;
                HpSlider.value = currentHP;
            }
        }

        public virtual void TakeDamge(int damge, int stunDamge, int trueDamge)
        {
            int Damge = Mathf.FloorToInt(damge 
                * (1 - Mathf.Clamp(Defense + defenseBonus, 0, DefenseMax)
                * (1 - Mathf.Clamp(StunResistance + stunResistanceBonus, 0, StunResistanceMax) / StunResistanceMax)/ 2500));
            Damge = Mathf.Max(Damge, 1); // luôn gây damge ít nhất là 1
            currentHP -= Damge + trueDamge;
            HpSlider.value = currentHP;
            DamPopUp = Damge + trueDamge;
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
        internal int stunDamgeBonus;
        internal float critRateBonus;
        internal float critDamgeBonus;
        protected int atk;

        public virtual void Attack(int attackNumber)
        {
            int damge = Mathf.FloorToInt((BaseATK + atkBonus) * (ATK[attackNumber]/100));
            if(Random.Range(0, 1f) <= (critRate+critRateBonus))
            {
                atk = Mathf.FloorToInt(damge * (1f + (critDamge+critDamgeBonus)/100));
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

        public Slider staminaBar;

        protected virtual void Start()
        {
            stamina = StaminaMax;
            staminaBar.maxValue = StaminaMax;
            staminaBar.value = stamina;
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
