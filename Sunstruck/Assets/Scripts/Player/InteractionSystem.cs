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
    public bool pickUpStunGun;
    public bool pickUpSuit;
    private GameObject box;
    private BoxCollider2D playerBox;
    private UIController uiController;
    public bool PKJump = true;

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
            Debug.LogError("UIManager object not found in the scene!");
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
                box = hitbox.collider.gameObject;

                box.GetComponent<FixedJoint2D>().enabled = true;
                box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
                box.GetComponent<StaticBox>().beingMove = true;
                this.GetComponent<PlayerMovement>().speed /= 2f;
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                PKJump = true;
                box.GetComponent<FixedJoint2D>().enabled = false;
                box.GetComponent<StaticBox>().beingMove = false;
                this.GetComponent<PlayerMovement>().speed = 3f;
            }
        }
        

        if (hititem.collider != null && Input.GetKeyDown(KeyCode.F))
        {
            pickUp(hititem.collider.gameObject);
        }

        if(pickUpStunGun)
        {
            uiController.ShowUI();
        }
    }
    
    public void pickUp(GameObject obj)
    {
        if (obj.tag == "StunGun")
        {
            print("StunGun Picked Up");
            pickUpStunGun = true;
            stunGun.GetComponent<Animator>().SetBool("stunGunPickUp", true);
            SceneController.instance.Cutscene();
            AudioManager.Instance.StunGunP();
            
        }
        else if(obj.tag == "Suit")
        {
            print("Suit Picked Up");
            pickUpSuit = true;
            Destroy(obj);
            //AudioManager.Instance.suit();
        }
        else
        {
            pickUpStunGun = false;
            pickUpSuit = false;
        }

        if (obj.tag == "NextScene")
        {
            SceneController.instance.NextLevel();
            GetComponent<CheckpointRespawn>().respawnPoint = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,(Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}
