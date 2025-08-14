using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [Header("Mission Data")]
    [SerializeField]
    public List<Mission> missions = new List<Mission>();

    [Header("UI References")]
    public GameObject questCardPrefab; // Prefab của Quest_Card
    public Transform contentHolder;    // Content trong Scroll View

    void Start()
    {
        GenerateQuestCards();
    }
    public void AddMission(Mission mission)
    {
        if (mission == null)
        {
            Debug.LogWarning("Mission is null.");
            return;
        }

        Debug.Log($"Trying to add mission: {mission.missionName}");

        bool alreadyExists = missions.Exists(m => m.missionName == mission.missionName);

        if (!alreadyExists)
        {
            missions.Add(mission);
            Debug.Log($"Successfully added mission: {mission.missionName}");
        }
        else
        {
            Debug.Log($"Mission already exists: {mission.missionName}");
        }

        Debug.Log($"Mission list now has {missions.Count} missions.");
        GenerateQuestCards();
    }
    public void GenerateQuestCards()
    {
        // Xóa card cũ
        foreach (Transform child in contentHolder)
        {
            Destroy(child.gameObject);
        }

        Debug.Log($"Generating {missions.Count} mission cards...");

        foreach (Mission mission in missions)
        {
            if (mission == null)
                continue;

            Debug.Log($"Render mission card: {mission.missionName}, State: {mission.state}");

            GameObject card = Instantiate(questCardPrefab, contentHolder);
            QuestCardUI cardUI = card.GetComponent<QuestCardUI>();
            if (cardUI != null)
            {
                cardUI.Setup(mission);
            }
        }
    }
}
