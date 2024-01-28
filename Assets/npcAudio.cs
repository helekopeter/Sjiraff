using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class npcAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] runAway;
    [SerializeField] private AudioClip[] knockedDown;

    [SerializeField] private AudioSource audio;

    private bool ignoreRun = false;

    private void OnValidate()
    {
        if (audio == null)
        {
            audio = GetComponent<AudioSource>();
        }
    }

    public void PlayRunSound()
    {
        if (!ignoreRun)
        {
            audio.PlayOneShot(runAway[Random.Range(0, runAway.Length)]);
        }
    }

    public void PlayLaunchSound()
    {
        ignoreRun = true;
        audio.PlayOneShot(knockedDown[Random.Range(0, runAway.Length)]);
    }
}
