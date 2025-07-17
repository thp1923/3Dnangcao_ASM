using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BarukClawsTrail : MonoBehaviour
{
    public GameObject[] trailEffects;
    public MeleeWeapon[] meleeWeapons;

    private bool[] damageEnabled;

    private void Start()
    {
        damageEnabled = new bool[trailEffects.Length];

        for (int i = 0; i < trailEffects.Length; i++)
        {
            if (trailEffects[i] != null)
                trailEffects[i].gameObject.SetActive(false);

            damageEnabled[i] = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < damageEnabled.Length; i++)
        {
            if (damageEnabled[i] && meleeWeapons[i] != null)
            {
                meleeWeapons[i].Activate();
            }
        }
    }

    // Hàm dùng cho Unity Event hoặc Animation Event
    public void PlayParticalOn(int index)
    {
        SetTrailState(index, true);
    }

    public void PlayParticalOff(int index)
    {
        SetTrailState(index, false);
    }

    private void SetTrailState(int index, bool state)
    {
        if (index < 0 || index >= trailEffects.Length || index >= meleeWeapons.Length)
        {
            //Debug.LogWarning("Index out of range!");
            return;
        }

        trailEffects[index].gameObject.SetActive(state);
        damageEnabled[index] = state;

        if (!state)
        {
            meleeWeapons[index].ResetSettings();
        }
    }
}
