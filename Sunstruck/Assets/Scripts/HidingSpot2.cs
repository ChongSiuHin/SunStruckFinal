using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot2 : MonoBehaviour
{
    [SerializeField] private Sprite nSprite;
    private Sprite oriSprite;
    private HidingMechanism checkHide;
    private bool playerCheck = false;
    public Animator anima;
    private bool hasPlayedAudio = false;

    private void Start()
    {
        //oriSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

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
                //Debug.Log("Player hiding");
                //gameObject.GetComponent<SpriteRenderer>().sprite = nSprite;
            }
            else
            {
                anima.SetBool("IsHiding1", false);
                //Debug.Log("Player show");
                hasPlayedAudio = false;
                //gameObject.GetComponent<SpriteRenderer>().sprite = oriSprite;
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

