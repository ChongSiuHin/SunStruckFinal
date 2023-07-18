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
                anima.SetBool("IsHiding", true);
            }
            else
            {
                anima.SetBool("IsHiding", false);
                Debug.Log("Player show");
                hasPlayedAudio = false;
            }
        }
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
