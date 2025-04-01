using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class SolitaireManager : MonoBehaviour
{
    private static readonly string[] cardSuits = new string[] { "H", "D", "C", "S" };
    private static readonly string[] cardValues = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "0", "J", "Q", "K" };

    [Header("Parámetros posición de cartas")]
    public float yCardOffset = 0.55f;
    public float zCardOffset = 0.03f;
    [Header("Parámetros snap de cartas")]
    public float snapRange = 0.7f;
    [Header("GameObjects & Scripts")]
    [SerializeField] private SolitaireCard cardPrefab;
    [SerializeField] private Sprite[] cardFaces;
    [SerializeField] private List<Transform> tableauPiles;
    [SerializeField] private List<Transform> foundationPiles;

    private Dictionary<string, Sprite> dictCardFaces;
    private List<SolitaireCard>[] tableauCards;
    private List<SolitaireCard> foundationCards;
    private List<SolitaireCard> allCards;
    private List<Transform> snapPoints;

    //~ SETUP ~//
    void Awake()
    {
        // Comprobar que el array cardFaces está inicializado y completo.
        if (cardFaces == null) {
            Debug.LogError("Array cardFaces no inicializada. Ves al componente y añade "
                + (cardSuits.Length * cardValues.Length) + "sprites de cartas.");
            return;
        } else if (cardFaces.Length == 0) {
            Debug.LogError("Array cardFaces no contiene suficientes sprites. Ves al componente y añade "
                + (cardSuits.Length * cardValues.Length - cardFaces.Length) + " sprites más.");
            return;
        }

        // Inicializa diccionario de sprites de cartas.
        dictCardFaces = new Dictionary<string, Sprite>();
        int index = 0;
        foreach (string type in cardSuits) {
            foreach(string value in cardValues) {
                string key = type+value;
                dictCardFaces.Add(key, cardFaces[index]);
                index++;
            }
        }

        // Preparación snap.
        snapPoints = tableauPiles.ToList();
    }

    void Start()
    {
        //PrepareBoard();
    }

    /*
    public void PrepareBoard() {
        deck = GenerateDeck();
        Shuffle(deck);

        DealCards();
    }

    public List<SolitaireCard> GenerateDeck() {
        List<SolitaireCard> newDeck = new List<SolitaireCard>();
        int faceIndex = 0;
        foreach (string type in cardSuits) {
            foreach (string value in cardValues) {
                var card = Instantiate<SolitaireCard>(cardPrefab);
                card.SetCard(type+value, cardFaces[faceIndex], this);
                newDeck.Add(card);
            }
        }
        return newDeck;
    }

    private void DealCards() {
        foreach (SolitaireCard card in deck) {
            card.Reveal();
        }
    }
    */

    //~ BOARD SETUP FUNCTIONS ~//
    public SolitaireCard CreateCard(string cardId) {
        SolitaireCard card = Instantiate(cardPrefab);
        card.SetCard(cardId, dictCardFaces[cardId], this);
        card.dragEndedDelegate = SnapCard;  // Dar funcionalidad de snap a carta.
        
        if (allCards == null) allCards = new List<SolitaireCard>();
        allCards.Add(card);

        return card;
    }

    public void PlaceCardOnTableau(SolitaireCard card, int index) {
        if (tableauCards == null) {
            tableauCards = new List<SolitaireCard>[7];
        }
        if (tableauCards[index] == null) {
            tableauCards[index] = new List<SolitaireCard>();
        }

        // Obtener carta top de escalera seleccionada.
        Transform topCard = GetTopCard(tableauPiles[index]);

        // Colocar card.
        card.transform.parent = topCard;
        if (topCard.CompareTag("SolitaireSlot")) {
            card.transform.localPosition = new Vector3(0f, 0f, -zCardOffset);
        } else {
            card.transform.localPosition = new Vector3(0f, -yCardOffset, -zCardOffset);
        }

        // Establecer nueva carta como snap point.
        snapPoints[index] = card.transform;
    }

    public void PlaceCardOnFoundation(SolitaireCard card, CardSuit cardType) {
        if (foundationCards == null) {
            foundationCards = new List<SolitaireCard>();
        }

        // Obtener carta top de foundation seleccionada.
        Transform topCard = GetTopCard(foundationPiles[(int)cardType]);

        // Colocar card.
        card.transform.parent = topCard;
        card.transform.localPosition = new Vector3(0f, 0f, -zCardOffset);
    }

    //~ GESTIÓN SNAP ~//
    public void SnapCard(Transform card) {
        // Encontrar SolitaireSlot padre original y su índice.
        Debug.Log("BEGINNING OF SNAP FUNCTION (" + card.name + ")");

        var (prevSlotType, prevSlotIndex) = FindSlotIndex(card);
        if (prevSlotIndex < 0) Debug.LogError("Índice del slot padre original no se pudo encontrar");   // DEBUG
        Debug.Log("Índice del slot padre original encontrado (" + prevSlotIndex + ")! Buscando slot...");

        Transform prevSlot = FindSlot(card, prevSlotType, prevSlotIndex);
        if (prevSlot == null) Debug.LogError("Índice del slot padre original no se pudo encontrar. Comprueba la jerarquía de SolitaireBoard."); // DEBUG
        else Debug.Log("Slot encontrado (" + prevSlot.name + ")! Buscando nuevo snap point...");

        // Encontrar su nuevo padre.
        for (int i = 0; i < snapPoints.Count; i++) {
            if (Vector2.Distance(snapPoints[i].position, card.position) <= snapRange) {
                Debug.Log("Nuevo snap point encontrado! (" + snapPoints[i].name + ")"); // DEBUG
                snapPoints[prevSlotIndex] = card.transform.parent; // Hacer su padre previo un snap point.

                Transform temp = snapPoints[i];
                snapPoints[i] = GetTopCard(prevSlot);   // Hacer la carta en la cima del conjunto de cartas que estamos moviendo un snap point.
                card.transform.parent = temp;
            }
        }
        Debug.Log("Finalizando snap...");   // DEBUG

        if (card.parent.CompareTag("SolitaireSlot")) {
            card.transform.localPosition = new Vector3(0f, 0f, -zCardOffset);
        } else {
            card.transform.localPosition = new Vector3(0f, -yCardOffset, -zCardOffset);
        }
        Debug.Log("Snap realizado!");   // DEBUG

        // DEBUG {
        string finalSnapPoints = "Final snap point: (";
        foreach(Transform point in snapPoints) {
            finalSnapPoints += point.name + ", ";
        }
        finalSnapPoints += ")";
        Debug.Log(finalSnapPoints);
        //} DEBUG
    }

    //~ UTILITIES ~//
    // Devuelve la carta arriba del todo de la pila que le pases (foundation o escalera de tableau)
    // Si el slot está vacio, devuelve la propia pila.
    private Transform GetTopCard(Transform pile) {
        Transform topCard = pile;
        if (topCard.CompareTag("SolitaireSlot") && topCard.childCount > 0) { // Primero, revisar si el slot está vacío.
            topCard = topCard.GetChild(0);
            // Si no lo está, revisar cada carta hijo hasta encontrar el último.
            while (topCard.childCount > 1) topCard = topCard.GetChild(1);
        }
        return topCard;
    }

    // Devuelve el tipo e índice del slot en la que se encuentra la carta indicada.
    private (SlotType, int) FindSlotIndex(Transform card) {
        // Encontrar el slot de la pila.
        Transform slot = card;
        while (!slot.CompareTag("SolitaireSlot")) {
            slot = slot.parent;
        }

        // Buscar entre las pilas de tableau.
        int slotIndex = tableauPiles.FindIndex(a => a.name == slot.name);
        if (slotIndex >= 0) return (SlotType.Tableau, slotIndex);

        // Buscar entre las foundations.
        slotIndex = foundationPiles.FindIndex(a => a.name == slot.name);
        if (slotIndex >= 0) return (SlotType.Foundation, slotIndex);

        // Valor en caso de que no se encuentre en ninguno.
        return (default, -1);
    }

    private Transform FindSlot(Transform card, SlotType slotType = default, int index = -1) {
        if (index < 0) {
            (slotType, index) = FindSlotIndex(card);
        }
        
        if (index >= 0) {
            switch(slotType) {
            case SlotType.Tableau:
                return tableauPiles[index];
            case SlotType.Foundation:
                return foundationPiles[index];
            }
        }

        return null;
    }
}
