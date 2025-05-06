using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameLoader : MonoBehaviour
{
    public SceneAsset[] minigameScenes;
    private string[] minigameSceneNames;
    private string currentMinigameScene;

    // Start is called before the first frame update
    // void Start()
    // {
    //     var async = SceneManager.LoadSceneAsync("PlataformasMinigame", LoadSceneMode.Additive);
    //     async.completed += _ =>
    //     {
    //         var scene = SceneManager.GetSceneByName("PlataformasMinigame");
    //         if (scene.IsValid())
    //             SceneManager.SetActiveScene(scene);
    //     };
    // }

    // Update is called once per frame
    
    void Awake()
    {
        // Convertimos los SceneAsset en nombres de escena para cargarlas
        minigameSceneNames = new string[minigameScenes.Length];
        for (int i = 0; i < minigameScenes.Length; i++)
        {
            if (minigameScenes[i] != null)
            {
                minigameSceneNames[i] = minigameScenes[i].name;
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadMinigame(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadMinigame(1);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadMinigame(2);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            LoadMinigame(3);
        }



        else if (Input.GetKeyDown(KeyCode.X))
        {
            UnloadCurrentMinigame();
        }
    }

    void LoadMinigame(int index){

         string sceneName = minigameSceneNames[index];

        if (currentMinigameScene == sceneName || !string.IsNullOrEmpty(currentMinigameScene))
            return;

        var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        async.completed += _ =>
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                SceneManager.SetActiveScene(scene);
                currentMinigameScene = sceneName;
            }
        };
    }

     void UnloadCurrentMinigame()
    {
        if (string.IsNullOrEmpty(currentMinigameScene)) return;

        SceneManager.UnloadSceneAsync(currentMinigameScene);
        currentMinigameScene = null;
    }
}
