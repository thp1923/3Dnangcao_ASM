using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public GameObject pressAnyButtonText;
    public GameObject messengerImage;
    public GameObject panelLogin;

    private bool canPress = false;
    private int pressCount = 0;

    void Start()
    {
        pressAnyButtonText.SetActive(false);
        messengerImage.SetActive(false);
        panelLogin.SetActive(false);

        // Sau 5s mới hiện PRESS ANY BUTTON
        Invoke(nameof(ShowPressAnyButton), 5f);
    }

    void ShowPressAnyButton()
    {
        pressAnyButtonText.SetActive(true);
        canPress = true;
    }

    void Update()
    {
        if (canPress && Input.anyKeyDown)
        {
            pressCount++;

            if (pressCount == 1)
            {
                // Lần đầu nhấn: hiện Messenger
                messengerImage.SetActive(true);
            }
            else if (pressCount == 2)
            {
                // Lần nhấn thứ 2: hiện Panel Login
                panelLogin.SetActive(true);

                // Ẩn những thứ không cần nữa
                messengerImage.SetActive(false);
                pressAnyButtonText.SetActive(false);
                this.enabled = false; // tắt script
            }
        }
    }
}
