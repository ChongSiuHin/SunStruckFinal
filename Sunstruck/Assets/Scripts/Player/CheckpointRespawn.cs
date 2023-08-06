using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointRespawn : MonoBehaviour
{
    [SerializeField] private GameObject popUpKey;
    [SerializeField] private GameObject deadSpace;
    public GameObject[] checkpoint;

    public Vector3 respawnPoint;
    public bool isCheckPoint;
    public bool isOldMan;
    private bool activable = true;
    public static bool isDead;

    public DialogueTrigger dTrigger;
    public static GameObject currentTriggerObj;
    private GameObject TutorialStunGun;

    void Start()
    {
        respawnPoint = transform.position;
        if(SceneManager.GetActiveScene().name == "BackStreet")
        {
            TutorialStunGun = GameObject.Find("Tutorial Stun Gun");
            TutorialStunGun.SetActive(false);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        deadSpace.transform.position = new Vector2(transform.position.x, deadSpace.transform.position.y);
        if(isCheckPoint && Input.GetKeyDown(KeyCode.J) && !DialogueManager.isActive)
        {
            respawnPoint = transform.position;
            currentTriggerObj.GetComponent<Animator>().SetTrigger("Activate");
            if (activable)
            {
                StartCoroutine(SmolRobot());
                activable = false;
                AudioManager.Instance.RespawnPoint();
            }
            else
            {
                dTrigger.StartDialogue();
            }    
        }

        if (isOldMan && Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(OldMan());
        }

        if (SceneManager.GetActiveScene().name == "SurfaceWorld")
        {
            if (currentTriggerObj == checkpoint[0])
            {
                //start cutscene
            }
            if(currentTriggerObj == checkpoint[4])
            {
                currentTriggerObj.GetComponent<Animator>().SetTrigger("Deactivate");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Void"))
        {
            transform.position = respawnPoint;
            StartCoroutine(DeadBool());
        }

        else if(collision.CompareTag("Checkpoint"))
        {
            popUpKey.SetActive(true);
            isCheckPoint = true;
            
            currentTriggerObj = collision.gameObject;
            dTrigger = currentTriggerObj.GetComponent<DialogueTrigger>();
        }
        
        else if(collision.CompareTag("OldMan") && GetComponent<InteractionSystem>().pickUpStunGun)
        {
            popUpKey.SetActive(true);
            isOldMan = true;
            
            currentTriggerObj = collision.gameObject;
            dTrigger = currentTriggerObj.GetComponent<DialogueTrigger>();
        }
        else if(collision.CompareTag("ExposeArea") && !InteractionSystem.pickUpSuit)
        {
            transform.position = respawnPoint;
            StartCoroutine(DeadBool());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Checkpoint"))
        {
            popUpKey.SetActive(false);
            isCheckPoint = false;
        }

        if(collision.CompareTag("OldMan"))
        {
            popUpKey.SetActive(false);
            isOldMan = false;
        }
    }

    IEnumerator SmolRobot()
    {
        yield return new WaitForSeconds(1.5f);
        dTrigger.StartDialogue();
        while (DialogueManager.isActive)
        {
            yield return null;
        }
        TutorialStunGun.SetActive(true);
    }

    IEnumerator OldMan()
    {
        dTrigger.StartDialogue();
        while (DialogueManager.isActive)
        {
            yield return null;
        }
        if(SceneManager.GetActiveScene().name != "AbandonedCargoArea")
        {
            SceneController.instance.NextLevel();
            respawnPoint = transform.position;
        }  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("ChaseEnemy"))
        {
            transform.position = respawnPoint;
            StartCoroutine(DeadBool());
        }
    }

    IEnumerator DeadBool()
    {
        isDead = true;
        yield return null;
        isDead = false;
    }
}
