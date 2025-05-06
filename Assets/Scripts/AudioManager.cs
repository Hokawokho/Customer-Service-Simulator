using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private AudioMixer mixer;

    private const string MASTER_VOLUME_PARAM = "MasterVolume";
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";

    private void Start()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_VOLUME_PARAM, 1f));

        Debug.Log("Master vol: " + PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 1f).ToString());

    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_PARAM, GetMasterVolume());
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PARAM, GetMusicVolume());
        PlayerPrefs.SetFloat(SFX_VOLUME_PARAM, GetSFXVolume());

        Debug.Log("Setting master vol: " +  GetMasterVolume().ToString()); 
    }

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

    public float GetMasterVolume()
    {
        mixer.GetFloat(MASTER_VOLUME_PARAM, out float vol);
        return logToLin(vol);
    }
    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat(MASTER_VOLUME_PARAM, linToLog(volume));
    }

    public float GetMusicVolume()
    {
        mixer.GetFloat(MUSIC_VOLUME_PARAM, out float vol);
        return logToLin(vol);
    }
    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat(MUSIC_VOLUME_PARAM, linToLog(volume));
    }
    public float GetSFXVolume()
    {
        mixer.GetFloat(SFX_VOLUME_PARAM, out float vol);
        return logToLin(vol);
    }
    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat(SFX_VOLUME_PARAM, linToLog(volume));
    }

    //~ UTILITIES ~//
    float linToLog(float linear)
    {
        return Mathf.Log10(linear) * 20f;
    }

    float logToLin(float logarithmic)
    {
        return Mathf.Pow(10f, logarithmic / 20f);
    }
}
