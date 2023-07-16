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
    public bool isTalking;

    public Animator anim;
    public Dialogue dialogue;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = string.Empty;
        isTalking = true;
    }

    // Update is called once per frameZ
    void Update()
    {
        //Triggering next sentence
        if(Input.anyKeyDown)
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
    }

    public void StartDialogue()
    {
        anim.SetBool("IsOpen", true);
        background.SetActive(true);
        index = 0;

        nameText.text = dialogue.name;

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
        anim.SetBool("IsOpen", false);
        background.SetActive(false);
        isTalking = false;
    }
}
