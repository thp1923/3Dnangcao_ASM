using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderr : MonoBehaviour
{
    public static SceneLoaderr Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // Gọi từ Menu, Trigger, Boss...
    public void LoadLevel(int sceneIndex)
    {
        // Lưu lại chỉ số scene đích
        PlayerPrefs.SetInt("NextSceneIndex", sceneIndex);
        // Chuyển ngay sang scene loading
        SceneManager.LoadScene("LoadScene");
    }

    internal object LoadScene(int nextSceneIndex)
    {
        throw new NotImplementedException();
    }
}
