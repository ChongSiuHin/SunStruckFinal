using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private VideoPlayer cutsceneVideo;
    [SerializeField] private GameObject skipButton;
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
        skipButton.SetActive(true);
        StartCoroutine(EndOfCutscene());
    }

    IEnumerator EndOfCutscene()
    {  
        yield return new WaitForSeconds(videoLength);
        cutsceneVideo.Stop();
        onCutscene = false;
        skipButton.SetActive(false);
    }

    public void SkipCutscene()
    {
        skipButton.SetActive(false);
        cutsceneVideo.Stop();
        onCutscene = false;
    }
}
