using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

    private CompareDocsManager manager;

    void Start()
    {
        // TODO: Cambiar a Application.persistentDataPath + "/Data/CompareDoc"
        // (y lidiar con los problemas que eso causa...)
        // Porque persistentDataPath es para los datos que persistirán entre ejecuciones.
        inputLinesPath = Application.dataPath + "/Data/CompareDoc/InputLine";
        regularLinesPath = Application.dataPath + "/Data/CompareDoc/RegularLine";

        manager = GetComponent<CompareDocsManager>();

        LoadLines();
    }

    private void LoadLines() {
        List<string> inputLineFiles = Directory.GetFiles(inputLinesPath, "*.json").ToList();
        List<string> regularLineFiles = Directory.GetFiles(regularLinesPath, "*json").ToList();
        Utilities.Shuffle(inputLineFiles);
        Utilities.Shuffle(regularLineFiles);

        List<LineData> lines = new List<LineData>();


        if (inputLineFiles.Count == 0 || regularLineFiles.Count == 0) {
            Debug.LogError("No se encontraron archivos json en " + inputLineFiles
                + "o " + regularLinesPath);
            return;
        }

        for (int i = 0; i < maxInputLines && i < inputLineFiles.Count; i++) {
            string file = inputLineFiles[i];
            string json = File.ReadAllText(file);
            InputLineData inputLine = JsonUtility.FromJson<InputLineData>(json);

            int r = Random.Range(0, inputLine.possibleResText.Count);
            inputLine.selectedResText = inputLine.possibleResText[r];

            lines.Add(inputLine);
        }
        for (int i = 0; i < maxRegularLines && i < regularLineFiles.Count; i++) {
            string selectedFile = regularLineFiles[i];
            string json = File.ReadAllText(selectedFile);
            RegularLineData regularLine = JsonUtility.FromJson<RegularLineData>(json);
            
            // Revisar si ya hay un inputline con el mismo texto.
            if (IsExistingInputLine(regularLine, lines)) {
                continue;
            }
            
            int r = Random.Range(0, regularLine.possibleSecondaryText.Count);
            regularLine.selectedSecondaryText = regularLine.possibleSecondaryText[r];

            lines.Add(regularLine);
        }

        Utilities.Shuffle(lines);

        for (int i = 0; i < lines.Count; i++) {
            ApplyLine(lines[i]);
        }

        docContents.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }

    private void ApplyLine(LineData line) {
        if (line is InputLineData) {
            GameObject inputLine = Instantiate(inputLinePrefab, docContents.transform);
            TMP_Text text = inputLine.transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = line.text;

            //TODO:
            // - Componente InputLineData que contenga en res esperado.
        } else {
            GameObject regularLine = Instantiate(regularLinePrefab, docContents.transform);
            TMP_Text text = regularLine.GetComponent<TMP_Text>();
            text.text = line.text + " " + ((RegularLineData) line).selectedSecondaryText;
        }
    }

    //~ UTILITIES ~//
    private bool IsExistingInputLine(LineData line, List<LineData> lines) {
        foreach (LineData l in lines) {
            if (l is  InputLineData && l.text == line.text) {
                return true;
            } else {
                continue;
            }
        }

        return false;
    }

}

// Cada linea tiene un texto primario.
[Serializable]
public class LineData {
    public string text;
}

// Linea de input tiene un resultado esperado en la caja en la que se escribe.
// - possibleResText es una de las posibilidades de resultado.
// - selectedResText está vacio en los Json, se escoge de entre las opciones
//      en possibleResText durante la carga.
[Serializable]
public class InputLineData : LineData {
    public List<string> possibleResText;
    public string selectedResText;
}

// Linea de input tiene un texto secundario.
// - possibleSecondaryText es una de las posibilidades de texto secundario.
// - selectedSecondaryText está vacio en los Json, se escoge de entre las opciones
//      en possibleSecondaryText durante la carga.
[Serializable]
public class RegularLineData : LineData {
    public List<string> possibleSecondaryText;
    public string selectedSecondaryText;
}