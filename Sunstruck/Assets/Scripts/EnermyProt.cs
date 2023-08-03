using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyProt : MonoBehaviour
{
    private StunGun stungun;

    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    public Animator anima;
    private Transform currentPoint;
    public Transform castPoint;
    public float speed;
    public GameObject player;
    private Vector2 endPos;
    public Transform Enemy;

    public Transform playerTransform;
    public string currentState;
    public float agroRange;
    public bool isPlayerSeek;
    public bool isFacingLeft = false;
    private bool playSound;
    private bool isPausing = false;

    public bool hitPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        currentPoint = pointB.transform;
        currentState = "walk";
        anima.SetBool("Run", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingLeft)
        {
            endPos = Enemy.position + (Vector3.left * agroRange);
        }
        else
        {
            endPos = Enemy.position + (Vector3.right * agroRange);
        }

        if (StunGun.hit && hitPlayer)
        {
            Debug.Log("hit parameter");
            //anima.SetBool("Run", false);
            anima.SetBool("Hit", true);         
            if (StunGun.stunEnemy)
            {
                playSound = false;
                //rb.velocity = new Vector2(0, 0);
                //Animation
            }  
        }
        else
        {
            playSound = true;
            if (CanSeekPlayer(agroRange))
            {
                anima.SetBool("Run", true);
                chasing();
            }
            else
            {
                if (!isPausing)
                {
                    anima.SetBool("Scanning", false);
                    anima.SetBool("Run", true);
                    walkAround();
                }
                else if (isPausing)
                {
                    anima.SetBool("Scanning", true);
                }
            }
        }

    }


    bool CanSeekPlayer(float distance)
    {
        bool val = false;
        float castDisk = distance;

        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            else
            {
                val = false;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.yellow);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPos, Color.blue);
        }
        return val;

    }

    public void chasing()
    {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            else if (transform.position.x < playerTransform.position.x)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        //}
    }

    public void walkAround()
    {
        if (isPausing) return;

        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
            isFacingLeft = false;
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
            isFacingLeft = true;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f & currentPoint == pointB.transform)
            {
                //anima.SetBool("Scanning", true);
                StartCoroutine(PauseBeforeMoving());
                currentPoint = pointA.transform;
                if (playSound)
                {
                    AudioManager.Instance.RobotMoving();
                }
            }
            else if(Vector2.Distance(transform.position, currentPoint.position) < 0.5f & currentPoint == pointA.transform)
            {
                //anima.SetBool("Scanning", true);
                StartCoroutine(PauseBeforeMoving());
                currentPoint = pointB.transform;
                if (playSound)
                {
                    AudioManager.Instance.RobotMoving();
                }
            }
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        //Debug.Log("flip1");
        localScale.x *= -1;
        //Debug.Log("flip2");
        transform.localScale = localScale;
        //Debug.Log("flip3");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider);
        }

        if (collision.gameObject.tag == "Player")
        {
            hitPlayer = true;
        }
        else
        {
            hitPlayer = false;
        }
    }

    IEnumerator PauseBeforeMoving()
    {
        isPausing = true;
        rb.velocity = new Vector2(0, 0);
        //anima.SetBool("Scanning", true);
        yield return new WaitForSeconds(4f);
        isPausing = false;
        flip();
    }
}
