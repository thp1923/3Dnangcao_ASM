using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider masterSlider, musicSlider, sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        masterSlider.onValueChanged.AddListener(val =>
        {
            int volume = Mathf.RoundToInt(val);
            SetVolume("Master", volume);
        });
        musicSlider.onValueChanged.AddListener(val =>
        {
            int volume = Mathf.RoundToInt(val);
            SetVolume("Music", volume);
        });
        sfxSlider.onValueChanged.AddListener(val =>
        {
            int volume = Mathf.RoundToInt(val);
            SetVolume("SFX", volume);
        });
    }

    void SetVolume(string type, int value)
    {
        Debug.Log($"{type} volume = {value}");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
