using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabLogin : MonoBehaviour
{
    void Start()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };


        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }


    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Đăng nhập PlayFab thành công!" + result.PlayFabId);
    }


    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Đăng nhập thất bại: " + error.GenerateErrorReport());
    }
}
