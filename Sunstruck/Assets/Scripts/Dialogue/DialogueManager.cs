using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject background;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public float textSpeed;

    public Animator anim;
    public Image actorImage;

    Sentence[] currentSentences;
    Actor[] currentActors;
    IdleSentence[] currentIdleSentences;
    //private Sentence sentenceToDisplay;

    public static bool isActive = false;
    public static bool isRepeat = false;
    int activeSentence = 0;

    void Start()
    {
        dialogueText.text = string.Empty;
    }

    private void Update()
    {
        //    //Triggering next sentence
        if (Input.anyKeyDown)
        {
            if (isRepeat)
            {
                if (dialogueText.text == currentIdleSentences[activeSentence].idleSentence)
                {
                    NextIdleSentence();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = currentIdleSentences[activeSentence].idleSentence;
                }
            }
            else
            {
                if (dialogueText.text == currentSentences[activeSentence].sentence)
                {
                    NextSentence();
                }
                else
                {
                    StopAllCoroutines();
                    dialogueText.text = currentSentences[activeSentence].sentence;
                }
            }
        }
        
    }

    public void OpenDialogue(Sentence[] sentences, Actor[] actors, IdleSentence[] idleSentences)
    {
        anim.SetBool("IsOpen", true);
        isActive = true;
        background.SetActive(true);
        activeSentence = 0;
        if (!isRepeat)
        {
            currentSentences = sentences;
            currentActors = actors;

            DisplaySentence();
        }
        else
        {
            currentIdleSentences = idleSentences;
            currentActors = actors;

            DisplayIdleSentence();
        }
       
    }

    void DisplaySentence()
    {
        Sentence sentenceToDisplay = currentSentences[activeSentence];
        StartCoroutine(TypeSentence(sentenceToDisplay.sentence));

        Actor actorToDisplay = currentActors[sentenceToDisplay.actorId];
        nameText.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    void DisplayIdleSentence()
    {
        IdleSentence IdleSentenceToDisplay = currentIdleSentences[activeSentence];
        StartCoroutine(TypeSentence(IdleSentenceToDisplay.idleSentence));

        Actor actorToDisplay = currentActors[IdleSentenceToDisplay.actorId];
        nameText.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextSentence()
    {
        activeSentence++;
        if (activeSentence < currentSentences.Length)
        {
            //dialogueText.text = string.Empty;
            DisplaySentence();
        }
        else
        {
            EndDialogue();
        }
    }

    public void NextIdleSentence()
    {
        activeSentence++;
        if (activeSentence < currentIdleSentences.Length)
        {
            //dialogueText.text = string.Empty;
            DisplayIdleSentence();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence(string sentenceToDisplay)
    {
        dialogueText.text = "";
        foreach (char letter in sentenceToDisplay.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void EndDialogue()
    {
        anim.SetBool("IsOpen", false);

        background.SetActive(false);

        isActive = false;

        if (FindObjectOfType<DialogueTrigger>().hasIdleSentence)
        {
            isRepeat = true;
        }
    } 
}
