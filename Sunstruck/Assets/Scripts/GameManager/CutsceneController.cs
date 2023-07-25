using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EndCutScene());
    }

    IEnumerator EndCutScene()
    {

        yield return new WaitForSeconds(10);
        SceneManager.UnloadSceneAsync("Cutscene2");
    }
}
