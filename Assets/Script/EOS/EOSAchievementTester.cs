using UnityEngine;
using Epic.OnlineServices;
using Epic.OnlineServices.Platform;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Achievements;
using PlayEveryWare.EpicOnlineServices;
public class EOSAchievementTester : MonoBehaviour
{
    [Header("EOS Settings")]
    public string achievementIdToUnlock = "TEST_ACH"; 

    [Header("Dev Auth Tool Credentials (for Login)")]
    public string devAuthHost = "localhost:1923"; 
    public string devAuthCredentialName = "thp1923"; 

    private ProductUserId _localUserId = null;
    private bool _isLoggedIn = false;
    private bool _achievementsQueried = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
