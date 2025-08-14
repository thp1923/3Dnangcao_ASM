using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using StatsManager;
using System.Collections.Generic;

public class GameAutoSaveManager : MonoBehaviour
{
    public static GameAutoSaveManager Instance;

    [Header("Save Slot")]
    public string saveSlot;

    [Header("Player State (cached)")]
    public int level = 1;
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

    [Header("Teleport on load")]
    public Vector3 nextPlayerPosition = Vector3.zero;

    // playtime counter
    float timer = 0f;
    float playTimeAccum = 0f;

    // ===== Cách 1: RAM snapshot =====
    private bool _isSceneChange = false;
    private GameStateData _lastSavedSnapshot = null;   // lưu stat snapshot mới nhất trong RAM

    // pending khi load từ server (die)
    private GameStateData _pendingLoadData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        // chỉ đếm playtime
        timer += Time.deltaTime;
        playTimeAccum += Time.deltaTime;
        if (playTimeAccum >= 1f)
        {
            int add = Mathf.FloorToInt(playTimeAccum);
            playTime += add;
            playTimeAccum -= add;
        }
    }

    public void Init(string slot, int startLevel = 1, int startSouls = 0)
    {
        saveSlot = slot;
        level = startLevel;
        playTime = 0;
        SaveCurrentGame(); // snapshot + push PlayFab
    }

    /// <summary>Gọi ngay trước khi chuyển scene bình thường (cổng/door/trigger)</summary>
    public void PrepareSceneChange()
    {
        _isSceneChange = true;
        timer = 0f; // tránh autosave chen ngang ngay khi vào scene mới
    }

    /// <summary>Lưu stat hiện tại (và snapshot RAM). Callback gọi sau khi PlayFab trả về (thành/bại).</summary>
    public void SaveCurrentGame(System.Action onDone = null)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            //Debug.LogWarning("Skip save: Player not found in this scene.");
            onDone?.Invoke();
            return;
        }

        var statsAlive       = player.GetComponent<StatsAlive>();
        var attackDamgePlayer= FindObjectOfType<AttackDamgePlayer>();
        var stamina          = FindObjectOfType<Stamina>();
        var upgradeStats     = FindObjectOfType<UpgradeStats>();
        var specialSkill     = FindObjectOfType<SpecialSkill>();
        var playerBuff       = FindObjectOfType<PlayerBuff>();

        if (statsAlive == null || attackDamgePlayer == null || stamina == null || upgradeStats == null)
        {
            //Debug.LogWarning("Skip save: some required components are missing.");
            onDone?.Invoke();
            return;
        }

        if (statsAlive != null)
        {
            currentHP = (statsAlive.HpSlider != null)
                ? Mathf.RoundToInt(statsAlive.HpSlider.value)
                : statsAlive.currentHP;

            MaxHP          = statsAlive.MaxHP;
            Defense        = statsAlive.Defense;
            StunResistance = statsAlive.StunResistance;
        }

        Vector3 pos = player.transform.position;

        GameStateData data = new GameStateData
        {
            Level           = upgradeStats.Level,
            Point           = upgradeStats.Point,
            MaxHP           = statsAlive.MaxHP,
            Defense         = statsAlive.Defense,
            StunResistance  = statsAlive.StunResistance,
            currentHP       = statsAlive.currentHP,

            BaseATK         = attackDamgePlayer.BaseATK,
            critRate        = attackDamgePlayer.critRate,
            critDamge       = attackDamgePlayer.critDamge,
            atkBonus        = attackDamgePlayer.atkBonus,
            damgeAttack     = attackDamgePlayer.damgeAttack,
            critRateBonus   = attackDamgePlayer.critRateBonus,
            critDamgeBonus  = attackDamgePlayer.critDamgeBonus,

            StaminaMax      = stamina.StaminaMax,

            canSkill        = (specialSkill != null && specialSkill.canSkill),
            SpecialSkillId  = specialSkill != null ? specialSkill.SpecialSkillId : 0,
            canBuff         = (playerBuff != null && playerBuff.canBuff),
            buffTypeId      = playerBuff != null ? playerBuff.buffTypeId : 0,

            LastScene       = sceneName,
            PlayTime        = playTime,
            posX            = pos.x,
            posY            = pos.y,
            posZ            = pos.z,
        };

        // --- Snapshot RAM ---
        _lastSavedSnapshot = data;

        // --- Push PlayFab (async) ---
        string json = JsonUtility.ToJson(data);
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> { { saveSlot, json } }
            },
            result =>
            {
                //Debug.Log($"✅ AutoSaved: {sceneName} at {pos}");
                onDone?.Invoke();
            },
            error =>
            {
                //Debug.LogError(error.GenerateErrorReport());
                onDone?.Invoke();
            }
        );
    }

    /// <summary>Gọi khi nghỉ bonfire (tuỳ chọn: lưu luôn inventory).</summary>
    public void OnBonfireRest()
    {
        // lưu inventory theo slot hiện tại (nếu có InventoryManager)
        InventoryManager.Instance?.SaveInventoryForSlot(saveSlot);

        // lưu core state
        SaveCurrentGame();
    }

    /// <summary>Khi chết → quay về checkpoint save ở bonfire (Load từ PlayFab)</summary>
    public void OnPlayerDie()
    {
        LoadGame(saveSlot);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Teleport nếu có
        if (nextPlayerPosition != Vector3.zero)
        {
            var playerSet = GameObject.FindWithTag("Player");
            if (playerSet != null) playerSet.transform.position = nextPlayerPosition;
            nextPlayerPosition = Vector3.zero;
        }

        // Nếu vừa đổi scene bình thường (trigger) → áp snapshot RAM
        if (_isSceneChange && _lastSavedSnapshot != null)
        {
            ApplyLoadedState(_lastSavedSnapshot);
            _isSceneChange = false;

            // Dựng lại túi + trang bị (UI invent thường thuộc scene)
            InventoryManager.Instance?.LoadInventoryForSlot(saveSlot);
            return;
        }

        // Nếu là nhánh respawn vì die (đã LoadGame) → áp pending và dựng invent
        if (_pendingLoadData != null)
        {
            ApplyLoadedState(_pendingLoadData);
            _pendingLoadData = null;

            enabled = true; // bật lại nếu trước đó có tắt
            InventoryManager.Instance?.LoadInventoryForSlot(saveSlot);
        }
    }

    public void LoadGame(string slot)
    {
        saveSlot = slot;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey(saveSlot))
            {
                string json = result.Data[saveSlot].Value;
                GameStateData data = JsonUtility.FromJson<GameStateData>(json);

                string currentScene = SceneManager.GetActiveScene().name;
                if (!string.IsNullOrEmpty(data.LastScene) && data.LastScene != currentScene)
                {
                    // chuyển scene để respawn
                    nextPlayerPosition = new Vector3(data.posX, data.posY, data.posZ);

                    enabled = false; // tránh autosave chen ngang
                    SceneManager.sceneLoaded += OnSceneLoadedAfterLoad;
                    SceneManager.LoadScene(data.LastScene);

                    // chờ scene loaded rồi apply
                    _pendingLoadData = data;
                }
                else
                {
                    ApplyLoadedState(data);
                    // dựng lại inventory ngay trong scene hiện tại
                    InventoryManager.Instance?.LoadInventoryForSlot(saveSlot);
                }
            }
            else
            {
                //Debug.LogWarning($"❗ Chưa có dữ liệu save cho slot: {saveSlot}");
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    private void OnSceneLoadedAfterLoad(Scene scene, LoadSceneMode mode)
    {
        if (nextPlayerPosition != Vector3.zero)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null) player.transform.position = nextPlayerPosition;
            nextPlayerPosition = Vector3.zero;
        }

        if (_pendingLoadData != null)
        {
            ApplyLoadedState(_pendingLoadData);
            _pendingLoadData = null;
        }

        enabled = true;
        SceneManager.sceneLoaded -= OnSceneLoadedAfterLoad;

        // dựng lại túi + trang bị khi respawn
        InventoryManager.Instance?.LoadInventoryForSlot(saveSlot);
    }

    private void ApplyLoadedState(GameStateData data)
    {
        // cập nhật biến quản lý
        level    = Mathf.Max(1, data.Level);
        playTime = Mathf.Max(0, data.PlayTime);

        var player = GameObject.FindWithTag("Player");
        //if (player == null)
        //{
        //     Debug.LogError("Player không tìm thấy để apply state.");
        //    return;
        //}

        // Lấy components
        var statsAlive = player.GetComponent<StatsAlive>();
        var atk        = FindObjectOfType<AttackDamgePlayer>();
        var stamina    = FindObjectOfType<Stamina>();
        var upgrade    = FindObjectOfType<UpgradeStats>();
        var special    = FindObjectOfType<SpecialSkill>();
        var playerBuff = FindObjectOfType<PlayerBuff>();

        // Upgrade/Level/Point
        if (upgrade != null)
        {
            upgrade.Level = Mathf.Max(1, data.Level);
            upgrade.Point = Mathf.Max(0, data.Point);
        }

        // StatsAlive
        if (statsAlive != null)
        {
            statsAlive.MaxHP         = Mathf.Max(1, data.MaxHP);
            statsAlive.Defense       = Mathf.Max(0, data.Defense);
            statsAlive.StunResistance= Mathf.Max(0, data.StunResistance);

            statsAlive.currentHP = Mathf.Clamp(data.currentHP, 0, statsAlive.MaxHP);

            if (statsAlive.HpSlider != null)
            {
                statsAlive.HpSlider.maxValue = statsAlive.MaxHP;
                statsAlive.HpSlider.value    = statsAlive.currentHP;
            }
        }

        // Attack/Crit
        if (atk != null)
        {
            atk.BaseATK        = Mathf.Max(0,  data.BaseATK);
            atk.critRate       = Mathf.Max(0f, data.critRate);
            atk.critDamge      = Mathf.Max(0f, data.critDamge);
            atk.atkBonus       = Mathf.Max(0,  data.atkBonus);
            atk.damgeAttack    = Mathf.Max(0f, data.damgeAttack);
            atk.critRateBonus  = Mathf.Max(0f, data.critRateBonus);
            atk.critDamgeBonus = Mathf.Max(0f, data.critDamgeBonus);
        }

        // Stamina
        if (stamina != null)
        {
            stamina.StaminaMax = Mathf.Max(0, data.StaminaMax);
            // nếu có current stamina: clamp tại đây
            // stamina.Current = Mathf.Clamp(stamina.Current, 0, stamina.StaminaMax);
        }

        // Skill & Buff
        if (special != null)
        {
            special.canSkill      = data.canSkill;
            special.SpecialSkillId= data.SpecialSkillId;
        }
        if (playerBuff != null)
        {
            playerBuff.canBuff   = data.canBuff;
            playerBuff.buffTypeId= data.buffTypeId;
        }

        // Đặt vị trí nếu cùng scene
        string currentScene = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(data.LastScene) && data.LastScene == currentScene)
        {
            player.transform.position = new Vector3(data.posX, data.posY, data.posZ);
        }

        //Debug.Log("✅ Loaded game state và áp dụng thành công (RAM snapshot / server).");
    }
}
