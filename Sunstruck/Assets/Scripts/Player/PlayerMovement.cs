using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
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
    private InteractionSystem interactionSystem;
    private bool PKJump;
    private Transform playerTrans;
    private GameObject currentTriggerObj;

    private bool isJumping = false;
    public static bool offset;
    public static bool inRoom;

    private void Awake()
    {
        interactionSystem = GetComponent<InteractionSystem>();
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerTrans = GetComponent<Transform>();
        playerRb.gravityScale = 3f;
    }
    private void Update()
    {
        if (DialogueManager.isActive || CameraSystem.onCam || CutsceneTrigger.onCutscene)
        {
            playerRb.bodyType = RigidbodyType2D.Static;
            return;
        }
        else if(!(HidingMechanism.isHiding || StunGun.hit))
        {
            playerRb.bodyType = RigidbodyType2D.Dynamic;
        }

        PKJump = interactionSystem.PKJump;
        horizontal = Input.GetAxis("Horizontal");
        if(HidingMechanism.isHiding)
        {

        }
        else
        {
            walk();
        }
    }

    private void FixedUpdate()
    {
        if (isLadder)
        {
            if (Mathf.Abs(verticle) != 0f)
            {
                isClimbing = true;
                anima.SetBool("Climbing", false);
                anima.SetBool("RopeToGround", true);
                anima.SetBool("RopeJump", false);
                anima.SetBool("ClimbMove", true);
                playerRb.velocity = new Vector2(playerRb.velocity.x, verticle * climbSpeed);
                playerRb.gravityScale = 0f;
                Physics2D.IgnoreLayerCollision(10, 3, true);
                AudioManager.Instance.CLimbingRope();
            }
            else
            {
                if (!isJumping)
                {
                    isClimbing = true;
                    anima.SetBool("Climbing", true);
                    playerRb.gravityScale = 0f;
                    playerRb.velocity = new Vector2(horizontal * speed, verticle * climbSpeed);
                    Physics2D.IgnoreLayerCollision(10, 3, true);
                    //AudioManager.Instance.StopCurrentSound();
                }
            }
        }
        else
        {
            playerRb.gravityScale = 3f;
            playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
            Physics2D.IgnoreLayerCollision(10, 3, false);
        }

        if (isGrounded())
        {
            isJumping = false;
            anima.SetBool("Jump", false);
            anima.SetBool("RopeToGround", false);
            anima.SetBool("ClimbMove", false);
        }
    }

    //private void FixedUpdate()
    //{
    //    if (isClimbing)
    //    {
    //        anima.SetBool("Climbing", true);
    //        playerRb.velocity = new Vector2(playerRb.velocity.x, verticle * climbSpeed);
    //        playerRb.gravityScale = 0f;
    //        AudioManager.Instance.CLimbingRope();
    //    }
    //    else
    //    {
    //        playerRb.gravityScale = 3f;
    //        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
    //    }

    //    if (isGrounded())
    //    {
    //        isJumping = false;
    //        anima.SetBool("Jump", false);
    //    }

    //}

    private bool isGrounded()
    {
        RaycastHit2D hitGround = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, Vector2.down, 0.02f, groundLayer);
        return hitGround.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Climable"))
        {
            anima.SetBool("RopeJump", false);
            currentTriggerObj = collision.gameObject;
            isLadder = true;
            isClimbing = true;
        }

        if (collision.CompareTag("Offset"))
        {
            offset = true;
        }

        if (collision.CompareTag("Room"))
        {
            inRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Climable"))
        {
            isLadder = false;
            //isClimbing = false;
        }

        if (collision.CompareTag("Offset"))
        {
            offset = false;
        }

        if (collision.CompareTag("Room"))
        {
            inRoom = false;
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
        if (isClimbing && isLadder && !isGrounded())
        {
            isJumping = false;
            playerTrans.position = new Vector3(currentTriggerObj.transform.position.x, playerTrans.position.y, playerTrans.position.z);
        }
    }

    private void walk()
    {
        if (!isJumping)
        {
            playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);
            anima.SetFloat("speed", Mathf.Abs(horizontal));
        }

        if (horizontal != 0f)
        {
            if (!isRunning)
            {
                isRunning = true;
                if (Platform)
                {
                    //AudioManager.Instance.WalkInPlatform();
                }
                else
                {
                    //AudioManager.Instance.PlayRunSound();
                }
            }

            if(!InteractionSystem.isBox)
            {
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

        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
                AudioManager.Instance.StopPlayerSound();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && PKJump)
        {
            AudioManager.Instance.StopPlayerSound();
            isJumping = true;
            isClimbing = false;
            anima.SetBool("Jump", true);
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            AudioManager.Instance.PlayJumpSound();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && isLadder && (Input.GetAxis("Horizontal") != 0))
        {
            AudioManager.Instance.StopPlayerSound();
            isJumping = true;
            isClimbing = false;
            anima.SetBool("RopeJump", true);
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            AudioManager.Instance.PlayJumpSound();
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
