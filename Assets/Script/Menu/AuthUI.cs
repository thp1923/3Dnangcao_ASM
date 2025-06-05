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
    public GameObject forgotPasswordPanel;
    public GameObject resetPasswordPanel;
    public GameObject changePasswordPanel; // New panel for change-password

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

    [Header("Forgot Password UI")]
    public TMP_InputField inputForgotEmail;
    public TextMeshProUGUI textStatus_forgot;

    [Header("Reset Password UI")]
    public TMP_InputField inputResetCode;
    public TMP_InputField inputNewPassword;
    public TMP_InputField inputConfirmNewPassword;
    public TextMeshProUGUI textStatus_reset;

    [Header("Change Password UI")] // New fields
    public TMP_InputField inputChangeUsername;
    public TMP_InputField inputChangeOldPassword;
    public TMP_InputField inputChangeNewPassword;
    public TMP_InputField inputChangeConfirmNewPassword;
    public TextMeshProUGUI textStatus_change;
    [Header("Loading UI")]
    public GameObject loadingPanel;
    void ShowLoading() => loadingPanel.SetActive(true);
    void HideLoading() => loadingPanel.SetActive(false);
    [Header("Main Menu UI")]
    public GameObject mainMenuPanel;
    void ShowMainMenu()
    {
    loginPanel.SetActive(false);
    mainMenuPanel.SetActive(true);
    }

    private string apiUrl = "https://database-namelessknightii.onrender.com/api/auth";


    public void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        confirmEmailPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        changePasswordPanel.SetActive(false);
    }

    public void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        confirmEmailPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        changePasswordPanel.SetActive(false);
    }

    public void ShowConfirmPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        confirmEmailPanel.SetActive(true);
        forgotPasswordPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        changePasswordPanel.SetActive(false);
    }

    public void ShowForgotPasswordPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        confirmEmailPanel.SetActive(false);
        forgotPasswordPanel.SetActive(true);
        resetPasswordPanel.SetActive(false);
        changePasswordPanel.SetActive(false);
    }

    public void ShowResetPasswordPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        confirmEmailPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        resetPasswordPanel.SetActive(true);
        changePasswordPanel.SetActive(false);
    }

    public void ShowChangePasswordPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        confirmEmailPanel.SetActive(false);
        forgotPasswordPanel.SetActive(false);
        resetPasswordPanel.SetActive(false);
        changePasswordPanel.SetActive(true);
    }

    public void OnLoginClicked() => StartCoroutine(Login());
    public void OnRegisterClicked() => StartCoroutine(RegisterRoutine());
    public void OnConfirmClicked() => StartCoroutine(ConfirmEmail());
    public void OnResendClicked() => StartCoroutine(RegisterRoutine());
    public void OnSendForgotCodeClicked() => StartCoroutine(SendForgotPasswordCode());
    public void OnResetPasswordClicked() => StartCoroutine(ResetPassword());
    public void OnChangePasswordClicked() => StartCoroutine(ChangePassword()); // New hook

    IEnumerator Login()
    {
        ShowLoading();
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
                StartCoroutine(ShowTemporaryMessage(textStatus_login, ExtractErrorMessage(serverMessage)));
            }
            else if (www.responseCode == 400)
            {
                if (serverMessage.Contains("Username không tồn tại"))
                    StartCoroutine(ShowTemporaryMessage(textStatus_login, "Tài khoản không tồn tại."));
                else if (serverMessage.Contains("Password không đúng"))
                    StartCoroutine(ShowTemporaryMessage(textStatus_login, "Mật khẩu không đúng."));
                else if (serverMessage.Contains("Email chưa xác thực"))
                    StartCoroutine(ShowTemporaryMessage(textStatus_login, "Email chưa xác thực."));
                else
                    StartCoroutine(ShowTemporaryMessage(textStatus_login, "Đăng nhập thất bại."));
            }
            else
            {
                var res = JsonUtility.FromJson<AuthResponse>(FixJson(serverMessage));
                PlayerPrefs.SetString("jwt_token", res.token);
                PlayerPrefs.SetString("user_role", res.role);
                PlayerPrefs.Save();
                StartCoroutine(ShowTemporaryMessage(textStatus_login, "Đăng nhập thành công."));
                ShowMainMenu();
            }
            HideLoading();
        }
    }

    IEnumerator RegisterRoutine()
    {
        ShowLoading();
        string password = inputRegisterPassword.text.Trim();
        string confirmPassword = inputRegisterConfirmPassword.text.Trim();
        string email = inputRegisterEmail.text.Trim();

        if (string.IsNullOrWhiteSpace(inputRegisterUsername.text) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_register, "Không được để trống bất kỳ trường nào."));
            HideLoading();
            yield break;
            
        }

        if (!IsValidEmail(email))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_register, "Email không hợp lệ."));
            yield break;

        }

        if (!email.EndsWith("@gmail.com"))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_register, "Chỉ chấp nhận địa chỉ Gmail."));
            HideLoading();
            yield break;

        }

        if (password != confirmPassword)
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_register, "Mật khẩu không khớp với mật khẩu xác nhận."));
            HideLoading();
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

        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/register-request", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            string serverMessage = www.downloadHandler.text;

            if (www.result != UnityWebRequest.Result.Success)
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_register, ExtractErrorMessage(serverMessage)));
            }
            else if ((int)www.responseCode == 500 && serverMessage.Contains("Gửi email thất bại"))
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_register, "Địa chỉ Gmail không hợp lệ hoặc không tồn tại."));
            }
            else if (www.responseCode == 400)
            {
                if (serverMessage.Contains("Username đã tồn tại"))
                    StartCoroutine(ShowTemporaryMessage(textStatus_register, "Username đã tồn tại."));
                else if (serverMessage.Contains("Email này đã được sử dụng"))
                    StartCoroutine(ShowTemporaryMessage(textStatus_register, "Email đã được sử dụng."));
                else
                    StartCoroutine(ShowTemporaryMessage(textStatus_register, "Lỗi đăng ký."));
            }
            else
            {
                PlayerPrefs.SetString("pending_email", email);
                PlayerPrefs.Save();
                StartCoroutine(ShowTemporaryMessage(textStatus_register, "Đã gửi mã xác thực tới email của bạn."));
                ShowConfirmPanel();
            }
            HideLoading();
        }
    }

    IEnumerator ConfirmEmail()
    {
        ShowLoading();
        string email = PlayerPrefs.GetString("pending_email", "").Trim();
        string code = inputConfirmCode.text.Trim();

        if (string.IsNullOrWhiteSpace(code))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_confirm, "Vui lòng nhập mã xác thực."));
            HideLoading();
            yield break;
        }

        if (string.IsNullOrEmpty(email))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_confirm, "Không tìm thấy email để xác thực."));
            HideLoading();
            yield break;
        }

        var data = new ConfirmEmailRequest { Email = email, Code = code };
        string json = JsonUtility.ToJson(data);

        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/confirm-email", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            string serverMessage = www.downloadHandler.text;

            if (www.result != UnityWebRequest.Result.Success)
                StartCoroutine(ShowTemporaryMessage(textStatus_confirm, "Không thể kết nối."));
            else if (www.responseCode == 400)
                StartCoroutine(ShowTemporaryMessage(textStatus_confirm, "Mã không đúng hoặc đã hết hạn."));
            else
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_confirm, "Xác thực thành công"));
                PlayerPrefs.DeleteKey("pending_email");
                yield return new WaitForSeconds(1.5f);
                ShowLoginPanel();
            }
            HideLoading();
        }
    }

    IEnumerator SendForgotPasswordCode()
    {
        ShowLoading();
        string email = inputForgotEmail.text.Trim();
        if (string.IsNullOrEmpty(email))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_forgot, "Vui lòng nhập email."));
            HideLoading();
            yield break;
        }

        var data = new ForgotPasswordRequest { Email = email };
        string json = JsonUtility.ToJson(data);

        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/forgot-password", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();


            if (www.result == UnityWebRequest.Result.Success)
            {
                PlayerPrefs.SetString("reset_email", email);
                ShowResetPasswordPanel();
            }
            else
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_forgot, ExtractErrorMessage(www.downloadHandler.text)));
            }
            HideLoading();
        }
    }

    IEnumerator ResetPassword()
    {
        ShowLoading();
        string email = PlayerPrefs.GetString("reset_email", "");
        string code = inputResetCode.text.Trim();
        string newPass = inputNewPassword.text;
        string confirmPass = inputConfirmNewPassword.text;

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_reset, "Vui lòng điền đầy đủ thông tin."));
            HideLoading();
            yield break;
        }

        if (newPass != confirmPass)
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_reset, "Mật khẩu xác nhận không khớp."));
            HideLoading();
            yield break;
        }

        var req = new ResetPasswordConfirmRequest
        {
            Email = email,
            Code = code,
            NewPassword = newPass,
            ConfirmNewPassword = confirmPass
        };

        string json = JsonUtility.ToJson(req);

        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/reset-password", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_reset, "Đổi mật khẩu thành công."));
                yield return new WaitForSeconds(1.5f);
                ShowLoginPanel();
            }
            else
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_reset, ExtractErrorMessage(www.downloadHandler.text)));
            }
            HideLoading();
        }
    }

    IEnumerator ChangePassword()
    {
        ShowLoading();
        string username = inputChangeUsername.text.Trim();
        string oldPass = inputChangeOldPassword.text;
        string newPass = inputChangeNewPassword.text;
        string confirmPass = inputChangeConfirmNewPassword.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPass) ||
            string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_change, "Vui lòng điền đầy đủ thông tin."));
            HideLoading();
            yield break;
        }

        if (newPass != confirmPass)
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_change, "Mật khẩu mới và xác nhận không khớp."));
            HideLoading();
            yield break;
        }

        if (newPass.Length < 6 || newPass.Length > 24)
        {
            StartCoroutine(ShowTemporaryMessage(textStatus_change, "Mật khẩu phải từ 6 đến 24 ký tự."));
            HideLoading();
            yield break;
        }

        var data = new ChangePasswordRequest
        {
            Username = username,
            OldPassword = oldPass,
            NewPassword = newPass,
            ConfirmNewPassword = confirmPass
        };

        string json = JsonUtility.ToJson(data);

        using (UnityWebRequest www = new UnityWebRequest(apiUrl + "/change-password", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_change, "Đổi mật khẩu thành công."));
                yield return new WaitForSeconds(1.5f);
                ShowLoginPanel();
            }
            else
            {
                StartCoroutine(ShowTemporaryMessage(textStatus_change, ExtractErrorMessage(www.downloadHandler.text)));
            }
            HideLoading();
        }
    }

    bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
    }

    string FixJson(string value)
    {
        return value.Replace("\"Token\"", "\"token\"")
                    .Replace("\"Username\"", "\"username\"")
                    .Replace("\"Role\"", "\"role\"");
    }

    IEnumerator ShowTemporaryMessage(TextMeshProUGUI label, string message, float duration = 3f)
    {
        label.text = message;
        label.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        label.text = "";
        label.gameObject.SetActive(false);
    }

    string ExtractErrorMessage(string json)
    {
        try
        {
            ErrorResponse errorRes = JsonUtility.FromJson<ErrorResponse>(json);
            return string.IsNullOrEmpty(errorRes.error) ? "Lỗi không xác định." : errorRes.error;
        }
        catch
        {
            return "Định dạng phản hồi không hợp lệ.";
        }
    }

    [System.Serializable] public class LoginData { public string username; public string Password; public bool CreateAccount; }
    [System.Serializable] public class AuthResponse { public string token; public string username; public string role; }
    [System.Serializable] public class ConfirmEmailRequest { public string Email; public string Code; }
    [System.Serializable] public class ForgotPasswordRequest { public string Email; }
    [System.Serializable] public class ResetPasswordConfirmRequest { public string Email; public string Code; public string NewPassword; public string ConfirmNewPassword; }
    [System.Serializable] private class RegisterRequest { public string Username; public string Email; public string Password; public string ConfirmPassword; }
    [System.Serializable] private class ChangePasswordRequest { public string Username; public string OldPassword; public string NewPassword; public string ConfirmNewPassword; }
    [System.Serializable] public class ErrorResponse { public string error; }
}
