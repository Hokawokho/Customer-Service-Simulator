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
    private static int randState = -1;  // Estático para que se mantenga igual al darle a restart.

    [SerializeField] private SolitaireManager manager;
    
    void Start()
    {
        // TODO: Cambiar a Application.persistentDataPath + "/Data/Solitaire"
        // (y lidiar con los problemas que eso causa...)
        // Porque persistentDataPath es para los datos que persistirán entre ejecuciones.
        stateFolderPath = Application.dataPath + "/Data/Solitaire";

        LoadState();
        ApplyState();
    }

    private void LoadState() {
        stateFiles = Directory.GetFiles(stateFolderPath, "*.json").ToList();

        if (stateFiles.Count == 0) {
            Debug.LogError("No se encontraron archivos json en " + stateFolderPath);
            return;
        }

        if (randState == -1) randState = Random.Range(0, stateFiles.Count);
        string selectedFile = stateFiles[randState];
        string json = File.ReadAllText(selectedFile);

        initialStateData = JsonUtility.FromJson<StateData>(json);
    }

    private void ApplyState() {
        // Colocar cartas en la foundation.
        CardData foundationCard = initialStateData.foundation.topCard;
        string foundationType = initialStateData.foundation.suit;
        int valueIndex = Array.IndexOf(SolitaireManager.cardValues, foundationCard.id[1].ToString());
        for (int i = 0; i <= valueIndex; i++) {
            SolitaireCard newCard = manager.CreateCard(foundationType + SolitaireManager.cardValues[i]);
            newCard.Reveal(true);
            newCard.SetDraggable(false);
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

    public void MinigameWon() {
        randState = -1; // Se devuelve a -1 para que la siguiente vez que salga el minijuego se cargue un estado distinto.
        Destroy(gameObject);
    }
}

[Serializable]
public class StateData
{
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
    public CardData topCard;
}

[Serializable]
public class TableauRowData {
    public List<CardData> cards;
}
