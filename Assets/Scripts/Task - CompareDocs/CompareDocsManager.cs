using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class CompareDocsManager : MonoBehaviour
{
    private List<InputLine> completedLines;
    private bool taskCompleted;

    public int numInputLines {
        private get; set;
    }

    [SerializeField] private GameObject inputLinePrefab;
    [SerializeField] private GameObject regularLinePrefab;
    [Header("Parámetros de oficina")]
    [SerializeField] private usableTransitionClick usableExitScript;

    void Start() {
        completedLines = new List<InputLine>();

        // TEMPORAL. Llamar desde OfficeManager cada vez que se quiera (re)iniciar tarea.
        NewTask();
    }

    void Update() {
        if (completedLines.Count == numInputLines && !taskCompleted) TaskWon();

        /*
        if (Input.GetKeyDown(KeyCode.Space)) {
            Clear();
            NewTask();
        }
        */
    }

    public void LineCompleted(InputLine line) {
        completedLines.Add(line);
    }

    public void LineUnCompleted(InputLine line) {
        completedLines.Remove(line);
    }

    // Usar en Office Manager.
    public void NewTask() {
        GetComponent<CompareDocsLoader>().LoadLines();
    }
    
    // Usar en Office Manager
    public void Clear() {
        GetComponent<CompareDocsLoader>().Clear();
        completedLines = new List<InputLine>();
        taskCompleted = false;
    }

    private void TaskWon() {
        taskCompleted = true;
        usableExitScript.Transition();
        // ej: officeManager.TaskWon(this); // TODO
    }
}