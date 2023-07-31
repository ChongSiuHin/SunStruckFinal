using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueScreen : MonoBehaviour
{
    public GameObject pauseMenu;
    private static bool isPaused;
    public GameObject settingMenu;
    public AudioSource backgroundMusic;
    public AudioSource WalkingMusic;

    private void Start()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("FrontStreet");
    }

    public void OpenSetting()
    {
        settingMenu.SetActive(true);
    }

    public void CloseSetting()
    {
        settingMenu.SetActive(false);
    }

    public void ToggleBackgroundMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
        else
        {
            backgroundMusic.Play();
        }
    }

    public void ToggleWalkingMusic()
    {
        if (WalkingMusic.isPlaying)
        {
            WalkingMusic.Stop();
        }
        else
        {
            WalkingMusic.Play();
        }
    }
}
