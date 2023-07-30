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
    private int i = 0;

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
            checkpoint[i].GetComponent<Animator>().SetTrigger("Activate");
            i++;
            if (activable)
            {
                AudioManager.Instance.RespawnPoint();
                StartCoroutine(SmolRobot());
                activable = false;
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
        Debug.Log("Start");
        dTrigger.StartDialogue();
    }

    IEnumerator OldMan()
    {
        dTrigger.StartDialogue();
        while (DialogueManager.isActive)
        {
            yield return null;
        }
        SceneController.instance.NextLevel();
        respawnPoint = transform.position;
    }
}
