using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    #region Weather Timer
    public enum Weather { Sunny, Rainy, Foggy, Cloudy };
    public Weather currentWeather;
    public float weatherChangeInterval = 20f;
    #endregion
    public GameObject rainy;
    // Start is called before the first frame update
    void Start()
    {
        currentWeather = Weather.Rainy;
        InvokeRepeating("ChangeWeather", weatherChangeInterval, weatherChangeInterval);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        #region RainLateUpdate
        if (rainy.activeSelf)
        {
            rainy.transform.position = Camera.main.transform.position;
            rainy.transform.forward = Camera.main.transform.forward;
            ParticleSystem[] rainSystems = rainy.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem rain in rainSystems)
            {
                rain.Play();
            }
        }
        #endregion
    }
    private void ChangeWeather()
    {
        currentWeather = (Weather)Random.Range(0, System.Enum.GetValues(typeof(Weather)).Length);
        UpdateWeatherEffects();
    }
    private void UpdateWeatherEffects()
    {
        #region Rain
        rainy.SetActive(currentWeather == Weather.Rainy);
        AudioSource rainAudio = rainy.GetComponentInChildren<AudioSource>();
        if (rainAudio != null)
        {
            if (currentWeather == Weather.Rainy)
            {
                rainAudio.Play();
            }
            else
            {
                rainAudio.Stop();
            }
        }
        #endregion

        #region Fog
        if (currentWeather == Weather.Foggy)
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = 0.09f;
            RenderSettings.fogColor = Color.gray;
        }
        else
        {
            RenderSettings.fog = false;
        }
        #endregion
    }
}
