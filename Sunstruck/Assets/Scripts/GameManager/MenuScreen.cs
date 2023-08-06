using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject Level;
    public GameObject Setting;

    public Button StartButton;
    public Button chooseLevelButton;
    public Button settingButton;

    private void Start()
    {
        StartMenu.SetActive(true);
        Level.SetActive(false);
        Setting.SetActive(false);

        Start1();
        chooseLevelButton.onClick.AddListener(OpenLevelMenu);
        settingButton.onClick.AddListener(OpenSettingMenu);
    }

    private void Update()
    {
        //Start1();
        //chooseLevelButton.onClick.AddListener(OpenLevelMenu);
        //settingButton.onClick.AddListener(OpenSettingMenu);
    }

    public void Start1()
    {
        StartButton.onClick.AddListener(() => LoadAnotherScreen("FrontStreet"));
    }

    public void LoadAnotherScreen(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        StartMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        StartMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("FrontStreet");
    }

    public void OpenSetting()
    {
        Setting.SetActive(true);
    }

    public void CloseSetting()
    {
        Setting.SetActive(false);
    }

    void OpenLevelMenu()
    {
        StartMenu.SetActive(false);
        Level.SetActive(true);
        Setting.SetActive(false);
    }

    void OpenSettingMenu()
    {
        StartMenu.SetActive(false);
        Level.SetActive(false);
        Setting.SetActive(true);
    }
}
