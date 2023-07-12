using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingMechanism : MonoBehaviour
{
    public bool isHiding = false;
    public static bool isHide = false;
    public static bool isHide2 = false;
    private bool hideAllow;
    private bool hideAllow2;
    public GameObject targetObject;
    private Rigidbody2D targetRB;
    private Vector2 originalVelocity;
    public Animator anima;

    public void Start()
    {
        targetRB = targetObject.GetComponent<Rigidbody2D>();
        //Debug.Log(isHiding);
    }
    // Update is called once per frame
    void Update()
    {
        if (hideAllow && Input.GetKeyDown(KeyCode.F))
        {
            isHiding = true;
            isHide = true;
            HideVelocity();
            this.GetComponent<BoxCollider2D>().isTrigger = true;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //anima.SetBool("IsHiding", true);
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            cancelHiding();
        }

        if (hideAllow2 && Input.GetKeyDown(KeyCode.F))
        {
            isHiding = true;
            isHide2 = true;
            HideVelocity();
            this.GetComponent<BoxCollider2D>().isTrigger = true;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //anima.SetBool("IsHiding", true);
            this.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            cancelHiding();
        }
    }

    public void cancelHiding()
    {
        isHiding = false;
        isHide = false;
        isHide2 = false;
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //anima.SetBool("IsHiding", false);
        this.GetComponent<SpriteRenderer>().enabled = true;
        ShowVelocity();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hidepoint"))
        {
            hideAllow = true;
        }
        else if (collision.CompareTag("Hidepoint2"))
        {
            hideAllow2 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hidepoint"))
        {
            hideAllow = false;
        }
        else if (collision.CompareTag("Hidepoint2"))
        {
            hideAllow2 = false;
        }
    }

    private void HideVelocity()
    {
        if (targetRB != null)
        {
            //AudioManager.Instance.Hiding();
            originalVelocity = targetRB.velocity;
            targetRB.velocity = Vector2.zero;
        }
    }

    private void ShowVelocity()
    {
        if (targetRB != null)
        {
            targetRB.velocity = originalVelocity;
        }
    }
}
