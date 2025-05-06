using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class CompareDocsManager : MonoBehaviour
{
    /*
    private List<GameObject> allDocLines;
    private List<GameObject> allPcLines;
    private List<GameObject> inputDocLines;
    private List<GameObject> regularDocLines;
    private List<GameObject> resPcLines;
    private List<GameObject> regularPcLines;
    */
    private List<InputLine> completedLines;

    public int numInputLines {
        private get; set;
    }

    [SerializeField] private GameObject inputLinePrefab;
    [SerializeField] private GameObject regularLinePrefab;

    void Start() {
        completedLines = new List<InputLine>();
    }

    void Update() {
        if (completedLines.Count == numInputLines) TaskWon();
    }

    /*
    public void addInputLine(GameObject line) {
        // Añadir lineas de input y lineas de res del pc.

        inputDocLines.Add(line);
        resPcLines.Add(line);
        allDocLines.Add(line);
    }

    public void addRegularDocLines(GameObject line) {
        // Añadir lineas normales de doc.

        regularDocLines.Add(line);
        allDocLines.Add(line);
    }

    public void addRegularPcLines(GameObject line) {
        // Añadir lineas normales de pc.

        regularPcLines.Add(line);
    }
    */

    public void LineCompleted(InputLine line) {
        completedLines.Add(line);
    }

    public void LineUnCompleted(InputLine line) {
        completedLines.Remove(line);
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