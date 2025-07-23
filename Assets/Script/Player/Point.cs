using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    UpgradeStats upStats;
    int point;

    public TMPro.TextMeshProUGUI pointText;
    // Start is called before the first frame update
    void Start()
    {
        upStats = GameObject.FindWithTag("Stats").GetComponent<UpgradeStats>();
        point = upStats.Point;
        pointText.text = point.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(point != upStats.Point)
        {
            point = upStats.Point;
            pointText.text = point.ToString();
        }
    }
}
