using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator cargoAnim;
    private float hitZoomIn;

    private float shakeIntensity = 3f;
    private float shakeTime = 1f;
    private float timer;

    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private void Start()
    {
        StopShake();
    }

    private void Update()
    {
        captureByEnemy();

        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                StopShake();
            }
        }
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
            StopShake();
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
}
