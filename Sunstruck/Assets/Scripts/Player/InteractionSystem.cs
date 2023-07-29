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
    
    public bool pickUpStunGun = false;
    public bool pickUpSuit = false;
    private GameObject box;
    private BoxCollider2D playerBox;
    private UIController uiController;
    public bool PKJump = true;

    private bool switchAllow;
    private bool isSwitchedOn;
    public bool offset;

    // Start is called before the first frame update
    void Start()
    {
        PKJump = true;
        playerBox = GetComponent<BoxCollider2D>();
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
        RaycastHit2D hitbox = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, movableObj);
        RaycastHit2D hititem = Physics2D.BoxCast(playerBox.bounds.center, playerBox.size, 0, Vector2.zero, 0, interactableObj);
        
        if(hitbox.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                PKJump = false;
                AudioManager.Instance.PushBox();
                box = hitbox.collider.gameObject;

                box.GetComponent<FixedJoint2D>().enabled = true;
                box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                box.GetComponent<StaticBox>().beingMove = true;
                this.GetComponent<PlayerMovement>().speed /= 2f;
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                PKJump = true;
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
                FindObjectOfType<StunGun>().UpdateAmmoUI(--FindObjectOfType<StunGun>().ammo);
                FindObjectOfType<CameraSystem>().SwitchOnCargo();
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
        }

        if (collision.CompareTag("Offset"))
        {
            offset = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Switches"))
        {
            switchAllow = false;
        }

        if (collision.CompareTag("Offset"))
        {
            offset = false;
        }
    }

    IEnumerator StunGunDialogue(GameObject obj)
    {
        yield return new WaitForSeconds(10.5f);
        obj.GetComponent<DialogueTrigger>().StartDialogue();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,(Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}