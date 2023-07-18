using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBox : MonoBehaviour
{
    public bool beingMove;
    float xPos;
    
    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (beingMove == false)
        {
            transform.position = new Vector3(xPos, transform.position.y);
        }
        else
        {
            xPos = transform.position.x;
        }       
    }
}
