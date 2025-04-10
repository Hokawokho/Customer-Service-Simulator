using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolitaireManager : MonoBehaviour
{
    public static readonly string[] cardSuits = new string[] { "H", "D", "C", "S" };
    public static readonly string[] cardValues = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "0", "J", "Q", "K" };

    [Header("Parámetros posición de cartas")]
    public float yCardOffset = 0.55f;
    public float zCardOffset = 0.03f;
    [Header("Parámetros snap y movimiento de cartas")]
    public float snapRange = 0.7f;
    public float moveSpeed = 30f;
    [Header("GameObjects & Scripts")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardFaces;
    [SerializeField] private List<Transform> tableauPiles;
    [SerializeField] private List<Transform> foundationPiles;

    private CardSuit goalFoundation;
    private Dictionary<string, Sprite> dictCardFaces;
    private Queue<Transform> movingCards;
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

        // Otras preparaciones.
        snapPoints = tableauPiles.ToList();
        movingCards = new Queue<Transform>();
    }

    void Update()
    {
        // Comprobar si las top cards se pueden colocar en el goal foundation.
        for (int i = 0; i < tableauPiles.Count; i++) {
            Transform topCard = GetTopCard(tableauPiles[i]);
            
            if (MoveCardToFoundation(topCard.transform)) {
                movingCards.Enqueue(topCard);
            }
        }

        // Mover cartas a foundation.
        if (movingCards.Count > 0) {
            Transform card = movingCards.Peek();
            Transform target = foundationPiles[(int)goalFoundation];
        
            card.position = Vector2.MoveTowards(card.position, target.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(card.position, target.position) < 0.01f) {
                Transform topFoundationCard = GetTopCard(target);

                card.parent = topFoundationCard;
                card.localPosition = new Vector3(0f, 0f, -zCardOffset);
                movingCards.Dequeue();
            }
        }

        // Condición de victoria.
        if (GetTopCard(foundationPiles[(int)goalFoundation]).CompareTag("SolitaireSlot")) return;
        if ((GetTopCard(foundationPiles[(int)goalFoundation]).GetComponent<SolitaireCard>().id
            == (goalFoundation.ToString() + "K"))
            && movingCards.Count == 0) {
            
            MinigameWon();
        }
    }

    //~ BOARD SETUP FUNCTIONS ~//
    public SolitaireCard CreateCard(string cardId) {
        GameObject card = Instantiate(cardPrefab);
        SolitaireCard cardScr = card.GetComponent<SolitaireCard>();
        cardScr.SetCard(cardId, dictCardFaces[cardId], this);
        cardScr.dragEndedDelegate = SnapCard;  // Dar funcionalidad de snap a carta.

        return cardScr;
    }

    public void PlaceCardOnTableau(SolitaireCard card, int index) {
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
        // Obtener carta top de foundation seleccionada.
        Transform topCard = GetTopCard(foundationPiles[(int)cardType]);

        // En caso de ser la primera carta, establecer cuál es el foundation objetivo.
        if (topCard.CompareTag("SolitaireSlot")) {
            goalFoundation = cardType;
        }

        // Colocar card.
        card.transform.parent = topCard;
        card.transform.localPosition = new Vector3(0f, 0f, -zCardOffset);
    }

    //~ GESTIÓN SNAP ~//
    public void SnapCard(Transform card) {
        //Debug.Log("BEGINNING OF SNAP FUNCTION (" + card.name + ")");    // DEBUG

        // Encontrar SolitaireSlot padre original y su índice.
        var (prevSlotType, prevSlotIndex) = FindSlotIndex(card);
        if (prevSlotIndex < 0) Debug.LogError("Índice del slot padre original no se pudo encontrar");
        //Debug.Log("Índice del slot padre original encontrado (" + prevSlotIndex + ")! Buscando slot...");   // DEBUG

        Transform prevSlot = FindSlot(card, prevSlotType, prevSlotIndex);
        if (prevSlot == null) Debug.LogError("Índice del slot padre original no se pudo encontrar. Comprueba la jerarquía de SolitaireBoard.");
        //Debug.Log("Slot encontrado (" + prevSlot.name + ")! Buscando nuevo snap point...");    // DEBUG

        // Encontrar su nuevo padre.
        for (int i = 0; i < snapPoints.Count; i++) {
            if (Vector2.Distance(snapPoints[i].position, card.position) <= snapRange
                && CheckSnapRestrictions(card, snapPoints[i])) {

                //Debug.Log("Nuevo snap point encontrado! (" + snapPoints[i].name + ")"); // DEBUG
                
                // Hacer su padre previo un snap point.
                if (!card.parent.CompareTag("SolitaireSlot")) {
                    card.parent.GetComponent<SolitaireCard>().Reveal(true);
                }
                snapPoints[prevSlotIndex] = card.parent;

                // Hacer la carta en la cima del conjunto de cartas que estamos moviendo un snap point.
                Transform temp = snapPoints[i];
                snapPoints[i] = GetTopCard(prevSlot);
                card.parent = temp;
            }
        }
        //Debug.Log("Finalizando snap...");   // DEBUG

        if (card.parent.CompareTag("SolitaireSlot")) {
            card.localPosition = new Vector3(0f, 0f, -zCardOffset);
        } else {
            card.localPosition = new Vector3(0f, -yCardOffset, -zCardOffset);
        }
        //Debug.Log("Snap realizado!");   // DEBUG

        /*// DEBUG {
        string finalSnapPoints = "Final snap point: (";
        foreach(Transform point in snapPoints) {
            finalSnapPoints += point.name + ", ";
        }
        finalSnapPoints += ")";
        Debug.Log(finalSnapPoints);
        *///} DEBUG
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
            return card.id[1].ToString() == "K";
        }
    }

    //~ OTHER ~//
    // Comprueba si la carta se puede poner en el foundation, y lo hace si es el caso.
    // Devuelve true si la carta se puede colocar en el foundation y comienza el proceso
    // de moverlo.
    private bool MoveCardToFoundation(Transform card) {
        SolitaireCard cardScr = card.GetComponent<SolitaireCard>();

        if (card.CompareTag("SolitaireSlot")) { // No es una carta.
            return false;
        }

        if (cardScr.id[0].ToString() == goalFoundation.ToString()) {
            Transform foundationSlot = foundationPiles[(int)goalFoundation];
            SolitaireCard topFoundationCard = GetTopCard(foundationSlot).GetComponent<SolitaireCard>();
            Transform underCard = card.parent;

            if (Array.IndexOf(cardValues, cardScr.id[1].ToString())
                == (Array.IndexOf(cardValues, topFoundationCard.id[1].ToString()) + 1)) {
                
                // Cambiar padre a top card del foundation.
                card.parent = null; // Se establecerá correctamente en update.
                card.GetComponent<SolitaireCard>().SetDraggable(false);

                // Hacer la carta que se encontraba debajo el snap point.
                var (_, underCardIndex) = FindSlotIndex(underCard);
                snapPoints[underCardIndex] = underCard;

                return true;
            } else {    // Su valor no es inmediatamente mayor al topcard del foundation.
                return false;
            }
        } else {    // No es del mismo tipo que el foundation.
            return false;
        }
    }

    // TODO: Hay que añadir escenas al build y cargarlo usando su build index o algo parecido.
    // Aparte, hay que cambiar como se reinicia el nivel. Opciones:
    // 1. Acceder a un método en futuro GameManager.
    // 2. 
    void OnGUI() {
        if (GUI.Button(new Rect (10, 10, 60, 20), "Reiniciar")) {
            GeneralManager.Instance.RestartScene();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void MinigameWon() {
        // TEMPORAL. En el futuro habrá un game manager que se ocupe de cerrar y/o
        // cambiar escena, o de cerrar el juego.
        FindObjectOfType<SolitaireLoader>().MinigameWon();
        Debug.Log("VICTORY!");
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
