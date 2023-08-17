using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask movableObj;
    [SerializeField] private LayerMask interactableObj;
    [SerializeField] private GameObject stunGun;
    [SerializeField] private GameObject castPoint;
    [SerializeField] private GameObject chaseEnemy;
    [SerializeField] private GameObject enemySpawnPoint;
    [SerializeField] private GameObject enemySpawnPoint2;
    [SerializeField] private GameObject popUpKey;

    public bool pickUpStunGun = false;
    public static bool pickUpSuit = false;
    private GameObject box;
    private BoxCollider2D playerBox;
    private UIController uiController;
    private StunGun stunGunScript;
    public bool PKJump = true;
    private Animator anima;

    private bool switchAllow;
    private bool isSwitchedOn;
    private Animator currentObjAnim;
    private CameraSystem cameraSystemScript;
    public HealthBar healthBar;

    public static bool isBox;
    public GameObject light2d;
    public static GameObject EnemyCrate;
    public static GameObject EnemyCrate1;
    public static GameObject Door;

    public GameObject audioManager;

    // Start is called before the first frame update
    void Start()
    {
        PKJump = true;
        playerBox = GetComponent<BoxCollider2D>();
        stunGunScript = GetComponent<StunGun>();
        cameraSystemScript = FindObjectOfType<CameraSystem>();
        anima = GetComponent<Animator>();

        GameObject uiManagerObj = GameObject.Find("UIManager");
        if (uiManagerObj != null)
        {
            uiController = uiManagerObj.GetComponent<UIController>();
        }
        else
        {
            Debug.Log("UIManager object not found in the scene!");
        }

        if(SceneManager.GetActiveScene().name == "SurfaceWorld")
        {
            EnemyCrate = GameObject.Find("EnemyCrate");
            //EnemyCrate1 = GameObject.Find("EnemyCrate (1)");
            Door = GameObject.Find("Door");
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitbox = Physics2D.Raycast(castPoint.transform.position, Vector2.right * transform.localScale.x, distance, movableObj);
        RaycastHit2D hititem = Physics2D.BoxCast(playerBox.bounds.center, playerBox.size, 0, Vector2.zero, 0, interactableObj);
        
        if(hitbox.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                PKJump = false;
                
                box = hitbox.collider.gameObject;
                anima.SetBool("Push", true);
                box.GetComponent<FixedJoint2D>().enabled = true;
                box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                box.GetComponent<StaticBox>().beingMove = true;
                this.GetComponent<PlayerMovement>().speed /= 2f;

                isBox = true;

                AudioManager.Instance.PushBox();
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                PKJump = true;

                anima.SetBool("Push", false);
                box.GetComponent<FixedJoint2D>().enabled = false;
                box.GetComponent<StaticBox>().beingMove = false;
                this.GetComponent<PlayerMovement>().speed = 3f;

                isBox = false;

                AudioManager.Instance.StopCurrentSound();
            }
        }  

        if (hititem.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                PickUp(hititem.collider.gameObject);
            }
        }

        if(pickUpStunGun)
        {
            uiController.ShowUI();
        }

        if(switchAllow && Input.GetKeyDown(KeyCode.J))
        {
            if (!isSwitchedOn)
            {
                anima.SetBool("Switch", true);
                AudioManager.Instance.drop();
                stunGunScript.UpdateAmmoUI(--stunGunScript.ammo);
                cameraSystemScript.SwitchOnCargo();
                currentObjAnim.enabled = true;
                isSwitchedOn = true;
                StartCoroutine(SetSwitchToFalse());
            }
        }
    }
    
    public void PickUp(GameObject obj)
    {
        if (obj.CompareTag("StunGun"))
        {
            if (!pickUpStunGun)
            {
                //obj.GetComponent<CutsceneTrigger>().PlayCutscene();
                StartCoroutine(StunGunDialogue(obj));
                AudioManager.Instance.StunGunP();
            }
            pickUpStunGun = true;
            stunGun.GetComponent<Animator>().SetBool("stunGunPickUp", true); 
        }
        
        if(obj.CompareTag("Suit"))
        {
            CheckpointRespawn.currentTriggerObj = obj;
            StartCoroutine(ActiveSuit());
            StartCoroutine(SuitDialogue(obj));
        }

        if (obj.CompareTag("NextScene"))
        {
            if(SceneManager.GetActiveScene().name == "FrontStreet")
            {
                obj.GetComponent<CutsceneTrigger>().PlayCutscene();
                StartCoroutine(StartingCutscene());
            }
            else
            {
                SceneController.instance.NextLevel();
                GetComponent<CheckpointRespawn>().respawnPoint = transform.position;
            }

        }

        if (obj.CompareTag("Pistol"))
        {
            EnemyCrate.SetActive(false);
            Door.SetActive(false);
            Instantiate(chaseEnemy, enemySpawnPoint.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Switches"))
        {
            switchAllow = true;
            currentObjAnim = collision.gameObject.GetComponent<Animator>();
            popUpKey.SetActive(true);
        }

        if (collision.CompareTag("SpawnChaseEnemy"))
        {
            EnemyCrate1.SetActive(false);
            Instantiate(chaseEnemy, enemySpawnPoint2.transform.position, Quaternion.identity);
        }

        if(collision.CompareTag("StunGun") || collision.CompareTag("Suit") || collision.CompareTag("NextScene") || collision.CompareTag("Pistol"))
        {
            popUpKey.SetActive(true);
        }

        if (collision.CompareTag("Laboratory"))
        {
            cameraSystemScript.ZoomOutLaboratory();
            StartCoroutine(EndingCutscene(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Switches"))
        {
            switchAllow = false;
            popUpKey.SetActive(false);
        }

        if (collision.CompareTag("StunGun") || collision.CompareTag("Suit") || collision.CompareTag("NextScene") || collision.CompareTag("Pistol"))
        {
            popUpKey.SetActive(false);
        }
    }

    IEnumerator StunGunDialogue(GameObject obj)
    {
        while (CutsceneTrigger.onCutscene)
        {
            yield return null;
        }
        CheckpointRespawn.currentTriggerObj = obj;
        obj.GetComponent<DialogueTrigger>().StartDialogue();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(castPoint.transform.position, (Vector2)castPoint.transform.position + Vector2.right * transform.localScale.x * distance);
    }

    IEnumerator SuitDialogue(GameObject obj)
    {
        obj.GetComponent<DialogueTrigger>().StartDialogue();
        while (DialogueManager.isActive)
        {
            yield return null;
        }
        pickUpSuit = true;
        light2d.SetActive(true);
        healthBar.gameObject.SetActive(true);
        AudioManager.Instance.Suit();
    }

    IEnumerator StartingCutscene()
    {
        while (CutsceneTrigger.onCutscene)
        {
            yield return null;
        }
        SceneController.instance.NextLevel();
        GetComponent<CheckpointRespawn>().respawnPoint = transform.position;
    }

    IEnumerator SetSwitchToFalse()
    {
        yield return new WaitForSeconds(1f);
        anima.SetBool("Switch", false);
    }

    IEnumerator ActiveSuit()
    {
        anima.SetBool("SuitActivate", true);
        yield return new WaitForSeconds(1f);
        anima.SetBool("SuitActivate", false);
    }

    IEnumerator EndingCutscene(GameObject labo)
    {
        yield return new WaitForSeconds(6);
        labo.GetComponent<CutsceneTrigger>().PlayCutscene();
        audioManager.SetActive(false);
        while (CutsceneTrigger.onCutscene)
        {
            yield return null;
        }
        audioManager.SetActive(true);
        CameraSystem.onCam = false;
        SceneController.instance.NextLevel();
    }
}