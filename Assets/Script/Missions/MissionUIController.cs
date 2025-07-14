using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionUIController : MonoBehaviour
{
    public GameObject missionLogUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            missionLogUI.SetActive(!missionLogUI.activeSelf);
        }
    }
}
