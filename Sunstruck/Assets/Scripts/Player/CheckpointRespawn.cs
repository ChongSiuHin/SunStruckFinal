using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRespawn : MonoBehaviour
{
    [SerializeField] private GameObject deadSpace;
    [SerializeField] private GameObject[] checkpoint;

    public Vector3 respawnPoint;
    public bool isCheckPoint;
    public bool isOldMan;
    private bool activable;
    private int i = 0;

    // Start is called before the first frame update
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
                FindObjectOfType<DialogueManager>().StartDialogue();
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
        }
        
        else if(collision.CompareTag("OldMan") && GetComponent<InteractionSystem>().pickUpStunGun)
        {
            isOldMan = true;
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
        FindObjectOfType<DialogueManager>().StartDialogue();
    }

    IEnumerator OldMan()
    {
        FindObjectOfType<DialogueManager>().StartDialogue();
        while (FindObjectOfType<DialogueManager>().DialogueOn)
        {
            yield return null;
        }
        SceneController.instance.NextLevel();
        respawnPoint = transform.position;
    }
}
