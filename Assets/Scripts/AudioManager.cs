using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //TODO: Audio Mixer
    [SerializeField]
    private AudioSource musicSource, sfxSource;

    public void PlayMusic(AudioClip clip) {
        if (musicSource.isPlaying && musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic() {
        musicSource.Stop();
    }

    public void PlaySound(AudioClip clip) {
        sfxSource.Stop();
    }
}
