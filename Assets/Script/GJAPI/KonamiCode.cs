using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonamiCode : MonoBehaviour
{
    [Header("Konami Code Settings")]
    public TrophyType TrophyType;

    private int progress = 0;

    private enum KonamiCodeInput
    {
        W, W2, S, S2, A, D, A2, D2, LeftClick, RightClick
    }

    private KonamiCodeInput[] konamiCode = new KonamiCodeInput[]
    {
        KonamiCodeInput.W, KonamiCodeInput.W2,
        KonamiCodeInput.S, KonamiCodeInput.S2,
        KonamiCodeInput.A, KonamiCodeInput.D,
        KonamiCodeInput.A2, KonamiCodeInput.D2,
        KonamiCodeInput.LeftClick,
        KonamiCodeInput.RightClick
    };

    // Update is called once per frame
    void Update()
    {
        if (CheckInput(konamiCode[progress]))
        {
            progress++;
            if (progress >= konamiCode.Length)
            {
                UnlockTrophy();
                progress = 0;
            }
        }
        else if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (!CheckInput(konamiCode[progress]))
            {
                progress = 0;
            }
        }
    }
    bool CheckInput(KonamiCodeInput input)
    {
        switch (input)
        {
            case KonamiCodeInput.W:
            case KonamiCodeInput.W2:
                return Input.GetKeyDown(KeyCode.W);
            case KonamiCodeInput.S:
            case KonamiCodeInput.S2:
                return Input.GetKeyDown(KeyCode.S);
            case KonamiCodeInput.A:
            case KonamiCodeInput.A2:
                return Input.GetKeyDown(KeyCode.A);
            case KonamiCodeInput.D:
            case KonamiCodeInput.D2:
                return Input.GetKeyDown(KeyCode.D);
            case KonamiCodeInput.LeftClick:
                return Input.GetMouseButtonDown(0); // chuột trái
            case KonamiCodeInput.RightClick:
                return Input.GetMouseButtonDown(1); // chuột phải
            default:
                return false;
        }
    }
    void UnlockTrophy()
    {
        var gjManager = FindObjectOfType<GameJoltManager>();
        if (gjManager != null)
        {
            gjManager.UnlockTrophy(TrophyType);
        }
    }
}
