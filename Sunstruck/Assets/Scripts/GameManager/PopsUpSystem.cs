using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopsUpSystem : MonoBehaviour
{
    public Animator popsUpAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
