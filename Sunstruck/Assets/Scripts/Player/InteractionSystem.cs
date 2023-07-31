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
    
    public bool pickUpStunGun = false;
    public bool pickUpSuit = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitbox = Physics2D.Raycast(castPoint.transform.position, Vector2.right * castPoint.transform.localScale.x, distance, movableObj);
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

                AudioManager.Instance.PushBox();
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                PKJump = true;
                anima.SetBool("Push", false);
                box.GetComponent<FixedJoint2D>().enabled = false;
                box.GetComponent<StaticBox>().beingMove = false;
                this.GetComponent<PlayerMovement>().speed = 3f;
            }
        }
        

        if (hititem.collider != null && Input.GetKeyDown(KeyCode.J))
        {
            PickUp(hititem.collider.gameObject);
        }

        if(pickUpStunGun)
        {
            uiController.ShowUI();
        }

        if(switchAllow && Input.GetKeyDown(KeyCode.J))
        {
            if (!isSwitchedOn)
            {
                stunGunScript.UpdateAmmoUI(--stunGunScript.ammo);
                cameraSystemScript.SwitchOnCargo();
                currentObjAnim.enabled = true;
                isSwitchedOn = true;
            }
        }
    }
    
    public void PickUp(GameObject obj)
    {
        if (obj.tag == "StunGun")
        {
            if (!pickUpStunGun)
            {
                SceneController.instance.Cutscene();
                StartCoroutine(StunGunDialogue(obj));
                AudioManager.Instance.StunGunP();
            }
            pickUpStunGun = true;
            stunGun.GetComponent<Animator>().SetBool("stunGunPickUp", true); 
        }
        
        if(obj.tag == "Suit")
        {
            print("Suit Picked Up");
            pickUpSuit = true;
            Destroy(obj);
        }

        if (obj.tag == "NextScene")
        {
            SceneController.instance.NextLevel();
            GetComponent<CheckpointRespawn>().respawnPoint = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Switches"))
        {
            switchAllow = true;
            currentObjAnim = collision.gameObject.GetComponent<Animator>();
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Switches"))
        {
            switchAllow = false;
        }

        
    }

    IEnumerator StunGunDialogue(GameObject obj)
    {
        yield return new WaitForSeconds(10.5f);
        CheckpointRespawn.currentTriggerObj = obj;
        obj.GetComponent<DialogueTrigger>().StartDialogue();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(castPoint.transform.position, (Vector2)castPoint.transform.position + Vector2.right * castPoint.transform.localScale.x * distance);
    }
}