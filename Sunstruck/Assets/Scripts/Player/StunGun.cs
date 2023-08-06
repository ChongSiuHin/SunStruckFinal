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

    // Start is called before the first frame update
    void Start()
    {
        isFire = false;
        stunEnemy = false;

        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        cameraSystemScript = FindObjectOfType<CameraSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //SpriteRenderer spriteRenderer = objectToHide.GetComponent<SpriteRenderer>();
        if (hit)
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

                    AudioManager.Instance.StunGunF();
                    AudioManager.Instance.RobotStuning();

                    //delayTimer = 1;
                }
                ammo--;
                UpdateAmmoUI(ammo);
                
            }
        }

        IEnumerator ReEnableSprite(float delay)
        {
            yield return new WaitForSeconds(delay);
            playerSpriteRenderer.enabled = true;
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

    IEnumerator UseStunGunTimer()
    {
        Enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        playerSpriteRenderer.enabled = false;

        yield return new WaitForSeconds(useDuration);
        
        if (!stunEnemy && hit)
        {   
            transform.position = GetComponent<CheckpointRespawn>().respawnPoint;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            Enemy.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            
        }

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
}
