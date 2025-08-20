using GameJolt.API;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCollector : MonoBehaviour
{
    [Header("Thành tựu sẽ mở khi đã lấy tất cả")]
    public TrophyType collectAllTrophy = TrophyType.CollectAllTrophy;

    [Header("Trophy sẽ bị bỏ qua khi kiểm tra")]
    public List<TrophyType> ignoredTrophies = new List<TrophyType> { TrophyType.TestingTrophy };

    private bool isChecking = false;

    public void CheckAllTrophiesUnlocked()
    {
        if (isChecking) return;
        isChecking = true;

        Trophies.Get(trophies =>
        {
            if (trophies == null)
            {
                //Debug.LogWarning("Không thể lấy danh sách trophies.");
                isChecking = false;
                return;
            }

            var manager = FindObjectOfType<GameJoltManager>();
            if (manager == null)
            {
                //Debug.LogWarning("Không tìm thấy GameJoltManager.");
                isChecking = false;
                return;
            }

            bool allUnlocked = true;

            foreach (var trophy in trophies)
            {
                TrophyType? type = manager.GetTrophyTypeById(trophy.ID);
                if (type == null)
                {
                    //Debug.LogWarning($"Không xác định được TrophyType cho ID: {trophy.ID}");
                    continue;
                }

                if (type == collectAllTrophy || ignoredTrophies.Contains(type.Value))
                {
                    continue; // bỏ qua trophy đặc biệt
                }

                if (!trophy.Unlocked)
                {
                    allUnlocked = false;
                    break;
                }
            }

            if (allUnlocked)
            {
                manager.UnlockTrophy(collectAllTrophy);
                //Debug.Log("Đã mở thành tựu: Collect All!");
            }

            isChecking = false;
        });
    }
}
