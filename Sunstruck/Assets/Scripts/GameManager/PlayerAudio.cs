using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip footstepClip;
    public AudioClip PlatformClip;
    public AudioSource audioSource;

    private bool platform;
    private PlayerMovement playerMovement;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        platform = playerMovement.Platform;
    }
    public void PlayFootstepSound()
    {
        Debug.Log("Playing footstep sound. Platform: " + playerMovement.Platform);
        if (playerMovement.Platform == true)
        {
            audioSource.PlayOneShot(PlatformClip);
        }
        else
        {
            audioSource.PlayOneShot(footstepClip);
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}

