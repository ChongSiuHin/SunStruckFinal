using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingSpot : MonoBehaviour
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
        checkHide = FindObjectOfType<HidingMechanism>();

        if (playerCheck)
        {
            if (HidingMechanism.isHide)
            {
                if (!hasPlayedAudio)
                {
                    AudioManager.Instance.Hiding();
                    hasPlayedAudio = true;
                }
                //AudioManager.Instance.Hiding();
                anima.SetBool("IsHiding", true);
                Debug.Log("Player hiding");
                //gameObject.GetComponent<SpriteRenderer>().sprite = nSprite;
            }
            else
            {
                anima.SetBool("IsHiding", false);
                Debug.Log("Player show");
                hasPlayedAudio = false;
                //gameObject.GetComponent<SpriteRenderer>().sprite = oriSprite;
            }
            //Debug.Log(HidingMechanism.isHide);
        }
        Debug.Log(playerCheck);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //playerCheck = false;
        }
    }
}
