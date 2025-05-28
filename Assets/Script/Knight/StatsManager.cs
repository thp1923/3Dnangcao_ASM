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

        protected int currentHP;

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
            int Damge = Mathf.FloorToInt((damge / (Defense + defenseBonus)) * 1.14f);
            Damge = Mathf.Max(Damge, 1); // luôn gây damge ít nhất là 1
            currentHP -= Damge + trueDamge;
            HpSlider.value = currentHP;
            //if(stunDamge > StunResistance)
            //{
            //    // sẽ chạy stun tùy theo mức độ
            //}
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
        protected int atk;

        public virtual void Attack(int attackNumber)
        {
            int damge = Mathf.FloorToInt((BaseATK + atkBonus) * (ATK[attackNumber]/100));
            if(Random.Range(0, 1f) <= critRate)
            {
                atk = Mathf.FloorToInt(damge * (1f + critDamge));
            }
            else
            {
                atk = damge;
            }
        }
    }

    public class StatsStamina : MonoBehaviour
    {
        public int StaminaMax;
        public int StaminaRecover;
        public float RecoverStaminaTime;

        protected float _recoverStaminaTime;
        internal int stamina;

        public virtual void LostStamina(int staminaLost)
        {
            stamina -= staminaLost;
        }

        protected virtual void Update()
        {
            RecoveStamina();
        }

        public void RecoveStamina()
        {
            _recoverStaminaTime -= Time.deltaTime;
            if (stamina < StaminaMax && _recoverStaminaTime < RecoverStaminaTime)
            {
                stamina += StaminaRecover;
            }
            else
                stamina = StaminaMax; return;
        }
    }
}
