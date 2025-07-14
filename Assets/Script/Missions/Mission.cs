using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "Missions/Mission")]
public class Mission : ScriptableObject
{
    public string missionName;
    public string missionDescription;
    public bool isMainStory;
}
