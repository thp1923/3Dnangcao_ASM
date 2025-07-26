using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    public GameObject Sword;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SwordSwich(MeshFilter sword_Mesh)
    {
        if (sword_Mesh != null && Sword != null)
        {
            MeshFilter currentMeshFilter = Sword.GetComponent<MeshFilter>();
            if (currentMeshFilter != null)
            {
                currentMeshFilter.mesh = sword_Mesh.sharedMesh; // hoặc .mesh nếu bạn muốn clone
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
