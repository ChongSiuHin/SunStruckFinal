using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopsUpSystem : MonoBehaviour
{
    public Animator popsUpAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            popsUpAnim.SetBool("PlayerAround", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            popsUpAnim.SetBool("PlayerAround", false);
        }
    }
}
