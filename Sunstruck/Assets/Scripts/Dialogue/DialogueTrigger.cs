using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Sentence[] sentences;
    public Actor[] actors;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(sentences, actors);
    }
}

[System.Serializable]
public class Sentence
{
    public int actorId;
    public string sentence;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}
