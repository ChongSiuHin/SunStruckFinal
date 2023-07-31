using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointRespawn : MonoBehaviour
{
    [SerializeField] private GameObject deadSpace;
    [SerializeField] private GameObject[] checkpoint;

    public Vector3 respawnPoint;
    public bool isCheckPoint;
    public bool isOldMan;
    private bool activable = true;

    public DialogueTrigger dTrigger;
    public static GameObject currentTriggerObj;

    void Start()
    {
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        deadSpace.transform.position = new Vector2(transform.position.x, deadSpace.transform.position.y);
        if(isCheckPoint && Input.GetKeyDown(KeyCode.J))
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Void"))
        {
            transform.position = respawnPoint;
        }

        else if(collision.CompareTag("Checkpoint"))
        {
            isCheckPoint = true;
            dTrigger = collision.gameObject.GetComponent<DialogueTrigger>();
            currentTriggerObj = collision.gameObject;
        }
        
        else if(collision.CompareTag("OldMan") && GetComponent<InteractionSystem>().pickUpStunGun)
        {
            isOldMan = true;
            dTrigger = collision.gameObject.GetComponent<DialogueTrigger>();
            currentTriggerObj = collision.gameObject;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Checkpoint"))
        {
            isCheckPoint = false;
        }

        if(collision.CompareTag("OldMan"))
        {
            isOldMan = false;
        }
    }

    IEnumerator SmolRobot()
    {
        yield return new WaitForSeconds(1.5f);
        dTrigger.StartDialogue();
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
}
