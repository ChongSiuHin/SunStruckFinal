using System.Collections.Generic;
using UnityEngine;

public class CameraAudioCheck : MonoBehaviour
{
    public AudioManager audioManager;
    private Dictionary<GameObject, AudioSource> audioSources;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        audioSources = audioManager.GetAllAudioSourcesWithObjects();
        if (mainCamera == null)
        {
            Debug.Log("Main camera is null");
        }

        if (audioManager == null)
        {
            Debug.Log("Audio Manager is null");
        }
        else
        {
            if (audioSources == null)
            {
                Debug.Log("Audio Sources is null");
            }
        }
    }

    private void Update()
    {
        if (audioSources != null)
        {
            foreach (var entry in audioSources)
            {
                GameObject enemy = entry.Key;
                AudioSource audioSource = entry.Value;

                if (audioSource == null)
                {
                    Debug.Log("An audio source is null");
                    continue;
                }

                if (IsInCameraView(enemy))
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                else
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
            }
        }
    }

    private bool IsInCameraView(GameObject obj)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(obj.transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }
}
