using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Text;
using System;

public class AuthUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject confirmEmailPanel;

    [Header("Login UI")]
    public TMP_InputField inputLoginUsername;
    public TMP_InputField inputLoginPassword;
    public TextMeshProUGUI textStatus_login;

    [Header("Register UI")]
    public TMP_InputField inputRegisterUsername;
    public TMP_InputField inputRegisterPassword;
    public TMP_InputField inputRegisterEmail;
    public TMP_InputField inputRegisterConfirmPassword;
    public TextMeshProUGUI textStatus_register;

    [Header("Confirm Email UI")]
    public TMP_InputField inputConfirmCode;
    public TextMeshProUGUI textStatus_confirm;

    private string apiUrl = "http://localhost:5203/api/auth";

    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        confirmEmailPanel.SetActive(false);
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        confirmEmailPanel.SetActive(false);
    }

    public void ShowConfirmPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        confirmEmailPanel.SetActive(true);
    }

    public void OnLoginClicked() => StartCoroutine(Login());
    public void OnRegisterClicked() => StartCoroutine(RegisterRequest());
    public void OnConfirmClicked() => StartCoroutine(ConfirmEmail());
    public void OnResendClicked() => StartCoroutine(RegisterRequest());

    IEnumerator Login()
{
    var data = new LoginData
    {
        username = inputLoginUsername.text.Trim(), 
        Password = inputLoginPassword.text.Trim(),
        CreateAccount = false
    };

    string json = JsonUtility.ToJson(data);

    using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/login", "POST"))
    {
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        string serverMessage = www.downloadHandler.text;

        if (www.result != UnityWebRequest.Result.Success)
        {
            textStatus_login.text = serverMessage;
        }
        else if (www.responseCode == 400)
        {
            Debug.LogError("[LOGIN] 400 Bad Request. Server: " + serverMessage);

            if (serverMessage.Contains("Username không tồn tại"))
                textStatus_login.text = "Tài khoản không tồn tại.";
            else if (serverMessage.Contains("Password không đúng"))
                textStatus_login.text = "Mật khẩu không đúng.";
            else if (serverMessage.Contains("Email chưa xác thực"))
                textStatus_login.text = "Email chưa xác thực.";
            else
                textStatus_login.text = "Đăng nhập thất bại.";
        }
        else // ✅ Thành công
        {

            var res = JsonUtility.FromJson<AuthResponse>(FixJson(serverMessage));
            PlayerPrefs.SetString("jwt_token", res.token);
            PlayerPrefs.SetString("user_role", res.role);
            PlayerPrefs.Save();

            textStatus_login.text = "Đăng nhập thành công.";
        }
    }
}


   IEnumerator RegisterRequest()
{
    string password = inputRegisterPassword.text.Trim();
    string confirmPassword = inputRegisterConfirmPassword.text.Trim();
    string email = inputRegisterEmail.text.Trim();

    // Check empty fields
    if (string.IsNullOrWhiteSpace(inputRegisterUsername.text) ||
        string.IsNullOrWhiteSpace(email) ||
        string.IsNullOrWhiteSpace(password) ||
        string.IsNullOrWhiteSpace(confirmPassword))
    {
        textStatus_register.text = "Không được để trống bất kỳ trường nào.";
        yield break;
    }

    // Check email format
    if (!IsValidEmail(email))
    {
        textStatus_register.text = "Email không hợp lệ.";
        yield break;
    }

    // Only allow @gmail.com domain
    if (!email.EndsWith("@gmail.com"))
    {
        textStatus_register.text = "Chỉ chấp nhận địa chỉ Gmail.";
        yield break;
    }

    // Check password match
    if (password != confirmPassword)
    {
        textStatus_register.text = "Mật khẩu không khớp với mật khẩu xác nhận.";
        yield break;
    }

    var data = new RegisterRequest
    {
        Username = inputRegisterUsername.text.Trim(),
        Email = email,
        Password = password,
        ConfirmPassword = confirmPassword
    };

    string json = JsonUtility.ToJson(data);
    Debug.Log("[REGISTER] JSON gửi đi: " + json);

    using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/register-request", "POST"))
    {
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        string serverMessage = www.downloadHandler.text;
        Debug.Log("[REGISTER] Server response: " + serverMessage);
        

        if (www.result != UnityWebRequest.Result.Success)
            {
                textStatus_register.text = "Không kết nối được máy chủ.";
            }
            else if ((int)www.responseCode == 500 && serverMessage.Contains("Gửi email thất bại"))
            {
                textStatus_register.text = "Địa chỉ Gmail không hợp lệ hoặc không tồn tại.";
            }
            else if (www.responseCode == 400)
            {
                if (serverMessage.Contains("Username đã tồn tại"))
                    textStatus_register.text = "Username đã tồn tại.";
                else if (serverMessage.Contains("Email này đã được sử dụng"))
                    textStatus_register.text = "Email đã được sử dụng.";
                else
                    textStatus_register.text = "Lỗi đăng ký.";
            }
            else
            {
                PlayerPrefs.SetString("pending_email", email);
                PlayerPrefs.Save();
                textStatus_register.text = "Đã gửi mã xác thực tới email của bạn.";
                ShowConfirmPanel(); // Switch to confirm panel
            }
    }
}

// Add this method inside the same class
bool IsValidEmail(string email)
{
    if (string.IsNullOrWhiteSpace(email))
        return false;
    string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
}


    IEnumerator ConfirmEmail()
{
    string email = PlayerPrefs.GetString("pending_email", "").Trim();
    string code = inputConfirmCode.text.Trim();

    Debug.Log($"[CONFIRM EMAIL] Email từ PlayerPrefs: {email}");
    Debug.Log($"[CONFIRM EMAIL] Mã xác thực nhập: {code}");

    if (string.IsNullOrWhiteSpace(code))
    {
        textStatus_confirm.text = "Vui lòng nhập mã xác thực.";
        yield break;
    }

    if (string.IsNullOrEmpty(email))
    {
        textStatus_confirm.text = "Không tìm thấy email để xác thực. Vui lòng đăng ký lại.";
        yield break;
    }

    var data = new ConfirmEmailRequest
    {
        Email = email,
        Code = code
    };

    string json = JsonUtility.ToJson(data);
    Debug.Log($"[CONFIRM EMAIL] JSON gửi: {json}");

    using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/confirm-email", "POST"))
    {
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        string serverMessage = www.downloadHandler.text;
        Debug.Log($"[CONFIRM EMAIL] Server response: {serverMessage}");

        if (www.result != UnityWebRequest.Result.Success)
        {
            textStatus_confirm.text = "Không thể kết nối tới máy chủ.";
        }
        else if (www.responseCode == 400)
        {
            if (serverMessage.Contains("Mã xác thực không hợp lệ") || serverMessage.Contains("đã hết hạn"))
                textStatus_confirm.text = "Mã xác thực không đúng hoặc đã hết hạn.";
            else if (serverMessage.Contains("Email đã được xác thực"))
                textStatus_confirm.text = "Email đã được xác thực trước đó.";
            else if (serverMessage.Contains("Username đã được tạo"))
                textStatus_confirm.text = "Username đã được người khác xác thực trước.";
            else
                textStatus_confirm.text = "Xác thực thất bại.";
        }
        else
        {
            textStatus_confirm.text = "✅ Tài khoản đã xác thực thành công!";
            PlayerPrefs.DeleteKey("pending_email");

            yield return new WaitForSeconds(1.5f); // Cho người dùng thấy thông báo

            ShowLoginPanel(); // 👈 chuyển về login sau khi xác thực
        }
    }
}



    string FixJson(string value)
    {
        return value.Replace("\"Token\"", "\"token\"")
                    .Replace("\"Username\"", "\"username\"")
                    .Replace("\"Role\"", "\"role\"");
    }

    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string Password;
        public bool CreateAccount;
    }

    [System.Serializable]
    public class AuthResponse
    {
        public string token;
        public string username;
        public string role;
    }


    [System.Serializable]
    public class ConfirmEmailRequest
    {
        public string Email;
        public string Code;
    }
}

[System.Serializable]
public class RegisterRequest
{
    public string Username;
    public string Email;
    public string Password;
    public string ConfirmPassword;
}