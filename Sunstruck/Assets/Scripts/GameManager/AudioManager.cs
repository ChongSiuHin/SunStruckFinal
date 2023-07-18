using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource backgroundMusicSource;
    public AudioSource runSoundSource;
    public AudioSource robotSoundSource;

    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip jumpSound;
    public AudioClip runSound;
    public AudioClip PlatformRun;
    public AudioClip TrashCan;
    public AudioClip StunGunPickUp;
    public AudioClip StunGunFire;
    public AudioClip RobotRun;
    public AudioClip RobotStun;
    public AudioClip Checkpoint;


    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayRunSound()
    {
        runSoundSource.clip = runSound;
        runSoundSource.Play();
        //audioSource.PlayOneShot(runSound);
    }

    public void StopCurrentSound()
    {
        runSoundSource.Stop();
    }

    public void RobotMoving()
    {
        robotSoundSource.clip = RobotRun;
        robotSoundSource.Play();
    }

    public void RobotStuning()
    {
        robotSoundSource.clip = RobotStun;
        robotSoundSource.Play();
    }

    public void WalkInPlatform()
    {
        runSoundSource.clip = PlatformRun;
        runSoundSource.Play();
    }

    public void StunGunP()
    {
        audioSource.PlayOneShot(StunGunPickUp);
    }

    public void StunGunF()
    {
        audioSource.PlayOneShot(StunGunFire);
    }

    public void Hiding()
    {
        audioSource.PlayOneShot(TrashCan);
        //audioSource.clip = TrashCan;
        //audioSource.Play();
    }

    public void RespawnPoint()
    {
        audioSource.PlayOneShot(Checkpoint);
    }
}

