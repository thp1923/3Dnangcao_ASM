using GameJolt.API;
using GameJolt.API.Objects;
using GameJolt.UI;
using UnityEngine;
public class GameJoltManager : MonoBehaviour
{
    private const string USERNAME_KEY = "GJ_Username";
    private const string TOKEN_KEY = "GJ_Token";

    // Start is called before the first frame update
    [System.Serializable]
    public struct Trophy
    {
        public int TrophyID;
    }
    void Start()
    {
        string savedUsername = PlayerPrefs.GetString(USERNAME_KEY, "");
        string savedToken = PlayerPrefs.GetString(TOKEN_KEY, "");

        if (!string.IsNullOrEmpty(savedUsername) && !string.IsNullOrEmpty(savedToken))
        {
            // Try auto sign in with saved credentials
            var user = new User(savedUsername, savedToken);
            user.SignIn((bool signInSuccess) => {
                if (signInSuccess)
                {
                    GameJoltAPI.Instance.CurrentUser = user;
                    Debug.Log("Auto-signed in as: " + user.Name);
                }
                else
                {
                    Debug.Log("Auto sign-in failed. Showing sign-in UI.");
                    ShowSignInUI();
                }
            });
        }
        else
        {
            // No saved credentials, show sign-in
            ShowSignInUI();
        }
    }

    public void ShowSignInUI()
    {
        GameJoltUI.Instance.ShowSignIn(
            (bool signInSuccess) =>
            {
                if (signInSuccess)
                {
                    string username = GameJoltAPI.Instance.CurrentUser.Name;
                    string token = GameJoltAPI.Instance.CurrentUser.Token;
                    Debug.Log("Signed in as: " + username);

                    // Save credentials for next time
                    PlayerPrefs.SetString(USERNAME_KEY, username);
                    PlayerPrefs.SetString(TOKEN_KEY, token);
                    PlayerPrefs.Save();
                }
                else
                {
                    Debug.Log("Sign-in failed or cancelled.");
                    // Optionally, retry or prompt again
                }
            }
        );
    }
    #region Trophies
    public void UnlockTrophyByID(int trophyID)
    {

    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
