using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

    //private CardSuit goalFoundation;
    private Dictionary<string, Sprite> dictCardFaces;
    //private List<SolitaireCard>[] tableauCards;
    //private List<SolitaireCard> foundationCards;
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
        /*
        if (tableauCards == null) {
            tableauCards = new List<SolitaireCard>[7];
        }
        if (tableauCards[index] == null) {
            tableauCards[index] = new List<SolitaireCard>();
        }
        */

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
        /*
        if (foundationCards == null) {
            foundationCards = new List<SolitaireCard>();
        }
        */

        // Obtener carta top de foundation seleccionada.
        Transform topCard = GetTopCard(foundationPiles[(int)cardType]);

        // En caso de ser la primera carta, establecer cuál es el foundation objetivo.
        /*
        if (topCard.CompareTag("SolitaireSlot")) {
            goalFoundation = cardType;
        }
        */

        // Colocar card.
        card.transform.parent = topCard;
        card.transform.localPosition = new Vector3(0f, 0f, -zCardOffset);
    }

    //~ GESTIÓN SNAP ~//
    public void SnapCard(Transform card) {
        // Encontrar SolitaireSlot padre original y su índice.
        Debug.Log("BEGINNING OF SNAP FUNCTION (" + card.name + ")");

        var (prevSlotType, prevSlotIndex) = FindSlotIndex(card);
        if (prevSlotIndex < 0) Debug.LogError("Índice del slot padre original no se pudo encontrar");
        Debug.Log("Índice del slot padre original encontrado (" + prevSlotIndex + ")! Buscando slot...");   // DEBUG

        Transform prevSlot = FindSlot(card, prevSlotType, prevSlotIndex);
        if (prevSlot == null) Debug.LogError("Índice del slot padre original no se pudo encontrar. Comprueba la jerarquía de SolitaireBoard.");
        Debug.Log("Slot encontrado (" + prevSlot.name + ")! Buscando nuevo snap point...");    // DEBUG

        // Encontrar su nuevo padre.
        for (int i = 0; i < snapPoints.Count; i++) {
            if (Vector2.Distance(snapPoints[i].position, card.position) <= snapRange
                && CheckSnapRestrictions(card, snapPoints[i])) {

                Debug.Log("Nuevo snap point encontrado! (" + snapPoints[i].name + ")"); // DEBUG

                // Hacer su padre previo un snap point.
                /*
                if (card.transform.parent.CompareTag("SolitaireSlot")
                    && !card.transform.parent.GetComponent<SolitaireCard>().IsFaceUp) {
                    
                    RevealCard(card.transform.parent);
                } else {
                    snapPoints[prevSlotIndex] = card.transform.parent;
                }
                */

                // TODO:
                // - Terminar método RevealCard

                // Hacer la carta en la cima del conjunto de cartas que estamos moviendo un snap point.
                Transform temp = snapPoints[i];
                snapPoints[i] = GetTopCard(prevSlot);
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

    private bool CheckSnapRestrictions(Transform grabbedItem, Transform snapPoint) {
        SolitaireCard card = grabbedItem.GetComponent<SolitaireCard>();
        SolitaireCard snapCard = snapPoint.GetComponent<SolitaireCard>();
        
        if (snapCard != null) {
            if (snapCard.IsFaceUp) {
                bool cond1 = ((card.id[0].ToString().Equals(cardSuits[0]) || card.id[0].ToString().Equals(cardSuits[1]))
                    && (snapCard.id[0].ToString().Equals(cardSuits[2]) || snapCard.id[0].ToString().Equals(cardSuits[3])))
                    || ((card.id[0].ToString().Equals(cardSuits[2]) || card.id[0].ToString().Equals(cardSuits[3]))
                    && (snapCard.id[0].ToString().Equals(cardSuits[0]) || snapCard.id[0].ToString().Equals(cardSuits[1])));

                bool cond2 = (Array.IndexOf(cardValues, card.id[1].ToString()) + 1) == Array.IndexOf(cardValues, snapCard.id[1].ToString());

                return cond1 && cond2;
            } else {    // La carta sobre la que se quiere hacer snap está boca abajo.
                return false;
            }
        } else {    // El snap point no es una carta, es decir, es un slot vacío.
            return true;
        }
    }

    /*
    //~ OTHER ~//
    // Revela carta boca abajo, comprueba si se puede colocar en foundation y lo hace
    // en caso afirmativo.
    // Devuelve si es una carta que se pueda colocar en foundation o no.
    private bool RevealCard(Transform card) {
        SolitaireCard cardScr = card.GetComponent<SolitaireCard>();
        cardScr.Reveal(true);

        if (cardScr.id[0].ToString() == goalFoundation.ToString()) {
            SolitaireCard topFoundationCard = GetTopCard(card).GetComponent<SolitaireCard>();

            if (Array.IndexOf(cardValues, cardScr.id[1].ToString())
                == (Array.IndexOf(cardValues, topFoundationCard.id[1].ToString()) + 1)) {
                    // TODO:
                    // - Colocar carta automáticamente en foundation.
                    // - Hacer mismo proceso de comprobación a carta que estaba justo debajo
                    //   antes de mover la carta.
            }
        }

        throw new NotImplementedException();
    }
    */

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
