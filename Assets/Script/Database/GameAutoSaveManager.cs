using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using StatsManager;
using System.ComponentModel.Design.Serialization;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using Unity.VisualScripting;
public class GameAutoSaveManager : MonoBehaviour
{
    public static GameAutoSaveManager Instance;

    public string saveSlot;
    public int level = 1;
    public int hp = 0;
    public int souls = 0;
    public int playTime = 0;
    public int MaxHP = 0;
    public int Defense = 0;
    public int StunResistance = 0;
    public int currentHP = 0;
    public int BaseATK = 0;
    public float CritRate = 0;
    public float CritDamge = 0;
    public int StaminaMax = 0;
    public int Point = 0;
    public int HeathCount = 0;
    public int AtkBonus = 0;
    public float DamgeAttack = 0;
    public int AtkBonusSkill = 0;
    public float CritRateBonus = 0;
    public float CritDamgeBonus = 0;
    public bool CanSkill;
    public int SpecialSkillId = 0;
    public bool CanBuff;
    public int BuffTypeId = 0;

    public Vector3 nextPlayerPosition = Vector3.zero;

    float autoSaveInterval = 60f;
    float timer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        timer += Time.deltaTime;
        playTime += (int)Time.deltaTime;

        if (timer >= autoSaveInterval)
        {
            SaveCurrentGame();
            timer = 0f;
        }
    }

    public void Init(string slot, int startLevel = 1, int startSouls = 0)
    {
        saveSlot = slot;
        level = startLevel;
        souls = startSouls;
        playTime = 0;


        SaveCurrentGame();
    }

    public void SaveCurrentGame()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        GameObject player = GameObject.FindWithTag("Player");

        // ✅ Lấy StatsAlive từ Player
        StatsAlive statsAlive = player.GetComponent<StatsAlive>();
        if (statsAlive != null)
        {
            hp = statsAlive.HpSlider.value > 0 ? Mathf.RoundToInt(statsAlive.HpSlider.value) : 0;
            currentHP = hp;
            MaxHP = statsAlive.MaxHP;
            Defense = statsAlive.Defense;
            StunResistance = statsAlive.StunResistance;
        }

        Vector3 pos = player.transform.position;

        GameStateData data = new GameStateData
        {
            Level = level,
            MaxHP = statsAlive.MaxHP,
            Defense = statsAlive.Defense,
            StunResistance = statsAlive.StunResistance,
            Souls = souls,
            LastScene = sceneName,
            PlayTime = playTime,
            posX = pos.x,
            posY = pos.y,
            posZ = pos.z,
        };

        string json = JsonUtility.ToJson(data);

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new System.Collections.Generic.Dictionary<string, string>
        {
            { saveSlot, json }
        }
        },
        result => Debug.Log($"✅ AutoSaved: {sceneName} at {pos}"),
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void OnBonfireRest()
    {
        SaveCurrentGame();
    }

    public void OnPlayerDie()
    {
        SaveCurrentGame();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (nextPlayerPosition != Vector3.zero)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = nextPlayerPosition;
            nextPlayerPosition = Vector3.zero;
        }
    }
}
