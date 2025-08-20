using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAchievement : MonoBehaviour
{
    [Header("Boss Achievements")]
    public bool unlockOnDeath = true;
    public TrophyType trophyToUnlock;

    private bool isunlocked = false;
    public void TryUnlock()
    {
        if (!unlockOnDeath || isunlocked) return;

        var gjManager = GameObject.FindObjectOfType<GameJoltManager>();
        if (gjManager != null)
        {
            gjManager.UnlockTrophy(trophyToUnlock);
            isunlocked = true;
        }
    }
}
