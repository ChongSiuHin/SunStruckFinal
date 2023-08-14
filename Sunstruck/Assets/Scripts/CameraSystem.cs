using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private GameObject cutsceneCam;
    private GameObject roomCam;
    private GameObject switchCam;
    private GameObject cargoCam;
    private GameObject LaboCam;
    [SerializeField] private GameObject player;
    private GameObject OldMan;
    [SerializeField] private Animator cargoAnim;
    [SerializeField] private Animator craneAnim;
    private float hitZoomIn;
    private float offsetY;

    private float shakeIntensity = 3f;
    private float shakeTime = 1f;
    private float timer;
    private bool isClue;

    private CinemachineBasicMultiChannelPerlin _cbmcp;

    public static bool onCam;

    private GameObject tutorialCharge;
    private GameObject tutorialRopJump;

    private void Start()
    {
        isClue = false;

        if (SceneManager.GetActiveScene().name == "AbandonedCargoArea")
        {
            switchCam = GameObject.Find("SwitchCam");
            cargoCam = GameObject.Find("CargoCam");
            OldMan = GameObject.FindGameObjectWithTag("OldMan");
            tutorialCharge = GameObject.Find("Tutorial Charge");
            tutorialRopJump = GameObject.Find("Tutorial Rope Jump");
            OldMan.GetComponent<DialogueTrigger>().StartDialogue();
            StartCoroutine(PreviewLevelACA());
        }

        if (SceneManager.GetActiveScene().name == "SurfaceWorld")
        {
            switchCam = GameObject.Find("SwitchCam");
            cargoCam = GameObject.Find("CargoCam");
            roomCam = GameObject.Find("RoomCam");
            LaboCam = GameObject.Find("LaboratoryCam");
        }

        StopShake();
    }

    private void Update()
    {
        CaptureByEnemy();

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StopShake();
            }
        }

        ViewEnemyBelow();

        if (SceneManager.GetActiveScene().name == "AbandonedCargoArea")
        {
            if (CheckpointRespawn.currentTriggerObj.CompareTag("Checkpoint") && Input.GetKeyDown(KeyCode.J) && !isClue)
            {
                StartCoroutine(viewCargoAfterCheckpoint());
            }
        }

        if (SceneManager.GetActiveScene().name == "SurfaceWorld")
        {
            FollowPlayerOnTrigger();

            if (CheckpointRespawn.currentTriggerObj == player.GetComponent<CheckpointRespawn>().checkpoint[3] && Input.GetKeyDown(KeyCode.J) && !isClue)
            {
                StartCoroutine(viewCargoAfterCheckpoint());
            }
        }
    }

    public void CaptureByEnemy()
    {
        if (StunGun.hit)
        {
            hitZoomIn -= 1f;
            ShakeCamera();
        }
        else
        {
            hitZoomIn += 1f;
            StopShake();
        }

        hitZoomIn = Mathf.Clamp(hitZoomIn, 1.5f, 3f);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, hitZoomIn, Time.deltaTime * zoomSpeed);
    }

    public void SwitchOnCargo()
    {
        StartCoroutine(DropCargo());
    }

    IEnumerator DropCargo()
    {
        yield return new WaitForSeconds(1);
        cargoCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        cinemachineVirtualCamera.enabled = false;
        yield return new WaitForSeconds(2);
        craneAnim.enabled = true;
        yield return new WaitForSeconds(0.83f);
        cargoAnim.enabled = true;
        ShakeCamera();
        yield return new WaitForSeconds(1.5f);
        cinemachineVirtualCamera.enabled = true;
        cargoCam.GetComponent<CinemachineVirtualCamera>().enabled = false;
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    public void StopShake()
    {
        CinemachineBasicMultiChannelPerlin _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = 0f;
    }

    IEnumerator PreviewLevelACA()
    {
        onCam = true;
        while (DialogueManager.isActive)
        {
            yield return null;
        }

        tutorialCharge.SetActive(false);
        tutorialRopJump.SetActive(false);
        cutsceneCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        cinemachineVirtualCamera.enabled = false;
        cutsceneCam.GetComponent<PlayableDirector>().enabled = true;
        
        yield return new WaitForSeconds(20);
        onCam = false;
        cinemachineVirtualCamera.enabled = true;
        cutsceneCam.GetComponent<CinemachineVirtualCamera>().enabled = false;
        tutorialCharge.SetActive(true);
        tutorialRopJump.SetActive(true);
    }

    public void ViewEnemyBelow()
    {
        CinemachineFramingTransposer offsetCam = cinemachineVirtualCamera.GetComponentInChildren<CinemachineFramingTransposer>();
        if (PlayerMovement.offset)
        {
            offsetY -= 0.2f;
        }
        else
        {
            offsetY += 1f;
        }

        offsetY = Mathf.Clamp(offsetY, -2.4f, 0f);

        float offsetSpeed = 5f;
        offsetCam.m_TrackedObjectOffset.y = Mathf.Lerp(offsetCam.m_TrackedObjectOffset.y, offsetY, Time.deltaTime * offsetSpeed);
    }

    public void FollowPlayerOnTrigger()
    {
        if (PlayerMovement.inRoom)
        {
            roomCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
            cinemachineVirtualCamera .enabled = false;
        }
        else
        {
            cinemachineVirtualCamera.enabled = true;
            roomCam.GetComponent<CinemachineVirtualCamera>().enabled = false;
        }
    }

    IEnumerator viewCargoAfterCheckpoint()
    {
        yield return new WaitForSeconds(1.5f);
        while (DialogueManager.isActive)
        {
            yield return null;
        }

        onCam = true;
        isClue = true;

        cargoCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        cinemachineVirtualCamera.enabled = false;

        yield return new WaitForSeconds(3);

        switchCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        cargoCam.GetComponent<CinemachineVirtualCamera>().enabled = false;

        yield return new WaitForSeconds(3);

        cinemachineVirtualCamera.enabled = true;
        switchCam.GetComponent<CinemachineVirtualCamera>().enabled = false;

        yield return new WaitForSeconds(2);

        onCam = false;
    }

    public void ZoomOutLaboratory()
    {
        onCam = true;

        LaboCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        cinemachineVirtualCamera.enabled = false;
    }
}
