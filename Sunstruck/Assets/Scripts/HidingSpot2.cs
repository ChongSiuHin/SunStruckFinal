using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot2 : MonoBehaviour
{
    [SerializeField] private Sprite nSprite;
    private HidingMechanism checkHide;
    private bool playerCheck = false;
    public Animator anima;
    private bool hasPlayedAudio = false;

    // Update is called once per frame
    void Update()
    {
        if (playerCheck)
        {
            //checkHide = FindObjectOfType<HidingMechanism>();

            if (HidingMechanism.isHide2)
            {
                if (!hasPlayedAudio)
                {
                    AudioManager.Instance.Hiding();
                    hasPlayedAudio = true;
                }
                anima.SetBool("IsHiding1", true);
            }
            else
            {
                anima.SetBool("IsHiding1", false);
                hasPlayedAudio = false;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //playerCheck = false;
        }
    }
}

