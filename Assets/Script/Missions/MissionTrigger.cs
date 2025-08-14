using System.Linq;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public Mission mission;
    public MissionNotifier notifier;
    public QuestManager questManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (mission == null)
        {
            Debug.LogWarning("Mission is NOT assigned on this trigger.");
            return;
        }

        Debug.Log($"Triggered by: {other.name}, Mission: '{mission.missionName}' - Current State: {mission.state}");

#if UNITY_EDITOR
        switch (mission.state)
        {
            case MissionState.NotStarted:
                mission.state = MissionState.InProgress;
                Debug.Log("Mission set to InProgress (Test Mode)");
                ShowMessage($"[TEST] Nhận: {mission.missionName}");
                TryAddToManager();
                break;

            case MissionState.InProgress:
                ShowMessage($"[TEST] Đang làm: {mission.missionName}");
                break;

            case MissionState.Completed:
                ShowMessage($"[TEST] Đã hoàn thành: {mission.missionName}");
                break;

            case MissionState.Failed:
                ShowMessage($"[TEST] Thất bại: {mission.missionName}");
                break;
        }
#else
        if (mission.state == MissionState.NotStarted)
        {
            mission.state = MissionState.InProgress;
            Debug.Log("Mission set to InProgress (Runtime)");
            ShowMessage($"ĐÃ NHẬN: {mission.missionName}");
            TryAddToManager();
        }
#endif
    }

    void ShowMessage(string message)
    {
        if (notifier != null)
        {
            notifier.ShowMissionNotification(message);
            Debug.Log("Showing message: " + message);
        }
        else
        {
            Debug.LogWarning("MissionNotifier is NOT assigned.");
        }
    }

    void TryAddToManager()
    {
        if (questManager == null)
        {
            Debug.LogWarning("QuestManager is NOT assigned.");
            return;
        }

        bool alreadyExists = questManager.missions.Any(m => m.missionName == mission.missionName);

        if (!alreadyExists)
        {
            questManager.missions.Add(mission);
            Debug.Log("Successfully added mission: " + mission.missionName);
        }
        else
        {
            Debug.Log("Mission already exists in QuestManager: " + mission.missionName);
        }

        questManager.GenerateQuestCards();
        Debug.Log("QuestPanel updated via GenerateQuestCards().");
    }
}
