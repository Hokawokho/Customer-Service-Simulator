using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StarWin : MonoBehaviour
{   

    private StressBar stressBar;

    private BoxCollider2D _box;

    private MiniGameLoader miniGameLoader;
    // Start is called before the first frame update
    void Start()
    {
        stressBar = FindObjectOfType<StressBar>();
        _box = GetComponent<BoxCollider2D>();
        miniGameLoader = FindObjectOfType<MiniGameLoader>();

    }

  void OnTriggerEnter2D(Collider2D other)
    {
        // Aquí puedes filtrar por etiqueta si solo el jugador debe activar esto
        if (other.CompareTag("Player") && miniGameLoader != null)
        {
            Debug.Log("Coleccionable recogido. Cerrando minijuego...");
            if (miniGameLoader.IsMinigameActive)
            {
                miniGameLoader.UnloadCurrentMinigame();
            }
        }
    }
}