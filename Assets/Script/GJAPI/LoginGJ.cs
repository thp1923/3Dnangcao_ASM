using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameJolt.API;
using GameJolt.API.Objects;
using System;
public class LoginGJ : MonoBehaviour
{
    public GameObject Signin;
    public GameObject Signedin;

    public Button login;
    public Button logout;

    public TMP_InputField username;
    public TMP_InputField token;

    Action<bool> callback;
    public TextMeshProUGUI usernameText;
    // Start is called before the first frame update
    private void Start()
    {
        Signin.SetActive(true);
        Signedin.SetActive(false);

        logout.onClick.AddListener(() => LogOut());
        login.onClick.AddListener(() => LogIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameJoltAPI.Instance.CurrentUser == null)
        {
            Signin.SetActive(true);
            Signedin.SetActive(false);
        }
        else
        {
            Signin.SetActive(false);
            Signedin.SetActive(true);
        }
    }
    void LogIn()
    {
        if (username.text.Trim() == string.Empty || token.text.Trim() == string.Empty)
        {
        }
        else
        {

            var user = new GameJolt.API.Objects.User(username.text.Trim(), token.text.Trim());
            user.SignIn(signInSuccess =>
            {
                if (signInSuccess)
                {
                    Dismiss(true);
                }
            });
        }
    }
    void LogOut()
    {
        GameJoltAPI.Instance.CurrentUser.SignOut();
    }
    public void Dismiss(bool success)
    {
        if (callback != null)
        {
            callback(success);
            callback = null;
        }
    }
}
