using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using StatsManager;
public class GameAutoSaveManager : MonoBehaviour
{
    public static GameAutoSaveManager Instance;

    public string saveSlot;
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

    public Vector3 nextPlayerPosition = Vector3.zero;

    float autoSaveInterval = 60f;
    float timer = 0f;
    float playTimeAccum = 0f;

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

        playTimeAccum += Time.deltaTime;
        if (playTimeAccum >= 1f)
        {
            int add = Mathf.FloorToInt(playTimeAccum);
            playTime += add;
            playTimeAccum -= add;
        }

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
        playTime = 0;


        SaveCurrentGame();
    }

    public void SaveCurrentGame()
    {
        var sceneName = SceneManager.GetActiveScene().name;
    var player = GameObject.FindWithTag("Player");
    if (player == null)
    {
        Debug.LogWarning("Skip save: Player not found in this scene.");
        return;
    }

    var statsAlive = player.GetComponent<StatsAlive>();
    var attackDamgePlayer = FindObjectOfType<AttackDamgePlayer>();
    var stamina = FindObjectOfType<Stamina>();
    var upgradeStats = FindObjectOfType<UpgradeStats>();
    var specialSkill = FindObjectOfType<SpecialSkill>();
    var playerBuff = FindObjectOfType<PlayerBuff>();

    if (statsAlive == null || attackDamgePlayer == null || stamina == null || upgradeStats == null)
    {
        Debug.LogWarning("Skip save: some required components are missing.");
        return;
    }
        if (statsAlive != null)
        {
            currentHP = statsAlive.HpSlider ? Mathf.RoundToInt(statsAlive.HpSlider.value) : statsAlive.currentHP;
            MaxHP = statsAlive.MaxHP;
            Defense = statsAlive.Defense;
            StunResistance = statsAlive.StunResistance;
        }
        Vector3 pos = player.transform.position;

        GameStateData data = new GameStateData
        {
            Level = upgradeStats.Level,
            Point = upgradeStats.Point,
            MaxHP = statsAlive.MaxHP,
            Defense = statsAlive.Defense,
            StunResistance = statsAlive.StunResistance,
            currentHP = statsAlive.currentHP,
            BaseATK = attackDamgePlayer.BaseATK,
            critRate = attackDamgePlayer.critRate,
            critDamge = attackDamgePlayer.critDamge,
            atkBonus = attackDamgePlayer.atkBonus,
            damgeAttack = attackDamgePlayer.damgeAttack,
            critRateBonus = attackDamgePlayer.critRateBonus,
            critDamgeBonus = attackDamgePlayer.critDamgeBonus,
            StaminaMax = stamina.StaminaMax,
            canSkill = specialSkill.canSkill,
            SpecialSkillId = specialSkill.SpecialSkillId,
            canBuff = playerBuff.canBuff,
            buffTypeId = playerBuff.buffTypeId,
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
    public void LoadGame(string slot)
    {
        saveSlot = slot;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey(saveSlot))
            {
                string json = result.Data[saveSlot].Value;
                GameStateData data = JsonUtility.FromJson<GameStateData>(json);

                // Nếu đang ở scene khác với LastScene thì load trước rồi apply sau khi scene loaded
                string currentScene = SceneManager.GetActiveScene().name;
                if (!string.IsNullOrEmpty(data.LastScene) && data.LastScene != currentScene)
                {
                    // Tạm thời lưu vị trí để OnSceneLoaded đặt lại
                    nextPlayerPosition = new Vector3(data.posX, data.posY, data.posZ);

                    // Tạm tắt auto-save trong lúc load để tránh ghi đè
                    enabled = false;

                    SceneManager.sceneLoaded += OnSceneLoadedAfterLoad;
                    SceneManager.LoadScene(data.LastScene);

                    // Lưu kèm data để apply stat sau khi scene load xong
                    _pendingLoadData = data;
                }
                else
                {
                    ApplyLoadedState(data);
                }
            }
            else
            {
                Debug.LogWarning($"❗ Chưa có dữ liệu save cho slot: {saveSlot}");
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }
    // Biến tạm giữ state khi chờ scene load
    private GameStateData _pendingLoadData;

    private void OnSceneLoadedAfterLoad(Scene scene, LoadSceneMode mode)
    {
        // Đặt lại vị trí nếu có
        if (nextPlayerPosition != Vector3.zero)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = nextPlayerPosition;
            }
            nextPlayerPosition = Vector3.zero;
        }

        if (_pendingLoadData != null)
        {
            ApplyLoadedState(_pendingLoadData);
            _pendingLoadData = null;
        }

        // Bật lại autosave sau khi apply xong
        enabled = true;

        // Hủy đăng ký để tránh lặp
        SceneManager.sceneLoaded -= OnSceneLoadedAfterLoad;
    }

    private void ApplyLoadedState(GameStateData data)
    {
        // Cập nhật biến quản lý
        level = Mathf.Max(1, data.Level);
        playTime = Mathf.Max(0, data.PlayTime);

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player không tìm thấy để apply state.");
            return;
        }

        // Lấy components
        StatsAlive statsAlive = player.GetComponent<StatsAlive>();
        AttackDamgePlayer atk = FindObjectOfType<AttackDamgePlayer>();
        Stamina stamina = FindObjectOfType<Stamina>();
        UpgradeStats upgrade = FindObjectOfType<UpgradeStats>();
        SpecialSkill special = FindObjectOfType<SpecialSkill>();
        PlayerBuff playerBuff = FindObjectOfType<PlayerBuff>();

        // Upgrade/Level/Point
        if (upgrade != null)
        {
            upgrade.Level = Mathf.Max(1, data.Level);
            upgrade.Point = Mathf.Max(0, data.Point);
        }

        // StatsAlive
        if (statsAlive != null)
        {
            statsAlive.MaxHP = Mathf.Max(1, data.MaxHP);
            statsAlive.Defense = Mathf.Max(0, data.Defense);
            statsAlive.StunResistance = Mathf.Max(0, data.StunResistance);

            // Clamp currentHP vào [0, MaxHP]
            statsAlive.currentHP = Mathf.Clamp(data.currentHP, 0, statsAlive.MaxHP);

            // Nếu bạn dùng Slider cho HP
            if (statsAlive.HpSlider != null)
            {
                statsAlive.HpSlider.maxValue = statsAlive.MaxHP;
                statsAlive.HpSlider.value = statsAlive.currentHP;
            }
        }

        // Attack/crit
        if (atk != null)
        {
            atk.BaseATK = Mathf.Max(0, data.BaseATK);
            atk.critRate = Mathf.Max(0f, data.critRate);
            atk.critDamge = Mathf.Max(0f, data.critDamge);
            atk.atkBonus = Mathf.Max(0, data.atkBonus);
            atk.damgeAttack = Mathf.Max(0f, data.damgeAttack);
            atk.critRateBonus = Mathf.Max(0f, data.critRateBonus);
            atk.critDamgeBonus = Mathf.Max(0f, data.critDamgeBonus);
        }

        // Stamina
        if (stamina != null)
        {
            stamina.StaminaMax = Mathf.Max(0, data.StaminaMax);
            // Nếu có current stamina, set thêm tại đây
            // stamina.Current = Mathf.Clamp(stamina.Current, 0, stamina.StaminaMax);
            // nếu bạn có event/UI update
        }

        // Skill & Buff
        if (special != null)
        {
            special.canSkill = data.canSkill;
            special.SpecialSkillId = data.SpecialSkillId;
         // nếu có logic set-up theo skill id
        }
        if (playerBuff != null)
        {
            playerBuff.canBuff = data.canBuff;
            playerBuff.buffTypeId = data.buffTypeId;
           // gọi ApplyOnLoad 
        }

        // Đặt vị trí nếu cùng scene
        string currentScene = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(data.LastScene) && data.LastScene == currentScene)
        {
            player.transform.position = new Vector3(data.posX, data.posY, data.posZ);
        }

        Debug.Log("✅ Loaded game state và áp dụng thành công.");
    }

}
