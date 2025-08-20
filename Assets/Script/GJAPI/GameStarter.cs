using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnNewclickGame()
    {
        GameObject.FindObjectOfType<GameJoltManager>()?.UnlockTrophy(TrophyType.StartNewGame);
    }
}
