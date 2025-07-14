using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionUIController : MonoBehaviour
{
    public GameObject missionLogUI;
    public GameObject missionEntryPrefab;
    public Transform missionListParent;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            missionLogUI.SetActive(!missionLogUI.activeSelf);
        }
    }

    public void RefreshMissionLog()
    {
        foreach (Transform child in missionListParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var mission in MissionManager.instance.activeMissions)
        {
            GameObject entry = Instantiate(missionEntryPrefab, missionListParent);
            entry.GetComponentInChildren<TMP_Text>().text = $"{mission.missionName}\n{mission.missionDescription}";
        }
    }
}
