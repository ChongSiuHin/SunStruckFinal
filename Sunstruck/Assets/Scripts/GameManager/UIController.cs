using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    void Start()
    {
        HideUI();
    }

    public void HideUI()
    {
        Debug.Log("UI was hidden");
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        Debug.Log("UI was shown");
        gameObject.SetActive(true);
    }
}
