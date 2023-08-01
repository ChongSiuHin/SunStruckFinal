using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetected1 : MonoBehaviour
{
    //private bool isHealthBarActive = false;
    private HealthBar healthBar;
    private bool isPlayerInside;
    private void Start()
    {
        isPlayerInside = false;
    }

    private void Update()
    {
        if (isPlayerInside && healthBar != null)
        {
            healthBar.TakeDamage();
            healthBar.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            healthBar = FindObjectOfType<HealthBar>();
            healthBar.TakeDamage();
            healthBar.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && healthBar != null)
        {
            //healthBar.TakeDamage();
            //healthBar.SetActive(true);
            isPlayerInside = true;
            AudioManager.Instance.exposed();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.StopCurrentSound();
            isPlayerInside = false;
        }
    }
}
