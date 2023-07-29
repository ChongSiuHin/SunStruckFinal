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
    private Sentence sentenceToDisplay;

    public static bool isActive = false;
    int activeSentence = 0;

    void Start()
    {
        dialogueText.text = string.Empty;
    }

    private void FixedUpdate()
    {
        //    //Triggering next sentence
        if (Input.anyKeyDown)
        {
            if (dialogueText.text == sentenceToDisplay.sentence)
            {
                NextSentence();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = sentenceToDisplay.sentence;
            }
        }
    }

    public void OpenDialogue(Sentence[] sentences, Actor[] actors)
    {
        anim.SetBool("IsOpen", true);
        currentSentences = sentences;
        currentActors = actors;
        activeSentence = 0;
        isActive = true;
        background.SetActive(true);

        DisplaySentence();
    }

    void DisplaySentence()
    {
        sentenceToDisplay = currentSentences[activeSentence];
        StartCoroutine(TypeSentence());

        Actor actorToDisplay = currentActors[sentenceToDisplay.actorId];
        nameText.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
    }

    public void NextSentence()
    {
        activeSentence++;
        if(activeSentence < currentSentences.Length)
        {
            //dialogueText.text = string.Empty;
            DisplaySentence();
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeSentence()
    {
        dialogueText.text = "";
        foreach (char letter in sentenceToDisplay.sentence.ToCharArray())
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
    } 
}
