using UnityEngine;
using Epic.OnlineServices;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Connect; 
using Epic.OnlineServices.Achievements;
using PlayEveryWare.EpicOnlineServices; 
using System; 
using System.Collections.Generic;
using System.Collections;
public class EOSAchievementTester : MonoBehaviour
{
    private PlatformInterface eosPlatform;
    private ProductUserId localProductUserId;

    private const string TEST_ACHIEVEMENT_ID = "TEST_ACH";
    private bool isLoggedIn = false;
    private bool eosInitializationAttempted = false;
    void Start()
    {
        if (EOSManager.Instance == null)
        {
            Debug.LogError("EOSManager.Instance is null. Ensure EOSManager prefab/component is in the scene.");
            this.enabled = false;
            return;
        }
        eosPlatform = EOSManager.Instance.GetEOSPlatformInterface();

        if (eosPlatform == null)
        {
            Debug.LogWarning("EOS Platform is not yet available from EOSManager.");
            if (!eosInitializationAttempted)
            {
                StartCoroutine(WaitforEOSInitializationAndLogin());
            }
        }
    }

    private IEnumerator WaitforEOSInitializationAndLogin()
    {
        eosInitializationAttempted = true;
        Debug.Log("Waiting for EOS Platform to initialize");
        float timeoutSeconds = 30f;
        float startTime = Time.realtimeSinceStartup;
        while (EOSManager.Instance.GetEOSPlatformInterface() == null)
        {
            if (Time.realtimeSinceStartup - startTime > timeoutSeconds)
            {
                Debug.LogError("EOS Platform initialization timed out.");
                this.enabled = false;
                yield break;
            }
            yield return null;
        }
        eosPlatform = EOSManager.Instance.GetEOSPlatformInterface();
    }
}
