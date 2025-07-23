using UnityEngine;
using TMPro;

public class UIDoorMes : MonoBehaviour
{
    public static UIDoorMes instance;
    public TextMeshProUGUI messageText;

    void Awake()
    {
        instance = this;
        messageText.enabled = false;
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;        
    }

    public void HideMessage()
    {
        messageText.enabled = false;    
    }
}
