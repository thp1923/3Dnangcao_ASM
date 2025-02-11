using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int enemyCount;
    public TMPro.TextMeshProUGUI scoreText;
    public GameObject Win;
    // Start is called before the first frame update
    void Start()
    {
        //scoreText.text = enemyCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //if(enemyCount >= 3)
        //{
        //    Time.timeScale = 0;
        //    Win.SetActive(true);
        //}
    }
    public void AddEnemyCount()
    {
        enemyCount++;
        scoreText.text = enemyCount.ToString();
    }
    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
