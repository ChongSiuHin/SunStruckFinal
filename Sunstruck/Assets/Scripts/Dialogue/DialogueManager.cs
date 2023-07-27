using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject background;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public float textSpeed;
    public bool DialogueOn;

    private GameObject dialogueBox;
    private Animator anim;
    private bool Checkpoint;
    private bool OldMan;
    public Dialogue dialogue;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = string.Empty;
        DialogueOn = false;
    }

    // Update is called once per frameZ
    void Update()
    {
        Checkpoint = FindObjectOfType<CheckpointRespawn>().isCheckPoint;
        if(Checkpoint)
        {
            dialogueBox = GameObject.FindGameObjectWithTag("DialogueCheckpoint");
        }

        OldMan = FindObjectOfType<CheckpointRespawn>().isOldMan;
        if (OldMan)
        {
            dialogueBox = GameObject.FindGameObjectWithTag("DialogueOldMan");
        }

        anim = dialogueBox.GetComponentInChildren<Animator>();

        //Triggering next sentence
        if (Input.anyKeyDown)
        {
            if(dialogueText.text == dialogue.sentences[index])
            {
                DisplayNextSentence();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogue.sentences[index];
            }  
        }

        nameText.text = dialogue.name;
    }

    public void StartDialogue()
    {
        DialogueOn = true;
        nameText.text = dialogue.name;

        if (Checkpoint)
        {
            anim.SetBool("IsOpenSmol", true);
        }
        else if (OldMan)
        {
            anim.SetBool("IsOpenOldMan", true);
        }
        
        background.SetActive(true);
        index = 0;

        StartCoroutine(TypeSentence());
    }

    public void DisplayNextSentence()
    {
        if(index < dialogue.sentences.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeSentence());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence()
    {
        dialogueText.text = "";
        foreach(char letter in dialogue.sentences[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndDialogue()
    {
        if (Checkpoint)
        {
            anim.SetBool("IsOpenSmol", false);
        }
        else if (OldMan)
        {
            anim.SetBool("IsOpenOldMan", false);
        }

        background.SetActive(false);

        DialogueOn = false;
    }
}
