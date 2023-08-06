using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private VideoPlayer cutsceneVideo;
    private float videoLength;
    public static bool onCutscene;


    private void Start()
    {
        videoLength = (float)cutsceneVideo.length;
    }

    public void PlayCutscene()
    {
        cutsceneVideo.Play();
        onCutscene = true;
        StartCoroutine(EndOfCutscene());
    }

    IEnumerator EndOfCutscene()
    {  
        yield return new WaitForSeconds(videoLength);
        cutsceneVideo.enabled = false;
        onCutscene = false;
    }
}
