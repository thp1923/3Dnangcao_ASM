using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    public GameObject[] SwordMesh;
    protected int weaponId;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SwordSwich(int WeaponId)
    {
        weaponId = WeaponId;

        for (int i = 0; i < SwordMesh.Length; i++)
        {
            if (SwordMesh[i] != null)
            {
                // Chỉ bật mesh có index trùng với weaponId, tắt những cái khác
                SwordMesh[i].SetActive(i == weaponId);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
