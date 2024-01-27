using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFXManager : MonoBehaviour
{
    public static AudioFXManager instance;

    [SerializeField] private AudioSource audioFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayAudioFXClip(AudioClip audioClip, Transform spawnTransform, float Volume)
    {
        //spawn in gameObj
        AudioSource audioSource = Instantiate(audioFXObject);

        //assign audio
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = Volume;

        //play sound
        audioSource.Play();

        //get clip lenth
        float clipLength = audioSource.clip.length;

        //selfdestruct
        Destroy(audioSource.gameObject, clipLength );
         

    }
}
