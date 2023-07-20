using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera cutsceneCam;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator cargoAnim;
    private float hitZoomIn;
    private float offsetY;

    private float shakeIntensity = 3f;
    private float shakeTime = 1f;
    private float timer;

    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "AbandonedCargoArea")
        {
            StartCoroutine(previewLevelACA());
        }
        
        StopShake();
    }

    private void Update()
    {
        captureByEnemy();

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StopShake();
            }
        }

        viewEnemyBelow();
    }

    private void captureByEnemy()
    {
        if (FindObjectOfType<StunGun>().hit)
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

    public void switchOnCargo()
    {
        StartCoroutine(dropCargo());
    }

    IEnumerator dropCargo()
    {
        cinemachineVirtualCamera.LookAt = cargoAnim.transform;
        cinemachineVirtualCamera.Follow = cargoAnim.transform;
        yield return new WaitForSeconds(1);
        cargoAnim.SetTrigger("Drop");
        StartCoroutine(followBackPlayer());
    }

    IEnumerator followBackPlayer()
    {
        ShakeCamera();
        yield return new WaitForSeconds(1);
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

    IEnumerator previewLevelACA()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        cutsceneCam.enabled = true;
        cinemachineVirtualCamera.enabled = false;
        yield return new WaitForSeconds(20);
        player.GetComponent<PlayerMovement>().enabled = true;
        cinemachineVirtualCamera.enabled = true;
        cutsceneCam.enabled = false;
    }

    public void viewEnemyBelow()
    {
        CinemachineFramingTransposer offsetCam = cinemachineVirtualCamera.GetComponentInChildren<CinemachineFramingTransposer>();
        if (FindObjectOfType<InteractionSystem>().offset)
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
}
