using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : MonoBehaviour
{
    [SerializeField] private int ammo;
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

    // Start is called before the first frame update
    void Start()
    {
        stunTimer = stunDuration;
        useTimer = useDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            useTimer -= 1 * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.F) && useTimer > 0)
            {
                hitsCount++;
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

                if (hitsCount <= maxHits)
                {
                    circles[hitsCount - 1].SetActive(false);
                }
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
            StartCoroutine(ChargeStunGun());
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ChargingStation") && !isCharging && ammo < maxHits)
        {
            StartCoroutine(ChargeStunGun());
        }
    }

    IEnumerator ChargeStunGun()
    {
        isCharging = true;

            while (ammo < maxHits)
        {
            yield return new WaitForSeconds(2);

            ammo++;

            UpdateAmmoUI(ammo);
        }
        isCharging = false;
    }

    void UpdateAmmoUI(int ammo)
    {
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].SetActive(i < ammo);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ChargingStation"))
        {
            StopCoroutine(ChargeStunGun());
        }
    }
}
