using UnityEngine;

public enum  MissionState
{
    NotStarted,
    InProgress,
    Completed,
    Failed
}
[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/Mission")]
public class Mission : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public bool isMainStory;

    [HideInInspector]
    public MissionState state = MissionState.NotStarted;

    public void OnMissionCompleted()
    {
        state = MissionState.Completed;
    }
}
