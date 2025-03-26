using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Random = UnityEngine.Random;

public class SolitaireLoader : MonoBehaviour
{
    public string stateFolderPath = Application.dataPath + "Data/Solitaire";
    private List<string> stateFiles;
    private StateData initialStateData;

    [SerializeField] private SolitaireManager manager;
    
    void Start()
    {
        LoadState();
    }

    private void LoadState() {
        stateFiles = Directory.GetFiles(stateFolderPath, "*.json").ToList();

        if (stateFiles.Count == 0) {
            Debug.LogError("No se encontraron archivos json en " + stateFolderPath);
            return;
        }

        int r = Random.Range(0, stateFiles.Count);
        string selectedFile = stateFiles[r];
        string json = File.ReadAllText(selectedFile);

        StateData initialStateData = JsonUtility.FromJson<StateData>(json);
        ApplyState();
    }

    private void ApplyState() {
        throw new NotImplementedException(); // TODO
    }

    private void ClearBoard() {
        throw new NotImplementedException(); // TODO
    }

}

[Serializable]
public class StateData
{
    public List<CardData> deck;
    public List<FoundationData> foundations;
    public List<TableauRowData> tableau;
}

[Serializable]
public class CardData {
    public string id;
    public bool revaled;
}

[Serializable]
public class FoundationData {
    public CardType type;
    public List<CardData> cards;
}

[Serializable]
public class TableauRowData {
    public List<CardData> cards;
}
