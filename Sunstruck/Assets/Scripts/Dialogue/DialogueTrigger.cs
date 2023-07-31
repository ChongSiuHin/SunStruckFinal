using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Sentence[] sentences;
    public Actor[] actors;
    public bool hasIdleSentence;
    public bool isRepeat = false;
    public IdleSentence[] idleSentences;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(sentences, actors, idleSentences, isRepeat);
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

[System.Serializable]
public class IdleSentence
{
    public int actorId;
    public string idleSentence;
}
