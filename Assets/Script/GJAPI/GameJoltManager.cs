using GameJolt.API;
using GameJolt.API.Objects;
using GameJolt.UI;
using System.Collections.Generic;
using UnityEngine;

public enum TrophyType
{
    StartNewGame,
    TestingTrophy,
    SecretCatTrophy,
    SecretCodeTrophy,
    MiniBossDefeatedTrophy,
    Boss1DefeatedTrophy,
    Boss2DefeatedTrophy,
    Boss3DefeatedTrophy,
    CollectAllTrophy

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
            //Debug.Log("Launched from Game Jolt App — auto-signing in...");
            var user = new User(usernameArg, tokenArg);
            user.SignIn(signInSuccess =>
            {
                if (signInSuccess)
                {
                    GameJoltAPI.Instance.CurrentUser = user;
                    //Debug.Log("Signed in via Game Jolt Client as: " + user.Name);
                }
                else
                {
                    //Debug.LogWarning("Game Jolt app login failed.");
                }
            });
            return;
        }

        string savedUsername = PlayerPrefs.GetString(USERNAME_KEY, "");
        string savedToken = PlayerPrefs.GetString(TOKEN_KEY, "");

        if (!string.IsNullOrEmpty(savedUsername) && !string.IsNullOrEmpty(savedToken))
        {
            //Debug.Log("Using saved PlayerPrefs credentials...");
            var user = new User(savedUsername, savedToken);
            user.SignIn(success =>
            {
                if (success)
                {
                    GameJoltAPI.Instance.CurrentUser = user;
                    //Debug.Log("Auto-signed in with saved credentials: " + user.Name);
                }
                else
                {
                    //Debug.LogWarning("Saved credential login failed.");
                }
            });
        }
        else
        {
            //Debug.Log("No saved credentials found, user not signed in.");
        }
    }
    #region Trophies
    public void UnlockTrophy(TrophyType type)
    {
        if (!GameJoltAPI.Instance.HasSignedInUser)
        {
            //Debug.LogWarning("Cannot unlock, user not signed in.");
            return;
        }

        if (!trophyMap.TryGetValue(type, out var id))
        {
            //Debug.LogError($"No Trophy ID for TrophyType.{type}");
            return;
        }

        //Debug.Log($"Attempting to unlock trophy '{type}' with ID {id}");

        GameJolt.API.Trophies.TryUnlock(id, result =>
        {
            switch (result)
            {
                case TryUnlockResult.Unlocked:
                    //Debug.Log($"Unlocked trophy '{type}' (ID {id})!");
                    break;
                case TryUnlockResult.AlreadyUnlocked:
                    //Debug.Log($"Trophy '{type}' already unlocked.");
                    break;
                case TryUnlockResult:
                    //Debug.LogError($"Unlock failed: Unknown error from Game Jolt for trophy '{type}' (ID {id})");
                    break;
            }
        });
    }

    public void ShowTrophies()
    {
        if (GameJoltAPI.Instance.HasSignedInUser)
            GameJoltUI.Instance.ShowTrophies();
        else
        {
            //Debug.LogWarning("User not signed in — can't show trophies.");
        }
    }
    #endregion
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Debug.Log("K key pressed — testing trophy unlock...");
            UnlockTrophy(TrophyType.TestingTrophy);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Debug.Log("Login state: " + (GameJoltAPI.Instance.HasSignedInUser ?
                //$"Signed in as {GameJoltAPI.Instance.CurrentUser.Name}" : "Not signed in"));
        }

        // --- Added: Press I to open GameJolt login UI in Editor ---
        if (Input.GetKeyDown(KeyCode.F10))
        {
            //Debug.Log("Opening Game Jolt Login UI...");
            GameJoltUI.Instance.ShowSignIn();
            StartCoroutine(CheckAndStoreCredentials());
        }
    }

    private System.Collections.IEnumerator CheckAndStoreCredentials()
    {
        // Wait until the user is signed in
        while (!GameJoltAPI.Instance.HasSignedInUser)
        {
            yield return null;
        }
        // Save credentials for future auto-login
        PlayerPrefs.SetString(USERNAME_KEY, GameJoltAPI.Instance.CurrentUser.Name);
        PlayerPrefs.SetString(TOKEN_KEY, GameJoltAPI.Instance.CurrentUser.Token);
        PlayerPrefs.Save();
        //Debug.Log("Game Jolt credentials saved for Editor auto-login.");
    }
}
