using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float climbSpeed;
    [SerializeField] private LayerMask groundLayer;

    private float horizontal;
    private float verticle;
    private bool isClimbing;
    private bool isLadder;
    private bool isRunning = false;
    public bool Platform;

    public Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    public Animator anima;
    private HidingMechanism hide;
    private InteractionSystem interactionSystem;
    private bool PKJump;
    private Transform playerTrans;
    private GameObject currentTriggerObj;

    private bool canJumpFromClimbable = false;

    private void Awake()
    {
        interactionSystem = GetComponent<InteractionSystem>();
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        hide = GetComponent<HidingMechanism>();
        playerTrans = GetComponent<Transform>();
        playerRb.gravityScale = 3f;
    }
    private void Update()
    {
        PKJump = interactionSystem.PKJump;
        horizontal = Input.GetAxis("Horizontal");
        if(hide.isHiding)
        {

        }
        else
        {
            walk();
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            AudioManager.Instance.CLimbingRope();
            playerRb.velocity = new Vector2(playerRb.velocity.x, verticle * climbSpeed);
        }
        else
        {
            playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
        }
        playerRb.gravityScale = 3f;
    }

    private bool isGrounded()
    {
        RaycastHit2D hitGround = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return hitGround.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climable"))
        {
            isLadder = true;
            isClimbing = true;
        }     
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climable"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Platform = true;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            Platform = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isClimbing)
        {
            playerTrans.position = new Vector3(currentTriggerObj.transform.position.x, playerTrans.position.y, playerTrans.position.z);
        }
    }

    private void walk()
    {
        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);

        anima.SetFloat("speed", Mathf.Abs(horizontal));

        if (horizontal != 0f)
        {
            if (!isRunning)
            {
                isRunning = true;
                if (Platform)
                {
                    AudioManager.Instance.WalkInPlatform();
                }
                else
                {
                    AudioManager.Instance.PlayRunSound();
                }
            }

            if (horizontal > 0f)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else
            {
                transform.localScale = new Vector2(-1, 1);
            }
            //AudioManager.Instance.StopCurrentSound();
        }
        else
        {
            if (isRunning)
            {
                Debug.Log("WasStop");
                isRunning = false;
                AudioManager.Instance.StopCurrentSound();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && PKJump)
        {
            isClimbing = false;
            AudioManager.Instance.PlayJumpSound();
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        }
        else if(Input.GetKeyDown(KeyCode.Space) && isLadder)
        {
            isClimbing = false;
            AudioManager.Instance.PlayJumpSound();
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        }
        verticle = Input.GetAxis("Vertical");

        //if (isLadder && (verticle > 0f || verticle < 0f))
        //{
        //    isClimbing = true;
        //}
        //else if (isLadder)
        //{
        //    isClimbing = true;
        //}
    }
}
