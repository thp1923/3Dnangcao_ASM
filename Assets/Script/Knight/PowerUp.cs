using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject buff;
    public GameObject heal;
    
    public float timeBuff;
    public float timeCD;
    float _timeCD;

    bool powerUp;
    float timeHeal;

    public int HealBonus;
    public int DamgeBonus;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        buff.SetActive(false);
        heal.SetActive(false);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeCD -= Time.deltaTime;
        OnPowerUp();
        if (powerUp)
        {
            buff.SetActive(true);
            heal.SetActive(true);
            timeHeal += Time.deltaTime;
            if (timeHeal >= 1)
            {
                GameSession.Instance.Heal(HealBonus);
                timeHeal = 0;
            }
        }
        else
        {
            buff.SetActive(false);
            heal.SetActive(false);
        }
    }

    private void OnPowerUp()
    {
        if(Input.GetKeyDown(KeyCode.E) && !powerUp && _timeCD <= 0)
        {
            PlayerAttackController.Instance.isEquipping = true;
            anim.SetTrigger("PowerUp");
            PowerUpOn();
            _timeCD = timeCD;
        }
    }

    public void PowerUpOn()
    {
        powerUp = true;
        GetComponent<AttackDamgePlayer>().Bonus(DamgeBonus);
        Invoke(nameof(PowerUpOff), timeBuff);
    }
    public void PowerUpOff()
    {
        powerUp = false;
        GetComponent<AttackDamgePlayer>().Bonus(-DamgeBonus);
    }
}
