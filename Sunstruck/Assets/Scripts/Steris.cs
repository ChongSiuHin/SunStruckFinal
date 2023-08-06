using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steris : MonoBehaviour
{
    [SerializeField] private GameObject theoby;
    [SerializeField] private float followSpeed = 2f;
    private Vector3 velocity = Vector3.zero;
    //private float lastPosition;
    private BoxCollider2D selfCollider;
    private Animator anim;

    //float startChasingDistance = 1.0f;
    //float stopChasingDistance = 1.0f;

    void Start()
    {
        selfCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceWithTheoby = transform.localPosition.x - theoby.transform.localPosition.x;

        if (Mathf.Abs(distanceWithTheoby) > 1)
        {
            anim.SetBool("Walking", true);
            Vector3 targetPosition = new Vector3(theoby.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);

            transform.localScale = new Vector2(distanceWithTheoby > 0 ? -1 : 1, 1);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        Physics2D.IgnoreCollision(selfCollider, theoby.GetComponent<BoxCollider2D>());
    }

    //void Update()
    //{
    //    float distanceWithTheoby = transform.localPosition.x - theoby.transform.localPosition.x;

    //    if (distanceWithTheoby < -startChasingDistance && theoby.transform.localScale.x == 1)
    //    {
    //        anim.SetBool("Walking", true);
    //        transform.localScale = new Vector2(1, 1);
    //        transform.position = Vector2.MoveTowards(transform.position, theoby.transform.position, theoby.GetComponent<Rigidbody2D>().velocity.x * Time.deltaTime);
    //    }
    //    else if (distanceWithTheoby > startChasingDistance && theoby.transform.localScale.x == -1)
    //    {
    //        anim.SetBool("Walking", true);
    //        transform.localScale = new Vector2(-1, 1);
    //        transform.position = Vector2.MoveTowards(transform.position, theoby.transform.position, -theoby.GetComponent<Rigidbody2D>().velocity.x * Time.deltaTime);
    //    }
    //    else if (distanceWithTheoby > -stopChasingDistance && distanceWithTheoby < stopChasingDistance)
    //    {
    //            anim.SetBool("Walking", false);
    //    }

    //    Physics2D.IgnoreCollision(selfCollider, theoby.GetComponent<BoxCollider2D>());
    //}    
}
