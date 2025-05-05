using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralManager : Singleton<GeneralManager>
{
    public float interSceneWaitTime = 3f;
    
    public AudioManager audioManager;

    [SerializeField] GameObject optionsPanel;
    private bool paused = false;
    public bool pause {
        get {
            return paused;
        }
        set {
            paused = value;
            if (paused) {
                Time.timeScale = 0;
                optionsPanel.SetActive(true);
            } else {
                Time.timeScale = 1;
                optionsPanel.SetActive(false);
            }
        }
    }


    const int MAIN_MENU_SCENE_INDEX = 6;
    const int MAIN_GAME_SCENE_INDEX = 0;   //TODO: Cambiar a índice de escena principal en build.

    //~ SETUP ~//
    public override void Awake()
    {
        audioManager = GetComponent<AudioManager>();
    }

    //~ GESTIÓN ESCENA ~//
    /*
    public void GoToNextScene(float waitTime = -1) {
        if (waitTime < 0) waitTime = interSceneWaitTime;
        StartCoroutine(WaitAndLoadNextScene(waitTime));
    }

    private IEnumerator WaitAndLoadNextScene(float waitSeconds) {
        yield return new WaitForSeconds(waitSeconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    */

    // Elegir scena específica
    public void LoadScene(int sceneIndex, float waitTime = -1) {
        if (waitTime < 0) waitTime = interSceneWaitTime;
        StartCoroutine(WaitAndLoadScene(sceneIndex, waitTime));
    }

    private IEnumerator WaitAndLoadScene(int sceneIndex, float waitSeconds) {
        yield return new WaitForSeconds(waitSeconds);
        SceneManager.LoadScene(sceneIndex);
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    public void GoToMainGameScene() {
        if (MAIN_GAME_SCENE_INDEX < 0) {
            Debug.LogError("Escena principal no implementada o añadida a la build.");
            return;
        }
        SceneManager.LoadScene(MAIN_GAME_SCENE_INDEX);
    }

    public void Quit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    //~ OTHER ~//
    public void MinigameWon(MinigameEnum minigame) {
        //TODO: Gestionar minijuego terminado.
        Quit();
    }
}
