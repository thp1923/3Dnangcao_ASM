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
            Debug.LogWarning("Mission is not assigned in MissionTrigger.");
            return;
        }

        Debug.Log($"Triggered by: {other.name}, Mission: '{mission.missionName}', State: {mission.state}");

#if UNITY_EDITOR
        HandleMissionStateForEditor();
#else
        HandleMissionStateForRuntime();
#endif
    }

    void HandleMissionStateForEditor()
    {
        switch (mission.state)
        {
            case MissionState.NotStarted:
                mission.state = MissionState.InProgress;
                Debug.Log("Mission state set to InProgress (Editor)");
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
    }

    void HandleMissionStateForRuntime()
    {
        if (mission.state == MissionState.NotStarted)
        {
            mission.state = MissionState.InProgress;
            Debug.Log("Mission state set to InProgress (runtime)");
            ShowMessage($"ĐÃ NHẬN: {mission.missionName}");
            TryAddToManager();
        }
    }

    void ShowMessage(string message)
    {
        if (notifier != null)
        {
            Debug.Log("Showing message: " + message);
            notifier.ShowMissionNotification(message);
        }
        else
        {
            Debug.LogWarning("MissionNotifier is not assigned.");
        }
    }

    void TryAddToManager()
    {
        if (questManager == null)
        {
            Debug.LogWarning("QuestManager is not assigned.");
            return;
        }

        bool alreadyExists = questManager.missions.Any(m => m != null && m.missionName == mission.missionName);
        if (!alreadyExists)
        {
            questManager.missions.Add(mission);
            Debug.Log("Successfully added mission: " + mission.missionName);
        }
        else
        {
            Debug.Log("Mission already exists in QuestManager: " + mission.missionName);
        }

        Debug.Log("Calling GenerateQuestCards()");
        questManager.GenerateQuestCards();
    }
}
