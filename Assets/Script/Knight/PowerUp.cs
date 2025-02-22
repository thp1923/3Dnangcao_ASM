using Invector.vCharacterController;
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

    [Header("----------CD--------")]
    public GameObject power;

    public TMPro.TextMeshProUGUI powerCD;
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
        powerCD.text = _timeCD.ToString("F1");
        OnPowerUp();
        IconPower();
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

    void IconPower()
    {
        if(!powerUp && _timeCD <= 0) power.SetActive(false);
        else power.SetActive(true);
    }

    private void OnPowerUp()
    {
        if(Input.GetKeyDown(KeyCode.E) && !powerUp && _timeCD <= 0)
        {
            GetComponent<AudioPlayer>().PlayAudio(3);
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
        GetComponent<AudioPlayer>().PlayAudioAlways(4);
        Invoke(nameof(PowerUpOff), timeBuff);
        //anim.speed += 0.5f;
        //GetComponent<vThirdPersonController>().moveSpeed += 10;
    }
    public void PowerUpOff()
    {
        powerUp = false;
        GetComponent<AudioPlayer>().PlayAudioStop(4);
        GetComponent<AttackDamgePlayer>().Bonus(-DamgeBonus);
        //GetComponent<vThirdPersonController>().moveSpeed -= 10;
        //anim.speed -= 0.5f;
    }
}
