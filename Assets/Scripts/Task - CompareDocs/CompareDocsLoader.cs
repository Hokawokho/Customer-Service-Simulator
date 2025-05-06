using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CompareDocsLoader : MonoBehaviour
{

    private string inputLinesPath;
    private string regularLinesPath;

    public int maxInputLines = 3;
    public int maxRegularLines = 7;
    [SerializeField] private GameObject inputLinePrefab;
    [SerializeField] private GameObject regularLinePrefab;
    [SerializeField] private GameObject docContents;
    [SerializeField] private GameObject pcRegularLinePrefab;
    [SerializeField] private GameObject pcContents;

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
        if (inputLineFiles.Count == 0 || regularLineFiles.Count == 0) {
            Debug.LogError("No se encontraron archivos json en " + inputLinesPath
                + "o " + regularLinesPath);
            return;
        }
        Utilities.Shuffle(inputLineFiles);
        Utilities.Shuffle(regularLineFiles);

        List<LineData> inputLines = new List<LineData>();
        List<LineData> regularLines = new List<LineData>();
        for (int i = 0; i < maxInputLines && i < inputLineFiles.Count; i++) {
            string file = inputLineFiles[i];
            string json = File.ReadAllText(file);
            InputLineData inputLine = JsonUtility.FromJson<InputLineData>(json);

            // Elegir un resultado esperado aleatorio.
            int r = Random.Range(0, inputLine.possibleResText.Count);
            inputLine.selectedResText = inputLine.possibleResText[r];

            inputLines.Add(inputLine);
        }
        for (int i = 0; i < regularLineFiles.Count; i++) {
            string file = regularLineFiles[i];
            string json = File.ReadAllText(file);
            RegularLineData regularLine = JsonUtility.FromJson<RegularLineData>(json);

            // Elegir un resultado esperado aleatorio.
            int r = Random.Range(0, regularLine.possibleSecondaryText.Count);
            regularLine.selectedSecondaryText = regularLine.possibleSecondaryText[r];

            regularLines.Add(regularLine);
        }

        LoadLinesInDoc(inputLines, regularLines);
        LoadLinesInPc(inputLines, regularLines);
    }

    private void LoadLinesInDoc(List<LineData> inputLines, List<LineData> regularLines) {
        List<LineData> lines = new List<LineData>(inputLines);
        
        int regularLineAmount = 0;
        for (int i = 0; regularLineAmount < maxRegularLines && i < regularLines.Count; i++) {
            RegularLineData regularLine = (RegularLineData)regularLines[i];
            
            // Revisar si ya hay una linea con el mismo texto.
            if (IsExistingLine(regularLine, lines)) {
                continue;
            }
            
            lines.Add(regularLine);
            regularLineAmount++;
        }

        Utilities.Shuffle(lines);

        for (int i = 0; i < lines.Count; i++) {
            ApplyLine(lines[i], docContents);
        }

        docContents.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }

    private void LoadLinesInPc(List<LineData> inputLines, List<LineData> regularLines) {
        List<LineData> lines = new List<LineData>(inputLines);

        for (int i = 0; i < regularLines.Count; i++) {
            RegularLineData regularLine = (RegularLineData)regularLines[i];
            
            // Revisar si ya hay una linea con el mismo texto.
            if (IsExistingLine(regularLine, lines)) {
                continue;
            }
            
            lines.Add(regularLine);
        }

        Utilities.Shuffle(lines);

        for (int i = 0; i < lines.Count; i++) {
            ApplyLine(lines[i], pcContents);
        }
    }

    private void ApplyLine(LineData line, GameObject contentObj) {
        if (line is InputLineData) {
            if (contentObj.name.Equals("DocContent")) {
                GameObject inputLine = Instantiate(inputLinePrefab, contentObj.transform);
                TMP_Text text = inputLine.transform.GetChild(0).GetComponent<TMP_Text>();
                text.text = line.text;

                //TODO:
                // - Componente InputLine que contenga en res esperado.
            } else {
                GameObject inputLine = Instantiate(pcRegularLinePrefab, contentObj.transform);
                TMP_Text text = inputLine.transform.GetComponent<TMP_Text>();
                text.text = "<b>" + line.text + "</b> " + ((InputLineData) line).selectedResText;
            }
        } else {
            GameObject regularLine;
            if (contentObj.name.Equals("DocContent")) {
                regularLine = Instantiate(regularLinePrefab, contentObj.transform);
            } else {
                regularLine = Instantiate(pcRegularLinePrefab, contentObj.transform);
                line.text = "<b>" + line.text + "</b>";
            }

            TMP_Text text = regularLine.GetComponent<TMP_Text>();
            text.text = line.text + " " + ((RegularLineData) line).selectedSecondaryText;
        }
    }

    //~ UTILITIES ~//
    private bool IsExistingLine(LineData line, List<LineData> lines) {
        foreach (LineData l in lines) {
            if (l.text == line.text) {
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