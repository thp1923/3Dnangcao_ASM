using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class PlayFabLoginViaWebAPI : MonoBehaviour
{
    [System.Serializable]
    public class LoginRequest
    {
        public string customId;
        public bool createAccount = true;
    }

    void Start()
    {
        StartCoroutine(LoginToServer("my_custom_id_123"));
    }

    IEnumerator LoginToServer(string customId)
    {
        var req = new LoginRequest { customId = customId };
        string json = JsonUtility.ToJson(req);

        UnityWebRequest www = new UnityWebRequest("http://localhost:5203/api/Auth/login", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Login Success: " + www.downloadHandler.text);
            // Bạn có thể parse JSON kết quả để lấy PlayFabId, SessionTicket...
        }
        else
        {
            Debug.LogError("Login Failed: " + www.error);
        }
    }
}
