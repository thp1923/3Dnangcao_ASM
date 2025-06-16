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
        // Build mapping
        trophyMap = new Dictionary<TrophyType, int>();
        foreach (var e in trophyEntries)
        {
            trophyMap[e.type] = e.id;
        }
    }

    private void Start()
    {
        // Auto sign-in
        string u = PlayerPrefs.GetString(USERNAME_KEY, "");
        string t = PlayerPrefs.GetString(TOKEN_KEY, "");
        if (!string.IsNullOrEmpty(u) && !string.IsNullOrEmpty(t))
        {
            var user = new User(u, t);
            user.SignIn(success => {
                if (success)
                {
                    GameJoltAPI.Instance.CurrentUser = user;
                    Debug.Log($"Signed in: {user.Name}");
                }
                else ShowSignInUI();
            });
        }
        else ShowSignInUI();
    }

    public void ShowSignInUI()
    {
        GameJoltUI.Instance.ShowSignIn(
            signInSuccess => {
                if (signInSuccess)
                {
                    var cur = GameJoltAPI.Instance.CurrentUser;
                    PlayerPrefs.SetString(USERNAME_KEY, cur.Name);
                    PlayerPrefs.SetString(TOKEN_KEY, cur.Token);
                    PlayerPrefs.Save();
                    Debug.Log($"Signed in: {cur.Name}");
                }
                else Debug.LogWarning("Sign-in failed or canceled");
            }
        );
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
            Debug.LogError($"No ID mapped for TrophyType.{type}");
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
