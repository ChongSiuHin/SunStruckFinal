using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    [SerializeField] private float offsetSpeed;
    [SerializeField] private float offsetManual;

    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float topBorder;
    [SerializeField] private float bottomBorder;

    private Vector3 camPos;
    private PlayerMovement playerMovement;
    private bool offsetting;
    
    // Start is called before the first frame update
    void Start()
    {
        camPos = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);
        playerMovement = FindObjectOfType<PlayerMovement>();
        offsetting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerMovement.playerRb.velocity.x > 0f)
        //{
        //    camPos = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, transform.position.z);
        //}
        //else if (playerMovement.playerRb.velocity.x < 0f)
        //{
        //    camPos = new Vector3(player.transform.position.x - offsetX, player.transform.position.y + offsetY, transform.position.z);
        //}
        //else
        //{
            if(!offsetting)
                camPos = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);

            //Offset to Right
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                camPos = new Vector3(player.transform.position.x + offsetManual, player.transform.position.y + offsetY, transform.position.z);
                offsetting = true;
            }
            else if(Input.GetKeyUp(KeyCode.RightArrow))
            {
                camPos = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);
                offsetting = false;
            }

            //Offset to Left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                camPos = new Vector3(player.transform.position.x - offsetManual, player.transform.position.y + offsetY, transform.position.z);
                offsetting = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                camPos = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);
                offsetting = false;
            }

            //Offset to Up
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                camPos = new Vector3(player.transform.position.x, player.transform.position.y + (offsetManual / 2f), transform.position.z);
                offsetting = true;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                camPos = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);
                offsetting = false;
            }

            //Offset to Down
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                camPos = new Vector3(player.transform.position.x, player.transform.position.y - (offsetManual / 2f), transform.position.z);
                offsetting = true;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                camPos = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, transform.position.z);
                offsetting = false;
            }
        //}

        if (transform.position.x < leftBorder || transform.position.x > rightBorder)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftBorder, rightBorder), player.transform.position.y + offsetY, transform.position.z);
        }
        else if (transform.position.y < bottomBorder || transform.position.y > topBorder)
        {
            transform.position = new Vector3(player.transform.position.x, Mathf.Clamp(transform.position.y, topBorder, bottomBorder), transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position, camPos, offsetSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftBorder, topBorder), new Vector2(rightBorder, topBorder));
        Gizmos.DrawLine(new Vector2(rightBorder, topBorder), new Vector2(rightBorder, bottomBorder));
        Gizmos.DrawLine(new Vector2(rightBorder, bottomBorder), new Vector2(leftBorder, bottomBorder));
        Gizmos.DrawLine(new Vector2(leftBorder, bottomBorder), new Vector2(leftBorder, topBorder));
    }
}