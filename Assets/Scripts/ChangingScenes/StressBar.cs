using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StressBar : MonoBehaviour
{
    private bool isCharging = false;
    private float timer = 0f;
    public float chargeDuration = 10f;
    public float chargeMultiplier = 1f;

    private MiniGameLoader miniGameLoader;

    private Animator _anim;

    private SceneManager sceneManager;

    private bool ClientActive;


    private void Start()
    {
        miniGameLoader = FindObjectOfType<MiniGameLoader>();
        _anim = FindObjectOfType<Animator>();
    }

    public void StartCharging()
    {
        isCharging = true;
        ClientActive = true;
        timer = 0f;
        transform.localScale = new Vector3(0f, transform.localScale.y, transform.localScale.z); 
    }
    void Update()
    {
        // Iniciar la carga al presionar C
        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     isCharging = true;
        //     timer = 0f;
        //     transform.localScale = new Vector3(0f, transform.localScale.y, transform.localScale.z); // Reiniciar la barra
        // }

        // Si se está cargando, actualizar la escala
        if (isCharging)
        {

            if (_anim != null && isCharging && ClientActive)
            {
                _anim.SetBool("ClientActive", isCharging && ClientActive);
            }

            timer += Time.deltaTime * chargeMultiplier;

            // Calcular el porcentaje de carga
            float progress = Mathf.Clamp01(timer / chargeDuration);

            // Escalar la barra en X
            transform.localScale = new Vector3(progress, transform.localScale.y, transform.localScale.z);

            // Si ha terminado
            if (progress >= 1f)
            {
                isCharging = false;
                ClientActive = false;
                Debug.Log("Cargado");
                if (miniGameLoader.IsMinigameActive && SceneManager.sceneCount > 1)
                {   
                    Debug.Log($"Escenas cargadas: {SceneManager.sceneCount}. Activa: {SceneManager.GetActiveScene().name}");

                    miniGameLoader.UnloadCurrentMinigame();
                }
                transform.localScale = new Vector3(0f, transform.localScale.y, transform.localScale.z);

            }
        }
    }
}