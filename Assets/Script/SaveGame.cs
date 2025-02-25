using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    GameObject player;

    public static bool haveSave;

    // Mã hóa float thành Base64
    private string EncodeFloat(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }


    // Gi?i mã Base64 thành float
    private float DecodeFloat(string base64Value, float defaultValue = 0.0f)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(base64Value);
            return BitConverter.ToSingle(bytes, 0);
        }
        catch (Exception)
        {
            return defaultValue; // Tr? v? giá tr? m?c ??nh n?u không th? gi?i mã
        }
    }

    private void Awake()
    {

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (!haveSave) return;
        // ??c t?a ?? t? PlayerPrefs và gi?i mã Base64
        float x = DecodeFloat(PlayerPrefs.GetString("x", EncodeFloat(0.0f)), 0.0f);
        float y = DecodeFloat(PlayerPrefs.GetString("y", EncodeFloat(0.0f)), 0.0f);
        float z = DecodeFloat(PlayerPrefs.GetString("z", EncodeFloat(0.0f)), 0.0f);

        // ??t v? trí cho player
        Vector3 vector = new Vector3(x, y, z);
        player.transform.position = vector;
        Debug.Log("?ã load: " + vector);
    }


    void Update()
    {

    }

    public void Save()
    {
        Vector3 vector = player.transform.position;


        // Mã hóa t?a ?? tr??c khi l?u
        PlayerPrefs.SetString("x", EncodeFloat(vector.x));
        PlayerPrefs.SetString("y", EncodeFloat(vector.y));
        PlayerPrefs.SetString("z", EncodeFloat(vector.z));


        PlayerPrefs.Save();
        haveSave = true;
        Debug.Log("?ã l?u: " + vector);
    }

}
