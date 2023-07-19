using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    [SerializeField] public int ammo;
    [SerializeField] private float stunDuration;
    [SerializeField] private float useDuration;

    public bool stunEnemy;
    public bool hit;
    public int maxHits = 3;
    public GameObject[] circles;

    private int hitsCount = 0;
    private float stunTimer;
    private float useTimer;
    private Collider2D enemyCollider;
    private Collider2D playerCollider;
    private bool isCharging = false;
    public bool isFire;
    private bool shouldStopCharging = false;

    // Start is called before the first frame update
    void Start()
    {
        stunTimer = stunDuration;
        useTimer = useDuration;
        isFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            useTimer -= 1 * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.J) && useTimer > 0 && !isFire)
            {
                hitsCount++;
                isFire = true;
                Debug.Log("Stun Enemy");
                stunEnemy = true;
                if (stunEnemy)
                {
                    AudioManager.Instance.StunGunF();
                    AudioManager.Instance.RobotStuning();
                    Debug.Log("stunning");
                    Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
                ammo--;
                useTimer = useDuration;
                UpdateAmmoUI(ammo);
            }
            else if (useTimer <= 0)
            {
                if(!stunEnemy)
                {
                    transform.position = this.GetComponent<CheckpointRespawn>().respawnPoint;
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                }
                useTimer = useDuration;
                hit = false;
            }
        }

        if (stunEnemy)
        {
            stunTimer -= 1 * Time.deltaTime;
        }

        if(stunTimer <= 0)
        {
            stunEnemy = false;
            hit = false;
            stunTimer = stunDuration;
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, false);
            isFire = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && !GetComponent<HidingMechanism>().isHiding)
        {
            enemyCollider = collision.otherCollider;
            playerCollider = collision.collider;
            if (GetComponent<InteractionSystem>().pickUpStunGun && ammo > 0)
            {
                hit = true;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;                
            }
            else
                transform.position = this.GetComponent<CheckpointRespawn>().respawnPoint;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ChargingStation"))
        {
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
            shouldStopCharging = true;
        }
    }
}
