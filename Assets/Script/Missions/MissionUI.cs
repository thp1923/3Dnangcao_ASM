using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;
public class MissionUI : MonoBehaviour
{
    public GameObject missionPanel;
    public TMP_Text missionListText;
    public List<Mission> allMissions;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            missionPanel.SetActive(!missionPanel.activeSelf);
            if (missionPanel.activeSelf)
                UpdateMissionList();
        }
    }
    void UpdateMissionList()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("In-Progress Missions:");
        foreach (var mission in allMissions)
        {
            if (mission.state == MissionState.InProgress)
            {
                sb.AppendLine($"- {mission.missionName}: {mission.missionDescription}");
            }
        }
        missionListText.text = sb.ToString();
    }
}
