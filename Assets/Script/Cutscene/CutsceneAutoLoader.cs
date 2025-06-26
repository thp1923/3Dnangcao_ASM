using System.Collections;
using UnityEngine;

public class CutsceneAutoLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The build index of your game scene")]
    public int gameSceneIndex = 3;

    [Tooltip("Time to wait on the loading screen before loading the game scene")]
    public float waitTime = 2f;

    void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(waitTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneIndex);
    }
}
