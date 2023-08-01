using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasing : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private GameObject targetPlayer;

    private Animator selfAnim;

    private void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        selfAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPlayer.transform.position, moveSpeed * Time.deltaTime);

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject == targetPlayer)
        {
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Box"))
        {
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Void"))
        {
            Destroy(gameObject);
        }
    }
}
