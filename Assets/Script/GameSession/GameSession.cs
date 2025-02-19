using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    [Header("Slider")]
    public Slider HpBar;
    public Slider delayHpBar;

    public int HpMax;

    int Hp;
    int HpDelay;

    [Header("Time")]
    public float timeDelayHp;
    float _timeDelayHp;

    [Header("-----------Inven----------")]
    public GameObject InventoryCanva;
    
    void Awake()
    {
        // so luong doi tuong GameSession
        int numbersession = FindObjectsOfType<GameSession>().Length;
        //neu no co nhieu hon phien ban thi se huy no
        if (numbersession > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject); //khong cho huy khi load
        Instance = this;
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
            delayHpBar.value -= 3;
            HpDelay -= 3;
            _timeDelayHp = timeDelayHp;
        }
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        HpBar.value = Hp;
        if (Hp <= 0)
        {
            FindObjectOfType<PlayerTakeDamge>().Death();
        }
    }

    public void Heal(int healBonous)
    {
        Hp += healBonous;
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
}
