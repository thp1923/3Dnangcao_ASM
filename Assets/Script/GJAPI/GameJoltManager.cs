using GameJolt.API;
using GameJolt.API.Objects;
using GameJolt.UI;
using System.Collections.Generic;
using UnityEngine;

public enum TrophyType
{
    StartNewGame,
    TestingTrophy

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
        string[] args = System.Environment.GetCommandLineArgs();
        string usernameArg = "";
        string tokenArg = "";

        foreach (var arg in args)
        {
            if (arg.StartsWith("-gjapi_username="))
                usernameArg = arg.Substring("-gjapi_username=".Length);

            if (arg.StartsWith("-gjapi_token="))
                tokenArg = arg.Substring("-gjapi_token=".Length);
        }

        if (!string.IsNullOrEmpty(usernameArg) && !string.IsNullOrEmpty(tokenArg))
        {
            Debug.Log("Launched from Game Jolt App — auto-signing in...");
            var user = new User(usernameArg, tokenArg);
            user.SignIn(signInSuccess =>
            {
                if (signInSuccess)
                {
                    GameJoltAPI.Instance.CurrentUser = user;
                    Debug.Log("Signed in via Game Jolt Client as: " + user.Name);
                }
                else
                {
                    Debug.LogWarning("Game Jolt app login failed.");
                }
            });
            return;
        }

        string savedUsername = PlayerPrefs.GetString(USERNAME_KEY, "");
        string savedToken = PlayerPrefs.GetString(TOKEN_KEY, "");

        if (!string.IsNullOrEmpty(savedUsername) && !string.IsNullOrEmpty(savedToken))
        {
            Debug.Log("Using saved PlayerPrefs credentials...");
            var user = new User(savedUsername, savedToken);
            user.SignIn(success =>
            {
                if (success)
                {
                    GameJoltAPI.Instance.CurrentUser = user;
                    Debug.Log("Auto-signed in with saved credentials: " + user.Name);
                }
                else
                {
                    Debug.LogWarning("Saved credential login failed.");
                }
            });
        }
        else
        {
            Debug.Log("No saved credentials found, user not signed in.");
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
