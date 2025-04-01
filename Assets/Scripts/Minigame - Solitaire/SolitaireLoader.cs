using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Random = UnityEngine.Random;

public class SolitaireLoader : MonoBehaviour
{
    private string stateFolderPath;
    private List<string> stateFiles;
    private StateData initialStateData;

    [SerializeField] private SolitaireManager manager;
    
    void Start()
    {
        // TODO: Cambiar a Application.persistentDataPath + "/Data/Solitaire"
        // (y lidiar con los problemas que eso causa...)
        // Porque persistentDataPath es para los datos que persistirán entre ejecuciones.
        stateFolderPath = Application.dataPath + "/Data/Solitaire";

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

        initialStateData = JsonUtility.FromJson<StateData>(json);
        ApplyState();
    }

    private void ApplyState() {
        /* DEBUG {
        float xOffset = 0f;
        float zOffset = -0.03f;

        // Comprobar la pila final a completar
        foreach (CardData card in initialStateData.foundation.cards) {
            SolitaireCard newCard = manager.CreateCard(card.id);
            newCard.Reveal(true);
            newCard.transform.position = new Vector3(-(Camera.main.orthographicSize * Camera.main.aspect) + xOffset,
                                                    0f,
                                                    zOffset);
            
            xOffset += 1f;
            zOffset -= 0.03f;
        }

        xOffset = 0f;
        zOffset = -0.03f;

        // Comprobar cada escalera del tablero
        float yTabOffset = -3f;
        foreach (TableauRowData row in initialStateData.tableau) {
            float zExtraOffset = -0.1f;
            foreach (CardData card in row.cards) {
                SolitaireCard newCard = manager.CreateCard(card.id);
                newCard.Reveal(true);
                newCard.transform.position = new Vector3(-(Camera.main.orthographicSize * Camera.main.aspect) + xOffset,
                                                        yTabOffset,
                                                        zOffset);

                xOffset += 1f;
                zOffset -= 0.03f;
            }

            xOffset = 0f;
            zOffset += zExtraOffset;
            yTabOffset -= 1f;
        }
        */// } DEBUG
        
        // Colocar cartas en la foundation.
        List<CardData> foundationCards = initialStateData.foundation.cards;
        string foundationType = initialStateData.foundation.suit[0].ToString();
        for (int i = 0; i < foundationCards.Count; i++) {
            SolitaireCard newCard = manager.CreateCard(foundationCards[i].id);
            newCard.Reveal(true);
            manager.PlaceCardOnFoundation(newCard, (CardSuit)Enum.Parse(typeof(CardSuit), foundationType));
        }

        // Colocar cartas en el tableau.
        int[] randIndexes = Utilities.GetRandomIndexes(initialStateData.tableau);
        int index = 0;
        foreach(var i in randIndexes) {
            TableauRowData row = initialStateData.tableau[i];
            for (int j = 0; j < row.cards.Count; j++) {
                SolitaireCard card = manager.CreateCard(row.cards[j].id);
                card.Reveal(row.cards[j].isFaceUp);
                manager.PlaceCardOnTableau(card, index);
            }
            index++;
        }
    }
}

[Serializable]
public class StateData
{
    //public List<CardData> deck;
    public FoundationData foundation;
    public List<TableauRowData> tableau;
}

[Serializable]
public class CardData {
    public string id;
    public bool isFaceUp;
}

[Serializable]
public class FoundationData {
    public string suit;
    public List<CardData> cards;
}

[Serializable]
public class TableauRowData {
    public List<CardData> cards;
}
