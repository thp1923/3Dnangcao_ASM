using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [Header("Mission Data")]
    public List<Mission> missions;

    [Header("UI References")]
    public GameObject questCardPrefab; // Prefab của Quest_Card
    public Transform contentHolder;    // Content trong Scroll View

    void Start()
    {
        GenerateQuestCards();
    }

    void GenerateQuestCards()
    {
        // Xóa các card cũ nếu có
        foreach (Transform child in contentHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (Mission mission in missions)
        {
            GameObject card = Instantiate(questCardPrefab, contentHolder);
            QuestCardUI cardUI = card.GetComponent<QuestCardUI>();
            if (cardUI != null)
            {
                cardUI.Setup(mission);
            }
        }
    }
}
