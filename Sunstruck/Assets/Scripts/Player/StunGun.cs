using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    public int ammo;
    [SerializeField] private float stunDuration;
    [SerializeField] private float useDuration;
    [SerializeField] private GameObject popUpKey;

    public static bool stunEnemy;
    public static bool hit;
    public int maxHits = 6;
    public GameObject[] circles;
    //public GameObject objectToHide;
    private SpriteRenderer playerSpriteRenderer;

    private int hitsCount = 0;
    private Collider2D enemyCollider;
    private Collider2D playerCollider;
    private bool isCharging = false;
    public bool isFire;
    private bool shouldStopCharging = false;

    private GameObject Enemy;
    private CameraSystem cameraSystemScript;
    public static bool canMove;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        isFire = false;
        stunEnemy = false;

        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        cameraSystemScript = FindObjectOfType<CameraSystem>();
        canMove = true;
    }

    void Update()
    {
        if (hit && !CameraSystem.onCam)
        {

            StartCoroutine(UseStunGunTimer());
            cameraSystemScript.CaptureByEnemy();

            if (Input.GetKeyDown(KeyCode.J) && !isFire)
            {
                StartCoroutine(ReEnableSprite(1f));
                popUpKey.SetActive(false);
                Enemy.GetComponent<Animator>().SetBool("StunGunHit", true);
                Enemy.GetComponent<Animator>().SetBool("Hit", false);

                hitsCount++;
                isFire = true;
                stunEnemy = true;

                if (stunEnemy)
                {
                    StartCoroutine(StunEnemyTimer());
                    Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    GetComponent<Collider2D>().isTrigger = false;

                    AudioManager.Instance.StunGunF();
                    AudioManager.Instance.RobotStuning();

                    //delayTimer = 1;
                }
                ammo--;
                UpdateAmmoUI(ammo);
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && !HidingMechanism.isHiding)
        {
            playerCollider = collision.otherCollider;
            enemyCollider = collision.collider;
            Enemy = collision.gameObject;
            if (GetComponent<InteractionSystem>().pickUpStunGun && ammo > 0)
            {
                hit = true;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                popUpKey.SetActive(true);
            }
            else
                StartCoroutine(Dying());
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ChargingStation"))
        {
            collision.gameObject.GetComponent<Animator>().SetBool("Activated", true);
            if (ammo != maxHits)
            {
                shouldStopCharging = false;
                canMove = false;
                playerSpriteRenderer.enabled = false;
                StartCoroutine(ChargeStunGun());
                if (ChangeSprite.changeSuit)
                {
                    collision.gameObject.GetComponent<Animator>().SetBool("Charging2", true);
                }
                else
                {
                    collision.gameObject.GetComponent<Animator>().SetBool("Charging", true);
                }
            }
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ChargingStation") && !isCharging && ammo < maxHits)
        {
            shouldStopCharging = false;
            StartCoroutine(ChargeStunGun());
        }
    }

    IEnumerator ChargeStunGun()
    {
        isCharging = true;
        AudioManager.Instance.ChargingStation();
        while (ammo < maxHits && !shouldStopCharging)
        {

            ammo++;
            if (hitsCount > 0)
            {
                hitsCount--;
            }
            yield return new WaitForSeconds(2);
            UpdateAmmoUI(ammo);
        }
        isCharging = false;
        StartCoroutine(ReEnableSprite(2f));
        canMove = true;
    }

    public void UpdateAmmoUI(int ammo)
    {
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].SetActive(i < ammo);
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ChargingStation"))
        {
            collision.gameObject.GetComponent<Animator>().SetBool("Activated", false);
            shouldStopCharging = true;
            if (ammo >= maxHits)
            {
                if (ChangeSprite.changeSuit)
                {
                    collision.gameObject.GetComponent<Animator>().SetBool("Charging2", false);
                }
                else
                {
                    collision.gameObject.GetComponent<Animator>().SetBool("Charging", false);
                } 
            }
        }
    }

    IEnumerator UseStunGunTimer()
    {
        Enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        playerSpriteRenderer.enabled = false;
        GetComponent<Collider2D>().isTrigger = true;


        yield return new WaitForSeconds(useDuration);
        
        if (!stunEnemy && hit)
        {
            StartCoroutine(Dying());
            //transform.position = GetComponent<CheckpointRespawn>().respawnPoint;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            Enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            
        }

        GetComponent<Collider2D>().isTrigger = false;
        Enemy.GetComponent<Animator>().SetBool("Hit", false);
        playerSpriteRenderer.enabled = true;

        hit = false;
        popUpKey.SetActive(false);
    }

    IEnumerator StunEnemyTimer()
    {
        hit = false;
        isFire = false;

        yield return new WaitForSeconds(stunDuration);

        Enemy.GetComponent<Animator>().SetBool("StunGunHit", false);
        Enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        stunEnemy = false;

        Physics2D.IgnoreCollision(enemyCollider, playerCollider, false);
    }

    IEnumerator ReEnableSprite(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerSpriteRenderer.enabled = true;
    }

    IEnumerator Dying()
    {
        animator.SetBool("Died", true);
        yield return new WaitForSeconds(1f);
        transform.position = this.GetComponent<CheckpointRespawn>().respawnPoint;
        animator.SetBool("Died", false);
    }
}
