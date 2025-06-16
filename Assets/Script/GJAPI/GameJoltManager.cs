using GameJolt.API;
using GameJolt.API.Objects;
using GameJolt.UI;
using System.Collections.Generic;
using UnityEngine;

public enum TrophyType
{
    StartNewGame,
    TestingTrophy
    // Extend this list
}

[System.Serializable]
public struct TrophyEntry
{
    public TrophyType type;
    public int id;
}
public class GameJoltManager : MonoBehaviour
{
    [Header("Map TrophyType to GameJolt Trophy ID")]
    [SerializeField] private TrophyEntry[] trophyEntries;

    private Dictionary<TrophyType, int> trophyMap;

    private const string USERNAME_KEY = "GJ_Username";
    private const string TOKEN_KEY = "GJ_Token";

    private void Awake()
    {
        trophyMap = new Dictionary<TrophyType, int>();
        foreach (var entry in trophyEntries)
        {
            trophyMap[entry.type] = entry.id;
        }
    }

    private void Start()
    {
        TryAutoLogin();
    }

    private void TryAutoLogin()
    {
        string savedUsername = PlayerPrefs.GetString(USERNAME_KEY, "");
        string savedToken = PlayerPrefs.GetString(TOKEN_KEY, "");

        if (!string.IsNullOrEmpty(savedUsername) && !string.IsNullOrEmpty(savedToken))
        {
            var user = new User(savedUsername, savedToken);
            user.SignIn(signInSuccess =>
            {
                if (signInSuccess)
                {
                    GameJoltAPI.Instance.CurrentUser = user;
                    Debug.Log("Auto-signed in to Game Jolt as: " + user.Name);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("Auto login failed. Showing Game Jolt login UI (Editor only).");
                    GameJolt.UI.GameJoltUI.Instance.ShowSignIn(result =>
                    {
                        if (result)
                        {
                            var current = GameJoltAPI.Instance.CurrentUser;
                            PlayerPrefs.SetString(USERNAME_KEY, current.Name);
                            PlayerPrefs.SetString(TOKEN_KEY, current.Token);
                            PlayerPrefs.Save();
                            Debug.Log("Manually signed in as: " + current.Name);
                        }
                        else
                        {
                            Debug.LogWarning("Manual login canceled or failed.");
                        }
                    });
#else
                Debug.Log("Auto login failed. Skipping Game Jolt login in build.");
#endif
                }
            });
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("No saved credentials. Showing login UI (Editor only).");
            GameJolt.UI.GameJoltUI.Instance.ShowSignIn(result =>
            {
                if (result)
                {
                    var current = GameJoltAPI.Instance.CurrentUser;
                    PlayerPrefs.SetString(USERNAME_KEY, current.Name);
                    PlayerPrefs.SetString(TOKEN_KEY, current.Token);
                    PlayerPrefs.Save();
                    Debug.Log("Manually signed in as: " + current.Name);
                }
                else
                {
                    Debug.LogWarning("Manual login canceled or failed.");
                }
            });
#else
        Debug.Log("No credentials. Skipping Game Jolt login in build.");
#endif
        }
    }
    #region Trophies
    public void UnlockTrophy(TrophyType type)
    {
        if (!GameJoltAPI.Instance.HasSignedInUser)
        {
            Debug.LogWarning("Cannot unlock, user not signed in.");
            return;
        }
        if (!trophyMap.TryGetValue(type, out var id))
        {
            Debug.LogError($"No Trophy ID for TrophyType.{type}");
            return;
        }
        // Use TryUnlock to prevent duplicate UI notifications
        GameJolt.API.Trophies.TryUnlock(id, result => {
            switch (result)
            {
                case TryUnlockResult.Unlocked:
                    Debug.Log($"Unlocked {type} (ID {id})!");
                    break;
                case TryUnlockResult.AlreadyUnlocked:
                    Debug.Log($"{type} already unlocked.");
                    break;
                default:
                    Debug.LogError($"Failed to unlock {type}.");
                    break;
            }
        });
    }

    public void ShowTrophies()
    {
        if (GameJoltAPI.Instance.HasSignedInUser)
            GameJoltUI.Instance.ShowTrophies();
        else
            Debug.LogWarning("User not signed in — can't show trophies.");
    }
    #endregion
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("K key pressed — testing trophy unlock...");
            UnlockTrophy(TrophyType.TestingTrophy);
        }
    }
}
