using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInGame : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject SettingMenu;

    public Slider BackGroundSlider;
    public Slider masterVolumeSlider;
    public Button Return;
    // Start is called before the first frame update
    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        BackGroundSlider.value = AudioManager.Instance.backgroundMusicSource.volume;

        masterVolumeSlider.value = volume;
        Return.onClick.AddListener(ReturnToStartMenu);
    }

    // Update is called once per frame
    void Update()
    {
        AudioManager.Instance.backgroundMusicSource.volume = BackGroundSlider.value;
        SetMasterVolume(masterVolumeSlider.value);
    }

    void SetMasterVolume(float volume)
    {
        AudioManager.Instance.runSoundSource.volume = volume;
        AudioManager.Instance.robotSoundSource.volume = volume;
        AudioManager.Instance.Player.audioSource.volume = volume;
        AudioManager.Instance.ExposedSoundSource.volume = volume;

        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    void ReturnToStartMenu()
    {
        PauseMenu.SetActive(true);
        SettingMenu.SetActive(false);
    }
}
