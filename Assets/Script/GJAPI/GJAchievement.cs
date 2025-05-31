using GameJolt.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GJAchievement : MonoBehaviour
{
    [Header("Set your Trophy ID from Game Jolt Dashboard")]
    public int trophyId = 269454;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (GameJoltAPI.Instance.CurrentUser != null)
            {
                Trophies.Unlock(trophyId, (bool success) =>
                {
                    Debug.Log(success);
                });
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.K) && GameJoltAPI.Instance.CurrentUser == null)
        {
            Debug.LogError("Failed to unlock trophy or already unlocked!");
        }
    }
}
