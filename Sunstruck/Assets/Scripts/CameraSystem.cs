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
    [SerializeField] private CinemachineVirtualCamera roomCam;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject OldMan;
    [SerializeField] private Animator cargoAnim;
    [SerializeField] private Animator craneAnim;
    private float hitZoomIn;
    private float offsetY;

    private float shakeIntensity = 3f;
    private float shakeTime = 1f;
    private float timer;
    public static bool onCam = false;

    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "AbandonedCargoArea")
        {
            CheckpointRespawn.currentTriggerObj = OldMan;
            OldMan.GetComponent<DialogueTrigger>().StartDialogue();
            StartCoroutine(PreviewLevelACA());
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
        if(SceneManager.GetActiveScene().name == "SurfaceWorld")
        {
            FollowPlayerOnTrigger();
        }
    }

    private void CaptureByEnemy()
    {
        if (StunGun.hit)
        {
            hitZoomIn -= 1f;
            ShakeCamera();
        }
        else
        {
            hitZoomIn += 1f;
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
        cinemachineVirtualCamera.LookAt = cargoAnim.transform;
        cinemachineVirtualCamera.Follow = cargoAnim.transform;
        yield return new WaitForSeconds(1);
        craneAnim.enabled = true;
        yield return new WaitForSeconds(0.83f);
        cargoAnim.enabled = true;
        ShakeCamera();
        yield return new WaitForSeconds(1.5f);
        cinemachineVirtualCamera.LookAt = player.transform;
        cinemachineVirtualCamera.Follow = player.transform;
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

        cutsceneCam.GetComponent<CinemachineVirtualCamera>().enabled = true;
        cinemachineVirtualCamera.enabled = false;
        cutsceneCam.GetComponent<PlayableDirector>().enabled = true;
        
        yield return new WaitForSeconds(20);
        onCam = false;
        cinemachineVirtualCamera.enabled = true;
        cutsceneCam.GetComponent<CinemachineVirtualCamera>().enabled = false;
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
            roomCam.enabled = true;
            cinemachineVirtualCamera .enabled = false;
        }
        else
        {
            cinemachineVirtualCamera.enabled = true;
            roomCam.enabled = false;
        }
    }
}
