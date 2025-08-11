using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
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

    public enum StatsType { HP, Attack, Stamina, Def, Crit }

    [Header("---------Canva----------")]
    public GameObject StatsCanva;
    bool IsHide;

    [Header("---------Alive----------")]
    #region Alive
    int currentHP_Max, nextHP_Max;
    public TMPro.TextMeshProUGUI HP;
    public TMPro.TextMeshProUGUI NextHP;

    int currentDef, nextDef;
    public TMPro.TextMeshProUGUI Def;
    public TMPro.TextMeshProUGUI NextDef;
    #endregion

    [Header("---------Attack----------")]
    #region Attack
    int currentBaseATK, nextBaseATK;
    public TMPro.TextMeshProUGUI BaseATK;
    public TMPro.TextMeshProUGUI NextBaseATK;

    float currentCritRate, nextCritRate;
    public TMPro.TextMeshProUGUI CritRate;
    public TMPro.TextMeshProUGUI NextCritRate;

    float currentCritDamge, nextCritDamge;
    public TMPro.TextMeshProUGUI CritDamge;
    public TMPro.TextMeshProUGUI NextCritDamge;
    #endregion

    [Header("---------Stamina----------")]
    #region Stamina
    int currentStaminaMax, nextStaminaMax;
    public TMPro.TextMeshProUGUI Stamina;
    public TMPro.TextMeshProUGUI NextStamina;
    #endregion

    private void Awake()
    {
        if (FindObjectsOfType<UpgradeStats>().Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
        IsHide = true;
        LevelText.text = "Level " + Level;
        PointText.text = "Point: " + Point;
        PointLostText.text = "Need Point: " + Point_Lost;
        if(Level > 0)
        {
            Point_Lost = Mathf.FloorToInt(10 * Mathf.Pow(Level, 1.05f));
            PointLostText.text = "Need Point: " + Point_Lost;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            player.GetComponent<vThirdPersonInput>().enabled = !IsHide;
            player.GetComponent<vThirdPersonController>().enabled = !IsHide;
            player.GetComponent<PlayerAttackController>().enabled = !IsHide;
        }

        // Tự tìm lại UI nếu bị mất
        if (LevelText == null)
            LevelText = GameObject.Find("LevelText")?.GetComponent<TMPro.TextMeshProUGUI>();
        if (PointText == null)
            PointText = GameObject.Find("PointText")?.GetComponent<TMPro.TextMeshProUGUI>();
        if (PointLostText == null)
            PointLostText = GameObject.Find("PointLostText")?.GetComponent<TMPro.TextMeshProUGUI>();

        if (StatsCanva == null)
            StatsCanva = GameObject.Find("StatsCanva");

        // Cập nhật lại stats sau khi scene load
        CurrentStats();

        if (!IsHide && StatsCanva != null)
            StatsCanva.SetActive(true);
    }

    public void AddPoint(int addPoint)
    {
        Point += addPoint;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null) return;
        }

        if (player.GetComponent<PlayerTakeDamge>().isDeath) return;
        if (!PlayerAttackController.CursorLocked) return;

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
        if (Point < Point_Lost)
        {
            //Debug.Log("Not Enough Point");
            return;
        }

        Level++;
        Point -= Point_Lost;
        Point_Lost = Mathf.FloorToInt(10 * Mathf.Pow(Level, 1.05f));

        LevelText.text = "Level " + Level;
        PointText.text = "Point: " + Point;
        PointLostText.text = "Need Point: " + Point_Lost;

        switch (type)
        {
            case StatsType.HP:
                nextHP_Max = (int)(currentHP_Max * 1.1f);
                currentHP_Max = nextHP_Max;
                HP.text = currentHP_Max.ToString();
                NextHP.text = currentHP_Max.ToString();
                player.GetComponent<PlayerTakeDamge>().UpgradeAlive(currentHP_Max, currentDef);
                break;

            case StatsType.Def:
                nextDef = (int)(currentDef * 1.05f);
                currentDef = nextDef;
                Def.text = currentDef.ToString();
                NextDef.text = currentDef.ToString();
                player.GetComponent<PlayerTakeDamge>().UpgradeAlive(currentHP_Max, currentDef);
                break;

            case StatsType.Attack:
                nextBaseATK = (int)(currentBaseATK * 1.1f);
                currentBaseATK = nextBaseATK;
                BaseATK.text = currentBaseATK.ToString();
                NextBaseATK.text = currentBaseATK.ToString();
                player.GetComponent<AttackDamgePlayer>().UpgradeAttack(currentBaseATK, currentCritRate, currentCritDamge);
                break;

            case StatsType.Crit:
                nextCritRate += 1f;
                currentCritRate = nextCritRate;
                nextCritDamge += 2f;
                currentCritDamge = nextCritDamge;
                CritRate.text = currentCritRate + "%";
                NextCritRate.text = currentCritRate + "%";
                CritDamge.text = currentCritDamge + "%";
                NextCritDamge.text = currentCritDamge + "%";
                player.GetComponent<AttackDamgePlayer>().UpgradeAttack(currentBaseATK, currentCritRate, currentCritDamge);
                break;

            case StatsType.Stamina:
                nextStaminaMax = (int)(currentStaminaMax * 1.2f);
                currentStaminaMax = nextStaminaMax;
                Stamina.text = currentStaminaMax.ToString();
                NextStamina.text = currentStaminaMax.ToString();
                player.GetComponent<Stamina>().UpgradeStamina(currentStaminaMax);
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
        if (player == null) return;

        LevelText.text = "Level " + Level;
        PointText.text = "Point: " + Point;
        PointLostText.text = "Need Point: " + Point_Lost;

        var takeDamage = player.GetComponent<PlayerTakeDamge>();
        var attack = player.GetComponent<AttackDamgePlayer>();
        var stamina = player.GetComponent<Stamina>();

        currentHP_Max = takeDamage.MaxHP;
        nextHP_Max = currentHP_Max;
        HP.text = currentHP_Max.ToString();
        NextHP.text = currentHP_Max.ToString();

        currentDef = takeDamage.Defense;
        nextDef = currentDef;
        Def.text = currentDef.ToString();
        NextDef.text = currentDef.ToString();

        currentBaseATK = attack.BaseATK;
        nextBaseATK = currentBaseATK;
        BaseATK.text = currentBaseATK.ToString();
        NextBaseATK.text = currentBaseATK.ToString();

        currentCritRate = attack.critRate;
        nextCritRate = currentCritRate;
        CritRate.text = currentCritRate + "%";
        NextCritRate.text = currentCritRate + "%";

        currentCritDamge = attack.critDamge;
        nextCritDamge = currentCritDamge;
        CritDamge.text = currentCritDamge + "%";
        NextCritDamge.text = currentCritDamge + "%";

        currentStaminaMax = stamina.StaminaMax;
        nextStaminaMax = currentStaminaMax;
        Stamina.text = currentStaminaMax.ToString();
        NextStamina.text = currentStaminaMax.ToString();
    }

    public void CanvaStats(bool isHide)
    {
        IsHide = isHide;

        if (isHide)
        {
            var attackCtrl = FindObjectOfType<PlayerAttackController>();
            attackCtrl.LockController(true);
            StatsCanva?.SetActive(false);
        }
        else
        {
            CurrentStats();
            var lockCtrl = FindObjectOfType<LockController>();
            lockCtrl.OutPlayerController();
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index);
        }
    }
}
