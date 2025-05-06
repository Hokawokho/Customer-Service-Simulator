using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usableSoundClick : usableObject
{
    //Variable que controla el sonido a reproducir al hacer click
    [Tooltip("Sonido a reproducir al hacer click")]
    public AudioClip soundToPlay;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        if (audioSource != null && soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay);
        }
        else
        {
            Debug.LogWarning("No AudioSource or AudioClip found on " + gameObject.name);
        }
    }
}
