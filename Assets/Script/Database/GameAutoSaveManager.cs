using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class GameAutoSaveManager : MonoBehaviour
{
    public static GameAutoSaveManager Instance;

    public string saveSlot;
    public int level = 1;
    public int hp = 100;
    public int souls = 0;
    public int playTime = 0;

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

    public void Init(string slot, int startLevel = 1, int startHP = 100, int startSouls = 0)
    {
        saveSlot = slot;
        level = startLevel;
        hp = startHP;
        souls = startSouls;
        playTime = 0;

        SaveCurrentGame();
    }

    public void SaveCurrentGame()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        GameObject player = GameObject.FindWithTag("Player");
        Vector3 pos = player.transform.position;

        GameStateData data = new GameStateData
        {
            Level = level,
            HP = hp,
            Souls = souls,
            LastScene = sceneName,
            PlayTime = playTime,
            posX = pos.x,
            posY = pos.y,
            posZ = pos.z
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
