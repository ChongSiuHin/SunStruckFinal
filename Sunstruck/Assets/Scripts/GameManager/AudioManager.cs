using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource backgroundMusicSource;
    public AudioSource runSoundSource;
    public AudioSource robotSoundSource;
    public AudioSource ExposedSoundSource;

    //public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip jumpSound;
    public AudioClip runSound;
    public AudioClip PlatformRun;
    public AudioClip TrashCan;
    public AudioClip StunGunPickUp;
    public AudioClip SuitPickUp;
    public AudioClip StunGunFire;
    public AudioClip RobotRun;
    public AudioClip RobotStun;
    public AudioClip Checkpoint;
    public AudioClip PushingBox;
    public AudioClip Climbing;
    public AudioClip Exposed;
    public AudioClip Drop;
    public AudioClip Charging;
    public PlayerAudio Player;


    public void Start()
    {
        backgroundMusicSource = GetComponent<AudioSource>();
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayJumpSound()
    {
        runSoundSource.PlayOneShot(jumpSound);
    }

    public void PlayRunSound()
    {
        runSoundSource.clip = runSound;
        runSoundSource.Play();
        //audioSource.PlayOneShot(runSound);
    }

    public void StopPlayerSound()
    {
        Debug.Log("Sound was stop");
        Player.Stop();
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
        backgroundMusicSource.PlayOneShot(StunGunPickUp);
    }

    public void StunGunF()
    {
        backgroundMusicSource.PlayOneShot(StunGunFire);
    }

    public void Hiding()
    {
        backgroundMusicSource.PlayOneShot(TrashCan);
        //audioSource.clip = TrashCan;
        //audioSource.Play();
    }

    public void RespawnPoint()
    {
        backgroundMusicSource.PlayOneShot(Checkpoint);
        Debug.Log("Checkpoint");
    }

    public void CLimbingRope()
    {
        runSoundSource.clip = Climbing;
        runSoundSource.Play();
    }

    public void PushBox()
    {
        runSoundSource.PlayOneShot(PushingBox);
    }

    public void exposed()
    {
        //runSoundSource.PlayOneShot(Exposed);
        ExposedSoundSource.clip = Exposed;
        ExposedSoundSource.Play();
        Debug.Log("Exposed");
    }

    public void drop()
    {
        runSoundSource.PlayOneShot(Drop);
    }

    public void OnBeforeTransformParentChanged()
    {
        runSoundSource.PlayOneShot(Charging);
    }

    public void Suit()
    {
        runSoundSource.PlayOneShot(SuitPickUp);
        Debug.Log("Suit PickUp")
;    }

    public void StopExposedSound()
    {
        ExposedSoundSource.Stop();
    }
}

