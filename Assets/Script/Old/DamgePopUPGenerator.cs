using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamgePopUPGenerator : MonoBehaviour
{
    public static DamgePopUPGenerator current;

    public GameObject prefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        current = this;
    }
    public void createPopUp(Vector3 position, string text)
    {
        var popUp = Instantiate(prefabs, position, Quaternion.identity);
        var temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popUp, 1f);
        Destroy(temp, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
