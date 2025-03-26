using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireManager : MonoBehaviour
{
    private static readonly string[] cardTypes = new string[] { "H", "D", "C", "S" };
    private static readonly string[] cardValues = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "0", "J", "Q", "K" };

    [SerializeField] private SolitaireCard cardPrefab;
    [SerializeField] private Sprite[] cardFaces;
    [SerializeField] private Transform stock;
    [SerializeField] private Transform wastePile;
    [SerializeField] private List<Transform> tableauPiles;
    [SerializeField] private List<Transform> foundationPiles;

    private List<SolitaireCard> deck;

    //~ SETUP ~//
    void Start()
    {
        PrepareBoard();
    }

    public void PrepareBoard() {
        deck = GenerateDeck();
        Shuffle(deck);

        DealCards();
    }

    public List<SolitaireCard> GenerateDeck() {
        List<SolitaireCard> newDeck = new List<SolitaireCard>();
        int faceIndex = 0;
        foreach (string type in cardTypes) {
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

    //~ BOARD PLACING FUNCTIONS ~//
    

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
