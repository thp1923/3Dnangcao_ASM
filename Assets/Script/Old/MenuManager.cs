using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region StartGame
    public void StartGame()
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region ExitGame
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion
}
