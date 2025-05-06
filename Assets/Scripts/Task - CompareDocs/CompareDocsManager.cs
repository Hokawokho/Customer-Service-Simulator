using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CompareDocsManager : MonoBehaviour
{
    private List<GameObject> allDocLines;
    private List<GameObject> allPcLines;
    private List<GameObject> inputDocLines;
    private List<GameObject> regularDocLines;
    private List<GameObject> resPcLines;
    private List<GameObject> regularPcLines;

    [SerializeField] private GameObject inputLinePrefab;
    [SerializeField] private GameObject regularLinePrefab;

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
}
