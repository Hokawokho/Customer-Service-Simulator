using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{   
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;

    void Start()
    {
        masterVolume.value = GeneralManager.Instance.audioManager.GetMasterVolume();
        musicVolume.value = GeneralManager.Instance.audioManager.GetMusicVolume();
        sfxVolume.value = GeneralManager.Instance.audioManager.GetSFXVolume();
    }


    public void OnMasterVolumeChanged()
    {
        GeneralManager.Instance.audioManager.SetMasterVolume(masterVolume.value);
    }
    public void OnMusicVolumeChanged()
    {
        GeneralManager.Instance.audioManager.SetMusicVolume(musicVolume.value);
    }
    public void OnSFXVolumeChanged()
    {
        GeneralManager.Instance.audioManager.SetSFXVolume(sfxVolume.value);
    }

    //~ BUTTONS ~//
    public void OnResume() {
        GeneralManager.Instance.pause = false;
    }

    public void OnGoToMainMenu()
    {
        GeneralManager.Instance.pause = false;
        GeneralManager.Instance.GoToMainMenu();
    }
}
