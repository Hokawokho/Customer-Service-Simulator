using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneOverlay : MonoBehaviour
{
    private bool overlayLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !overlayLoaded)
        {
            SceneManager.LoadSceneAsync("NombreEscena", LoadSceneMode.Additive);
            overlayLoaded = true;
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && overlayLoaded)
        {
            SceneManager.UnloadSceneAsync("NombreEscena");
            overlayLoaded = false;
        }
    }
}
