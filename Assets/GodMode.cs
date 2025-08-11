using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodMode : MonoBehaviour
{
    public GameObject godCanva;
    public KeyCode godKey = KeyCode.F12;

    public int godModeDamge = 1000000000;
    LockController lockController;
    // Start is called before the first frame update
    void Start()
    {
        godCanva.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CanvaGod();
    }

    void CanvaGod()
    {
        if (Input.GetKeyDown(godKey))
        {
            lockController = FindObjectOfType<LockController>();
            if (godCanva.activeSelf && !lockController.isInven && !lockController.isQuest)
            {
                godCanva.SetActive(false);
                var attackCtrl = FindObjectOfType<PlayerAttackController>();
                attackCtrl.LockController(true);
            }
            else
            {
                godCanva.SetActive(true);
                lockController.OutPlayerController();
            }
        }
    }

    public void GodChange()
    {
        var attack = GameObject.FindWithTag("Player").GetComponent<AttackDamgePlayer>();
        var takeDamge = GameObject.FindWithTag("Player").GetComponent<PlayerTakeDamge>();
        attack.godDamge = godModeDamge;
        takeDamge.GodMode = true;
    }

    public void BackScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index - 1);
    }

    public void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }
}
