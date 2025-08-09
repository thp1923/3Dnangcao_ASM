using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI;
    bool isShopOpen = false;
    [Header("Các mục trong shop")]
    public GameObject VuKhi;
    public GameObject AoGiap;
    public GameObject VatPham;



    void Start()
    {
        shopUI.SetActive(false);
        VuKhi.SetActive(false);
        AoGiap.SetActive(false);
        VatPham.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isShopOpen)
            CloseShop();

        if (isShopOpen == true)
        {
            FindFirstObjectByType<NPC>().TogglePlayerScripts(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (isShopOpen == false)
        {
            //FindFirstObjectByType<NPC>().TogglePlayerScripts(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Openshop()
    {
        shopUI.SetActive(true);
        VuKhi.SetActive(true);
        AoGiap.SetActive(false);
        VatPham.SetActive(false);

        isShopOpen = true;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);

        isShopOpen = false;
    }

    public void OpenVuKhi()
    {
        VuKhi.SetActive(true);
        AoGiap.SetActive(false);
        VatPham.SetActive(false);
    }

    public void OpenAoGiap()
    {
        VuKhi.SetActive(false);
        AoGiap.SetActive(true);
        VatPham.SetActive(false);
    }

    public void OpenVatPham()
    {
        VuKhi.SetActive(false);
        AoGiap.SetActive(false);
        VatPham.SetActive(true);
    }
}
