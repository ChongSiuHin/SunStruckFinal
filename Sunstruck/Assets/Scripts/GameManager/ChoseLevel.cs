using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoseLevel : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject Level;
    public GameObject Setting;

    public Button Level1;
    public Button Level2;
    public Button Level3;
    public Button Return;
    // Start is called before the first frame update
    void Start()
    {
        L1();
        L2();
        L3();
        Return.onClick.AddListener(ReturnToStartMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAnotherScreen(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void L1()
    {
        Level1.onClick.AddListener(() => LoadAnotherScreen("FrontStreet"));
    }

    public void L2()
    {
        Level2.onClick.AddListener(() => LoadAnotherScreen("AbandonedCargoArea"));
    }

    public void L3()
    {
        //Level3.onClick.AddListener(() => LoadAnotherScreen("AbandonedCargoArea"));
    }

    void ReturnToStartMenu()
    {
        StartMenu.SetActive(true);
        Level.SetActive(false);
        Setting.SetActive(false);
    }
}
