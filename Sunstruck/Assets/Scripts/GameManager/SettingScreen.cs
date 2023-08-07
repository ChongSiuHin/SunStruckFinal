using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreen : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject Level;
    public GameObject Setting;

    public Slider BackGroundSlider;
    public Slider masterVolumeSlider;
    public Button Return;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("BackGroundVolume", 1.0f);
        float volume1 = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        BackGroundSlider.value = AudioManager.Instance.backgroundMusicSource.volume;
        BackGroundSlider.value = volume1;
        masterVolumeSlider.value = volume;
        Return.onClick.AddListener(ReturnToStartMenu);
    }

    void Update()
    {
        SetBackGroundVolume(BackGroundSlider.value);
        SetMasterVolume(masterVolumeSlider.value);
    }

    void SetBackGroundVolume(float volume1)
    {
        AudioManager.Instance.backgroundMusicSource.volume = BackGroundSlider.value;
        PlayerPrefs.SetFloat("BackGroundVolume", volume1);
        PlayerPrefs.Save();
    }
    void SetMasterVolume(float volume)
    {
        AudioManager.Instance.runSoundSource.volume = volume;
        AudioManager.Instance.robotSoundSource.volume = volume;
        //AudioManager.Instance.Player.audioSource.volume = volume;
        AudioManager.Instance.ExposedSoundSource.volume = volume;

        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
    void ReturnToStartMenu()
    {
        StartMenu.SetActive(true);
        Level.SetActive(false);
        Setting.SetActive(false);
    }
}
