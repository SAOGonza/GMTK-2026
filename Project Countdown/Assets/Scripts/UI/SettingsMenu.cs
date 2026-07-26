using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider VolumeSlider;
    public Toggle BloomToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("Volume")) VolumeSlider.value = PlayerPrefs.GetFloat("Volume");
        if (PlayerPrefs.HasKey("Bloom")) BloomToggle.isOn = PlayerPrefs.GetInt("Bloom") == 1;
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        //print("Set volume to " + volume);
    }

    public void UpdateVolume()
    {
        SetVolume(VolumeSlider.value);
    }

    public void SetBloom(bool bloom)
    {
        PlayerPrefs.SetInt("Bloom", bloom ? 1 : 0);
        //print("Set bloom to " + bloom);
    }

    public void UpdateBloom()
    {
        SetBloom(BloomToggle.isOn);
    }

    public void SaveSettings()
    {
        print("Set volume to " + PlayerPrefs.GetFloat("Volume"));
        print("Set bloom to " + PlayerPrefs.GetInt("Bloom"));
        PlayerPrefs.Save();
        if (PlayerPrefs.HasKey("Volume")) AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        PostProcessingSetting.Instance.ApplySettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
