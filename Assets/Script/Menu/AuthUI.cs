using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;

public class AuthUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    public TMP_InputField inputLoginUsername;
    public TMP_InputField inputLoginPassword;

    public TMP_InputField inputRegisterUsername;
    public TMP_InputField inputRegisterPassword;

    public TextMeshProUGUI textStatus_login;
    public TextMeshProUGUI textStatus_register;

    // Địa chỉ WebAPI backend
    private string apiUrl = "http://localhost:5203/api/auth";

    public void OnLoginClicked()
    {
        StartCoroutine(Login());
    }

    public void OnRegisterClicked()
    {
        StartCoroutine(Register());
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void ShowLoginPanel()
    {
        registerPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    IEnumerator Login()
    {
        LoginData loginData = new LoginData
        {
            CustomId = inputLoginUsername.text,
            Password = inputLoginPassword.text,
            CreateAccount = false
        };

        string json = JsonUtility.ToJson(loginData);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                textStatus_login.text = "Sai mật khẩu hoặc lỗi kết nối.";
            }
            else
            {
                string responseText = www.downloadHandler.text;
                AuthResponse authResponse = JsonUtility.FromJson<AuthResponse>(FixJson(responseText));

                PlayerPrefs.SetString("jwt_token", authResponse.token);
                PlayerPrefs.SetString("user_role", authResponse.role);
                PlayerPrefs.Save();

                textStatus_login.text = $"Đăng nhập thành công. Role: {authResponse.role}";

                if (authResponse.role == "Admin")
                {
                    Debug.Log("Bạn là Admin. Mở giao diện quản trị...");
                    // SceneManager.LoadScene("AdminScene"); // nếu muốn chuyển cảnh
                }
            }
        }
    }

    IEnumerator Register()
    {
        RegisterData registerData = new RegisterData
        {
            Username = inputRegisterUsername.text,
            Password = inputRegisterPassword.text
        };

        string json = JsonUtility.ToJson(registerData);
        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                textStatus_register.text = "Tài khoản đã tồn tại hoặc lỗi.";
            else
                textStatus_register.text = "Đăng ký thành công.";
        }
    }
    IEnumerator AuthenticatedPost(string endpoint, string json, System.Action<string> onSuccess, System.Action<string> onError)
{
    string token = PlayerPrefs.GetString("jwt_token", "");

    using (UnityWebRequest www = new UnityWebRequest(apiUrl + endpoint, "POST"))
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();

        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + token);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            onError?.Invoke(www.downloadHandler.text);
        else
            onSuccess?.Invoke(www.downloadHandler.text);
    }
}

    // 🔧 Hỗ trợ JsonUtility nếu tên field trả về viết hoa
    string FixJson(string value)
    {
        value = value.Replace("\"Token\"", "\"token\"")
                     .Replace("\"Username\"", "\"username\"")
                     .Replace("\"Role\"", "\"role\"");
        return value;
    }
}

[System.Serializable]
public class LoginData
{
    public string CustomId;
    public string Password; // 👈 THÊM
    public bool CreateAccount;
}

[System.Serializable]
public class RegisterData
{
    public string Username;
    public string Password;
}

[System.Serializable]
public class AuthResponse
{
    public string token;
    public string username;
    public string role;
}
