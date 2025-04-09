using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuTheme;

    void Start()
    {
        if (mainMenuTheme != null) GeneralManager.Instance.audioManager.PlayMusic(mainMenuTheme);
    }

    public void StartGame() {
        GeneralManager.Instance.GoToMainGameScene();
    }

    public void Credits() {
        //TODO
    }

    public void QuitGame() {
        GeneralManager.Instance.Quit();
    }
}
