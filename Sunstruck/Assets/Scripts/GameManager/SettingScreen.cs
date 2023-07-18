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
    public Slider WalkingSlider;
    public Slider RobotSlider;
    public Button Return;

    // Start is called before the first frame update
    void Start()
    {
        BackGroundSlider.value = AudioManager.Instance.backgroundMusicSource.volume;
        WalkingSlider.value = AudioManager.Instance.runSoundSource.volume;
        RobotSlider.value = AudioManager.Instance.robotSoundSource.volume;
        Return.onClick.AddListener(ReturnToStartMenu);
    }

    // Update is called once per frame
    void Update()
    {
        AudioManager.Instance.backgroundMusicSource.volume = BackGroundSlider.value;
        AudioManager.Instance.runSoundSource.volume = WalkingSlider.value;
        AudioManager.Instance.robotSoundSource.volume = RobotSlider.value;
        // add other AudioSource references as needed
    }

    void ReturnToStartMenu()
    {
        StartMenu.SetActive(true);
        Level.SetActive(false);
        Setting.SetActive(false);
    }
}
