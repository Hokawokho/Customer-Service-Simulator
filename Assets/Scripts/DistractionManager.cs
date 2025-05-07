using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionManager : MonoBehaviour
{
    public float timeBetweenChecks = 60f;

    public float initialProbability = 10f;

    
    public float probabilityIncrement = 15f;

    private float currentProbability;
    private Coroutine currentRoutine;


    private MiniGameLoader miniGameLoader;

    private StressBar stressBar;

    private bool barResetTriggered = false;

    private List<int> availableMinigameIndices = new List<int> { 0, 1, 2, 3, 4 };


    private void Start()
    {
        miniGameLoader = FindObjectOfType<MiniGameLoader>();
        stressBar = FindObjectOfType<StressBar>();
        

        currentProbability = initialProbability;

        currentRoutine = StartCoroutine(CheckProbabilityRoutine());
    }

    private void Update()
    {
        if (miniGameLoader.IsMinigameActive)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
                stressBar.chargeMultiplier = 0.5f;

                
                 //TODO: esto esta en Start, pero hay que llamarlo cuando sea el cliente correspondiente.
                /*
                if (stressBar != null)
                {
                    stressBar.StartCharging();
                }
                */

                Debug.Log("Minijuego detectado. Corrutina detenida.");
            }
        }
        else
        {
            if (currentRoutine == null)
            {
                currentProbability = initialProbability;
                currentRoutine = StartCoroutine(CheckProbabilityRoutine());
                stressBar.chargeMultiplier = 1f;
                Debug.Log("Minijuego descargado. Corrutina reiniciada.");


                //TODO: Más de lo mismo, esto se debe llamar desde el cliente que pide la faena
                /*
                if (stressBar != null)
                {
                    stressBar.StartCharging(); // reiniciar barra visualmente y temporizador
                }
                */



            }

        }

        if (stressBar != null && stressBar.transform.localScale.x >= 1f && !barResetTriggered)
        {
                barResetTriggered = true;
                StartCoroutine(ResetStressBarAfterDelay(1f));
        }
    }

    private int GetRandomAvailableMinigame()
{
    if (availableMinigameIndices.Count == 0)
    {
        availableMinigameIndices = new List<int> { 0, 1, 2, 3, 4 };
        Debug.Log("Todos los minijuegos han sido jugados. Reiniciando la lista.");
    }

    int randomIndex = Random.Range(0, availableMinigameIndices.Count);
    int minigameIndex = availableMinigameIndices[randomIndex];
    availableMinigameIndices.RemoveAt(randomIndex); // Eliminar para evitar repetición

    return minigameIndex;
}

    private IEnumerator CheckProbabilityRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenChecks);

            float roll = Random.Range(0f, 100f);
            Debug.Log($"Probabilidad actual: {currentProbability}%");

            if (roll < currentProbability)
            {
                int selectedMinigame = GetRandomAvailableMinigame();
                Debug.Log($"Lanzando minijuego aleatorio: {selectedMinigame}");
                miniGameLoader.LoadMinigame(selectedMinigame);
            }
            else
            {
                currentProbability += probabilityIncrement;
                Debug.Log($"No ha saltado el minijuego. Probabilidad incrementada a {currentProbability}%");
            }
        }
    }

    public IEnumerator ResetStressBarAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (stressBar != null)
        {
            stressBar.transform.localScale = new Vector3(0f, stressBar.transform.localScale.y, stressBar.transform.localScale.z);
        }

        barResetTriggered = false;
    }
}