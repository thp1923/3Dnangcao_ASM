using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;
public class AudioSettings : MonoBehaviour
{
    public Slider masterSlider, musicSlider, sfxSlider;
    public AudioMixer audioMixer;

    // PlayerPrefs keys
    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    void Start()
    {
        // Load saved values or default to 1.0f
        masterSlider.value = PlayerPrefs.GetFloat(MasterKey, 1.0f);
        musicSlider.value = PlayerPrefs.GetFloat(MusicKey, 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFXKey, 1.0f);

        // Apply loaded values to mixer
        SetVolume("MasterVolume", masterSlider.value);
        SetVolume("MusicVolume", musicSlider.value);
        SetVolume("SFXVolume", sfxSlider.value);

        // Add listeners to sliders
        masterSlider.onValueChanged.AddListener(val => { SetVolume("MasterVolume", val); SaveVolume(MasterKey, val); });
        musicSlider.onValueChanged.AddListener(val => { SetVolume("MusicVolume", val); SaveVolume(MusicKey, val); });
        sfxSlider.onValueChanged.AddListener(val => { SetVolume("SFXVolume", val); SaveVolume(SFXKey, val); });
    }

    void SetVolume(string parameter, float sliderValue)
    {
        // Convert linear [0,1] to decibel [-80, 0]
        float dB = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameter, dB);
    }

    void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    // Optional: For a "Reset to Default" button
    public void ResetAudioSettings()
    {
        masterSlider.value = 1.0f;
        musicSlider.value = 1.0f;
        sfxSlider.value = 1.0f;
    }
}
