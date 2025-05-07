using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrintManager : MonoBehaviour
{
    public float printChance = 0.5f;
    public float printSpeed = 1f;

    [SerializeField] private Transform document;
    [SerializeField] private Transform beginPoint;
    [SerializeField] private Transform endPoint;

    public bool isPrinting {
        get; private set;
    }
    public bool isPrinted {
        get; private set;
    }

    void Update()
    {
        if (isPrinting) {
            document.position = Vector3.MoveTowards(document.position,
                                                    endPoint.position,
                                                    printSpeed * Time.deltaTime);

            if (Vector2.Distance(document.position, endPoint.position) < 0.01f) {
                isPrinting = false;
                isPrinted = true;

                //TaskWon();
            }
        }

        /*DEBUG
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Resetting task...");
            NewTask();
        }
        */ 
    }

    public void NewTask() {
        ResetPrint();
    }

    public void TryPrint() {
        if (isPrinting || isPrinted) return;

        float r = Random.Range(0f, 1f);
        if (r >= printChance) {
            StartPrint();
        } else {
            // StartMinigame();
        }
    }

    public void StartPrint() {
        isPrinting = true;
    }

    public void ResetPrint() {
        document.position = beginPoint.position;
        isPrinting = false;
        isPrinted = false;
    }

   // Opcional: Empezar minijuego bola papel. 
    public void StartMinigame() {
        //TODO?
    }

    private void TaskWon() {
        // TEMPORAL. En el futuro habrá un game manager que se ocupe de cerrar y/o
        // cambiar escena, o de cerrar el juego.
        Debug.Log("TASK COMPLETED!");
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
