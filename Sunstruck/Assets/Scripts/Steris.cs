using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steris : MonoBehaviour
{
    [SerializeField] private GameObject theoby;
    private float lastPosition;
    private BoxCollider2D selfCollider;
    private Animator anim;

    void Start()
    {
        selfCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceWithTheoby = transform.localPosition.x - theoby.transform.localPosition.x;

        if (distanceWithTheoby < -1 && theoby.transform.localScale.x == 1)
        {
            anim.SetBool("Walking", true);
            transform.localScale = new Vector2(1, 1);
            transform.position = Vector2.MoveTowards(transform.position, theoby.transform.position, theoby.GetComponent<Rigidbody2D>().velocity.x * Time.deltaTime);
        }
        else if (distanceWithTheoby > 1 && theoby.transform.localScale.x == -1)
        {
            anim.SetBool("Walking", true);
            transform.localScale = new Vector2(-1, 1);
            transform.position = Vector2.MoveTowards(transform.position, theoby.transform.position, -theoby.GetComponent<Rigidbody2D>().velocity.x * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        Physics2D.IgnoreCollision(selfCollider, theoby.GetComponent<BoxCollider2D>());
    }    
}
