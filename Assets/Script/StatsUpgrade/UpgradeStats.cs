using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeStats : MonoBehaviour
{
    public int Point;

    public int Level;

    public int Point_Lost = 10;

    private GameObject player;

    public enum StatsType
    {
        HP, 
        Attack, 
        Stamina,
        Def,
        Crit
    }


    [Header("---------Canva----------")]
    public GameObject StatsCanva;
    bool IsHide;

    #region Alive
    int currentHP_Max;
    int nextHP_Max;
    int currentDef;
    int nextDef;
    #endregion

    #region Attack
    int currentBaseATK;
    int nextBaseATK;
    float currentCritRate;
    float nextCritRate;
    float currentCritDamge;
    float nextCritDamge;
    #endregion

    #region Stamina
    int currentStaminaMax;
    int nextStaminaMax;
    #endregion

    private void Awake()
    {
        //so luong doi tuong GameSession
        int numbersession = FindObjectsOfType<UpgradeStats>().Length;
        //neu no co nhieu hon phien ban thi se huy no
        if (numbersession > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject); //khong cho huy khi load
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (player.GetComponent<PlayerTakeDamge>().isDeath) return;
        if(!PlayerAttackController.CursorLocked) return;
        if (!IsHide)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            player.GetComponent<vThirdPersonInput>().enabled = false;
            player.GetComponent<vThirdPersonController>().enabled = false;
            player.GetComponent<PlayerAttackController>().enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.GetComponent<vThirdPersonInput>().enabled = true;
            player.GetComponent<vThirdPersonController>().enabled = true;
            player.GetComponent<PlayerAttackController>().enabled = true;
        }
    }

    public void Upgrade(StatsType type)
    {
        if(Point < Point_Lost) return;
        Level++;
        Point_Lost = Mathf.FloorToInt(10 * Mathf.Pow(Level, 1.05f));
        switch (type)
        {
            case StatsType.HP:
                nextHP_Max = (int)(currentHP_Max * 1.1f);
                currentHP_Max = nextHP_Max;
                player.GetComponent<PlayerTakeDamge>().UpgradeAlive(currentHP_Max, currentDef);
                break;
            case StatsType.Def:
                nextDef = (int)(currentDef * 1.05f);
                currentDef = nextDef;
                player.GetComponent<PlayerTakeDamge>().UpgradeAlive(currentHP_Max, currentDef);
                break;
            case StatsType.Attack:
                nextBaseATK = (int)(currentBaseATK * 1.1f);
                currentBaseATK = nextBaseATK;
                player.GetComponent<AttackDamgePlayer>().UpgradeAttack(currentBaseATK, currentCritRate, currentCritDamge);
                break;
            case StatsType.Crit:
                nextCritRate += 1f;
                currentCritRate = nextCritRate;
                nextCritDamge += 2f;
                currentCritDamge = nextCritDamge;
                player.GetComponent<AttackDamgePlayer>().UpgradeAttack(currentBaseATK, currentCritRate, currentCritDamge);
                break;
            case StatsType.Stamina:
                nextStaminaMax = (int)(currentStaminaMax * 1.2f);
                currentStaminaMax = nextStaminaMax;
                player.GetComponent<Stamina>().UpgradeStamina(currentStaminaMax);
                break;
            default:
                break;
        }
    }

    void CurrentStats()
    {
        currentHP_Max = player.GetComponent<PlayerTakeDamge>().MaxHP;
        nextHP_Max = currentHP_Max;
        currentDef = player.GetComponent<PlayerTakeDamge>().Defense;
        nextDef = currentDef;
        currentBaseATK = player.GetComponent<AttackDamgePlayer>().BaseATK;
        nextBaseATK = currentBaseATK;
        currentCritRate = player.GetComponent<AttackDamgePlayer>().critRate;
        nextCritRate = currentCritRate;
        currentCritDamge = player.GetComponent<AttackDamgePlayer>().critDamge;
        nextCritDamge = currentCritDamge;
        currentStaminaMax = player.GetComponent<Stamina>().StaminaMax;
        nextStaminaMax = currentStaminaMax;
    }

    public void CanvaStats(bool isHide)
    {
        CurrentStats();
        IsHide = isHide;
        if (isHide)
        {
            StatsCanva.SetActive(false);
        }
        else
        {
            StatsCanva.SetActive(true);
            //lay index cua scene hien tai
            int currentsceneindex = SceneManager.GetActiveScene().buildIndex;
            //load lai scene hien tai

            SceneManager.LoadScene(currentsceneindex);
        }
    }
}
