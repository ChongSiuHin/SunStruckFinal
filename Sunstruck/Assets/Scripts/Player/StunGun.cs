using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    public int ammo;
    [SerializeField] private float stunDuration;
    [SerializeField] private float useDuration;
    [SerializeField] private GameObject popUpKey;

    public bool stunEnemy;
    public static bool hit;
    public int maxHits = 6;
    public GameObject[] circles;
    public GameObject objectToHide;

    private int hitsCount = 0;
    private float stunTimer;
    private float useTimer;
    private Collider2D enemyCollider;
    private Collider2D playerCollider;
    private bool isCharging = false;
    public bool isFire;
    private bool shouldStopCharging = false;

    private Animator enemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        stunTimer = stunDuration;
        useTimer = useDuration;
        isFire = false;
        stunEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer spriteRenderer = objectToHide.GetComponent<SpriteRenderer>();
        if (hit)
        {
            //animator.SetBool("Hit", true);
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
                StartCoroutine(ReEnableSprite(2f));
            }
            useTimer -= 1 * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.J) && useTimer > 0 && !isFire)
            {
                enemyAnimator.SetBool("StunGunHit", true);
                hitsCount++;
                isFire = true;
                stunEnemy = true;
                if (stunEnemy)
                {
                    AudioManager.Instance.StunGunF();
                    AudioManager.Instance.RobotStuning();
                    Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    //delayTimer = 1;
                }
                ammo--;
                useTimer = useDuration;
                UpdateAmmoUI(ammo);
                popUpKey.SetActive(false);
            }
            else if (useTimer <= 0)
            {
                enemyAnimator.SetBool("Hit", false);
                if (!stunEnemy)
                {
                    spriteRenderer.enabled = true;
                    transform.position = this.GetComponent<CheckpointRespawn>().respawnPoint;
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
                useTimer = useDuration;
                hit = false;
                popUpKey.SetActive(false);
            }
        }

        if (stunEnemy)
        {
            stunTimer -= 1 * Time.deltaTime;
        }

        if (stunTimer <= 0)
        {
            enemyAnimator.SetBool("StunGunHit", false);
            stunEnemy = false;
            hit = false;
            stunTimer = stunDuration;
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, false);
            isFire = false;
        }

        IEnumerator ReEnableSprite(float delay)
        {
            yield return new WaitForSeconds(delay);
            spriteRenderer.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && !HidingMechanism.isHiding)
        {
            enemyCollider = collision.otherCollider;
            playerCollider = collision.collider;
            enemyAnimator = collision.gameObject.GetComponent<Animator>();
            if (GetComponent<InteractionSystem>().pickUpStunGun && ammo > 0)
            {
                hit = true;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                popUpKey.SetActive(true);
            }
            else
                transform.position = this.GetComponent<CheckpointRespawn>().respawnPoint;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ChargingStation"))
        {
            collision.gameObject.GetComponent<Animator>().SetBool("Activated", true);
            shouldStopCharging = false;
            StartCoroutine(ChargeStunGun());
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

            while (ammo < maxHits && !shouldStopCharging)
        {
            yield return new WaitForSeconds(2);

            ammo++;
            if (hitsCount > 0)
            {
                hitsCount--;
            }

            UpdateAmmoUI(ammo);
        }
        isCharging = false;
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
        }
    }
}
