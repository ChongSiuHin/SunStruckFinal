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

    private void Update()
    {
        captureByEnemy();
    }

    private void captureByEnemy()
    {
        if (FindObjectOfType<StunGun>().hit)
        {
            hitZoomIn -= 1f;
        }
        else
        {
            hitZoomIn += 1f;
        }

        hitZoomIn = Mathf.Clamp(hitZoomIn, 1.5f, 3f);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, hitZoomIn, Time.deltaTime * zoomSpeed);
    }

    public void switchOn()
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
        yield return new WaitForSeconds(1);
        cinemachineVirtualCamera.LookAt = player.transform;
        cinemachineVirtualCamera.Follow = player.transform;
    }
}
