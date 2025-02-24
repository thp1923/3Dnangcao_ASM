using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    

    [Header("Slider")]
    public Slider HpBar;
    public Slider delayHpBar;

    public int HpMax;

    int Hp;
    int HpDelay;

    [Header("Time")]
    public float timeDelayHp;
    float _timeDelayHp;
    int HpLost;

    [Header("-----------Inven----------")]
    public GameObject InventoryCanva;

    [Header("-----------Death----------")]
    public GameObject canvaDie;
    public GameObject canvaStart;
    
    void Awake()
    {
        // so luong doi tuong GameSession
        int numbersession = FindObjectsOfType<GameSession>().Length;
        //neu no co nhieu hon phien ban thi se huy no
        if (numbersession > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject); //khong cho huy khi load
    }
    // Start is called before the first frame update
    void Start()
    {
        Hp = HpMax;
        HpDelay = Hp;
        HpBar.maxValue = HpMax;
        HpBar.value = HpMax;
        delayHpBar.maxValue = HpMax;
        delayHpBar.value = HpMax;
    }

    // Update is called once per frame
    void Update()
    {
        Inventory();
        _timeDelayHp -= Time.deltaTime;
        if(Hp < HpDelay && _timeDelayHp <= 0)
        {
            delayHpBar.value -= (int)(HpLost/(float)4);
            HpDelay -= (int)(HpLost / (float)4);
            _timeDelayHp = timeDelayHp;
        }
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        HpBar.value = Hp;
        HpLost = damage;
        if (Hp <= 0)
        {
            FindObjectOfType<PlayerTakeDamge>().Death();
            canvaDie.SetActive(true);
        }
    }

    public void Heal(float healBonous)
    {
        Hp += (int)(healBonous*(float)HpMax);
        HpBar.value = Hp;
        if (Hp > HpMax)
            Hp = HpMax;
    }

    public void Inventory()
    {
        if (InventoryCanva.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        else{
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (InventoryCanva.activeInHierarchy)
            {
                InventoryCanva.SetActive(false);
                LockMouse.CursorLocked = true;
            }
            else
            {
                InventoryCanva.SetActive(true);
                LockMouse.CursorLocked = false;
            }
        }
    }
    public void LockMouseInGamesession(int number)
    {
        if(number == 0)
            LockMouse.CursorLocked = false;
        else
            LockMouse.CursorLocked = true;
    }

    public void Canva(int index)
    {
        switch (index)
        {
            case 0:
                canvaStart.SetActive(true); 
                canvaDie.SetActive(false);
                PlayAgain();
                break;
            case 1:
                canvaStart.SetActive(false);
                canvaDie.SetActive(true);
                break;
            case 2:
                canvaStart.SetActive(false);
                canvaDie.SetActive(false);
                break;
            case 3:
                canvaStart.SetActive(false);
                break;
            case 4:
                canvaDie.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);//load lai Scene 0
        Time.timeScale = 1;
    }
    public void PlayAgain()
    {
        //lay index cua scene hien tai
        int currentsceneindex = SceneManager.GetActiveScene().buildIndex;
        //load lai scene hien tai

        SceneManager.LoadScene(currentsceneindex);
        Time.timeScale = 1;
        //Destroy(gameObject); //destroy GameSession luon
        Hp = HpMax;
        HpDelay = Hp;
        HpBar.value = Hp; 
        delayHpBar.value = Hp;
    }
}
