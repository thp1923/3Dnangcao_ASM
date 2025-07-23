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

    public TMPro.TextMeshProUGUI LevelText;

    public TMPro.TextMeshProUGUI PointText;

    public TMPro.TextMeshProUGUI PointLostText;

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

    [Header("---------Alive----------")]

    #region Alive
    int currentHP_Max;
    int nextHP_Max;
    public TMPro.TextMeshProUGUI HP;
    public TMPro.TextMeshProUGUI NextHP;
    int currentDef;
    int nextDef;
    public TMPro.TextMeshProUGUI Def;
    public TMPro.TextMeshProUGUI NextDef;
    #endregion

    [Header("---------Attack----------")]

    #region Attack
    int currentBaseATK;
    int nextBaseATK;
    public TMPro.TextMeshProUGUI BaseATK;
    public TMPro.TextMeshProUGUI NextBaseATK;
    float currentCritRate;
    float nextCritRate;
    public TMPro.TextMeshProUGUI CritRate;
    public TMPro.TextMeshProUGUI NextCritRate;
    float currentCritDamge;
    float nextCritDamge;
    public TMPro.TextMeshProUGUI CritDamge;
    public TMPro.TextMeshProUGUI NextCritDamge;
    #endregion

    [Header("---------Stamina----------")]

    #region Stamina
    int currentStaminaMax;
    int nextStaminaMax;
    public TMPro.TextMeshProUGUI Stamina;
    public TMPro.TextMeshProUGUI NextStamina;
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

    public void AddPoint(int addPoint)
    {
        Point += addPoint;
    }
    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) return;
        }
        if (player.GetComponent<PlayerTakeDamge>().isDeath) return;
        if(!PlayerAttackController.CursorLocked) return;
        if (IsHide)
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
        if (Point < Point_Lost)
        {
            Debug.Log("Not Enought Point");
            return;
        }
        Level++;
        Point -= Point_Lost;
        Point_Lost = Mathf.FloorToInt(10 * Mathf.Pow(Level, 1.05f));

        LevelText.text = ("Level ") + Level.ToString();
        PointText.text = ("Point: ") + Point.ToString();
        PointLostText.text = ("Need Point: ") + PointLostText.ToString();
        switch (type)
        {
            case StatsType.HP:
                nextHP_Max = (int)(currentHP_Max * 1.1f);
                currentHP_Max = nextHP_Max;
                NextHP.text = currentHP_Max.ToString();
                player.GetComponent<PlayerTakeDamge>().UpgradeAlive(currentHP_Max, currentDef);
                break;
            case StatsType.Def:
                nextDef = (int)(currentDef * 1.05f);
                currentDef = nextDef;
                NextDef.text = currentDef.ToString();
                player.GetComponent<PlayerTakeDamge>().UpgradeAlive(currentHP_Max, currentDef);
                break;
            case StatsType.Attack:
                nextBaseATK = (int)(currentBaseATK * 1.1f);
                currentBaseATK = nextBaseATK;
                NextBaseATK.text = currentBaseATK.ToString();
                player.GetComponent<AttackDamgePlayer>().UpgradeAttack(currentBaseATK, currentCritRate, currentCritDamge);
                break;
            case StatsType.Crit:
                nextCritRate += 1f;
                currentCritRate = nextCritRate;
                nextCritDamge += 2f;
                currentCritDamge = nextCritDamge;
                NextCritRate.text = currentCritRate.ToString() + ("%");
                NextCritDamge.text = currentCritDamge.ToString() + ("%");
                player.GetComponent<AttackDamgePlayer>().UpgradeAttack(currentBaseATK, currentCritRate, currentCritDamge);
                break;
            case StatsType.Stamina:
                nextStaminaMax = (int)(currentStaminaMax * 1.2f);
                currentStaminaMax = nextStaminaMax;
                NextStamina.text = currentStaminaMax.ToString();
                player.GetComponent<Stamina>().UpgradeStamina(currentStaminaMax);
                break;
            default:
                break;
        }
    }

    public void UpgradeHP() => Upgrade(StatsType.HP);
    public void UpgradeDef() => Upgrade(StatsType.Def);
    public void UpgradeAttack() => Upgrade(StatsType.Attack);
    public void UpgradeCrit() => Upgrade(StatsType.Crit);
    public void UpgradeStamina() => Upgrade(StatsType.Stamina);

    void CurrentStats()
    {
        LevelText.text = ("Level ") + Level.ToString();
        PointText.text = ("Point: ") + Point.ToString();
        PointLostText.text = ("Need Point: ") + PointLostText.ToString();

        var takeDamge = player.GetComponent<PlayerTakeDamge>();
        var attack = player.GetComponent<AttackDamgePlayer>();
        var stamina = player.GetComponent<Stamina>();

        currentHP_Max = takeDamge.MaxHP;
        nextHP_Max = currentHP_Max;
        HP.text = currentHP_Max.ToString();
        NextHP.text = currentHP_Max.ToString();

        currentDef = takeDamge.Defense;
        nextDef = currentDef;
        Def.text = currentDef.ToString();
        NextDef.text = currentDef.ToString();

        currentBaseATK = attack.BaseATK;
        nextBaseATK = currentBaseATK;
        BaseATK.text = currentBaseATK.ToString();
        NextBaseATK.text = currentBaseATK.ToString();

        currentCritRate = attack.critRate;
        nextCritRate = currentCritRate;
        CritRate.text = currentCritRate.ToString() + ("%");
        NextCritRate.text = currentCritRate.ToString() + ("%");

        currentCritDamge = attack.critDamge;
        nextCritDamge = currentCritDamge;
        CritDamge.text = currentCritDamge.ToString() + ("%");
        NextCritDamge.text= currentCritDamge.ToString() + ("%");

        currentStaminaMax = stamina.StaminaMax;
        nextStaminaMax = currentStaminaMax;
        Stamina.text = currentStaminaMax.ToString();
        NextStamina.text = currentStaminaMax.ToString();
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
            //lay index cua scene hien tai
            int currentsceneindex = SceneManager.GetActiveScene().buildIndex;
            //load lai scene hien tai

            SceneManager.LoadScene(currentsceneindex);
            StatsCanva.SetActive(true);
        }
    }
}
