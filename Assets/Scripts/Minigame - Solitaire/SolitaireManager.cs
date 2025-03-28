using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireManager : MonoBehaviour
{
    private static readonly string[] cardSuits = new string[] { "H", "D", "C", "S" };
    private static readonly string[] cardValues = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "0", "J", "Q", "K" };

    public float yCardOffset = 0.2f;
    public float zCardOffset = 0.03f;
    [SerializeField] private SolitaireCard cardPrefab;
    [SerializeField] private Sprite[] cardFaces;
    [SerializeField] private List<Transform> tableauPiles;
    [SerializeField] private List<Transform> foundationPiles;

    private List<SolitaireCard> deck;
    private Dictionary<string, Sprite> dictCardFaces;
    private List<SolitaireCard>[] tableauCards;
    private List<SolitaireCard> foundationCards;


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

    //~ BOARD PLACING FUNCTIONS ~//
    public SolitaireCard CreateCard(string cardId) {
        SolitaireCard card = Instantiate(cardPrefab);
        card.SetCard(cardId, dictCardFaces[cardId], this);
        //deck.Add(card);
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
        Transform topCard = tableauPiles[index];
        while (topCard.childCount > 0) topCard = topCard.GetChild(0);

        // Colocar card.
        card.transform.parent = topCard;
        card.transform.position = new Vector3(0f, yCardOffset, -zCardOffset);
    }

    //~ UTILITIES ~//
    void Shuffle<T> (List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int r = Random.Range(i, list.Count);
            T tmp = list[i];
            list [i] = list[r];
            list[r] = tmp;
        }
    }
}
