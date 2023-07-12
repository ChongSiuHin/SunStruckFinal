using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerUseItems : MonoBehaviour
{
    public event Action OnItemDestroyed;
    public HealthBar healthBar;

    private bool pickUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pickUp && Input.GetKeyDown(KeyCode.F))
        {
            PickUp();
            OnDestroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("Player"))
        {
            pickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            pickUp = false;
        }
    }

    private void PickUp()
    {
        OnItemDestroyed?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        healthBar.gameObject.SetActive(true);
    }
}
