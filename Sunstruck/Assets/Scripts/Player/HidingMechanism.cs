using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingMechanism : MonoBehaviour
{
    public bool isHiding = false;
    public static bool isHide = false;

    private bool hideAllow;
    private Rigidbody2D playerRb;
    private BoxCollider2D playerBox;
    private SpriteRenderer playerSprite;
    private Animator currentHidePointAnim;

    private Vector2 originalVelocity;

    public void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerBox = GetComponent<BoxCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (hideAllow && Input.GetKeyDown(KeyCode.J))
        {
            isHiding = true;
            isHide = true;
            HideVelocity();
            playerRb.bodyType = RigidbodyType2D.Static;
            playerBox.enabled = false;
            playerSprite.enabled = false;
            currentHidePointAnim.SetBool("IsHiding", true);
            AudioManager.Instance.Hiding();
            AudioManager.Instance.StopCurrentSound();
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            CancelHiding();
        }
    }

    public void CancelHiding()
    {
        isHiding = false;
        isHide = false;
        playerBox.enabled = true;
        playerRb.bodyType = RigidbodyType2D.Dynamic;
        playerSprite.enabled = true;
        currentHidePointAnim.SetBool("IsHiding", false);
        ShowVelocity();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hidepoint"))
        {
            hideAllow = true;
            currentHidePointAnim = collision.gameObject.GetComponent<Animator>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hidepoint"))
        {
            hideAllow = false;
        }
    }

    private void HideVelocity()
    {
        if (playerRb != null)
        {
            originalVelocity = playerRb.velocity;
            playerRb.velocity = Vector2.zero;
        }
    }

    private void ShowVelocity()
    {
        if (playerRb != null)
        {
            playerRb.velocity = originalVelocity;
        }
    }
}
