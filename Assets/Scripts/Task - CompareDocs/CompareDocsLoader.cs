using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

// WIP (quizás rehacer con un approach distinto)
public class CompareDocsLoader : MonoBehaviour
{

    private string inputLinesPath;
    private string regularLinesPath;

    public int maxInputLines = 3;
    public int maxRegularLines = 7;
    [SerializeField] private GameObject inputLinePrefab;
    [SerializeField] private GameObject regularLinePrefab;
    [SerializeField] private GameObject docContents;

    void Start()
    {
        // TODO: Cambiar a Application.persistentDataPath + "/Data/CompareDoc"
        // (y lidiar con los problemas que eso causa...)
        // Porque persistentDataPath es para los datos que persistirán entre ejecuciones.
        inputLinesPath = Application.dataPath + "/Data/CompareDoc/InputLine";
        regularLinesPath = Application.dataPath + "/Data/CompareDoc/RegularLine";

        LoadLines();
    }

    private void LoadLines() {
        List<string> inputLineFiles = Directory.GetFiles(inputLinesPath, "*.json").ToList();
        List<string> regularLineFiles = Directory.GetFiles(regularLinesPath, "*json").ToList();

        List<LineData> lines = new List<LineData>();


        if (inputLineFiles.Count == 0 || regularLineFiles.Count == 0) {
            Debug.LogError("No se encontraron archivos json en " + inputLineFiles
                + "o " + regularLinesPath);
            return;
        }

        for (int i = 0; i < maxInputLines; i++) {
            string file = inputLineFiles[i];
            string json = File.ReadAllText(file);
            LineData inputLine = JsonUtility.FromJson<LineData>(json);

            lines.Add(inputLine);
        }
        for (int i = 0; i < maxRegularLines; i++) {
            string selectedFile = regularLineFiles[i];
            string json = File.ReadAllText(selectedFile);
            LineData regularLine = JsonUtility.FromJson<LineData>(json);

            lines.Add(regularLine);
        }

        Utilities.Shuffle(lines);

        for (int i = 0; i < lines.Count; i++) {
            ApplyLine(lines[i]);
        }

        docContents.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }

    private void ApplyLine(LineData line) {
        if (line.isInputLine) {
            GameObject inputLine = Instantiate(inputLinePrefab, docContents.transform);
            TMP_Text text = inputLine.transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = line.text;

            //TODO
        } else {
            GameObject regularLine = Instantiate(regularLinePrefab, docContents.transform);
            TMP_Text text = regularLine.GetComponent<TMP_Text>();
            text.text = line.text;
        }
    }

}

[Serializable]
public class LineData {
    public string text;
    public string desiredRes;
    public bool isInputLine;
}