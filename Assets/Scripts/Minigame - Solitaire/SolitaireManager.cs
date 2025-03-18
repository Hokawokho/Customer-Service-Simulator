using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireManager : MonoBehaviour
{
    private static readonly string[] cardTypes = new string[] { "H", "D", "C", "S" };
    private static readonly string[] cardValues = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    [SerializeField] private SolitaireCard cardPrefab;
    [SerializeField] private Sprite[] cardFaces;

    private List<SolitaireCard> deck;

    void Start()
    {
        DealCards();
    }

    public void DealCards() {
        deck = GenerateDeck();
    }

    public List<SolitaireCard> GenerateDeck() {
        List<SolitaireCard> newDeck = new List<SolitaireCard>();
        int face = 0;
        foreach (string t in cardTypes) {
            foreach (string v in cardValues) {
                var card = Instantiate<SolitaireCard>(cardPrefab);
                card.SetCard(t+v, cardFaces[face], this);
                newDeck.Add(card);
            }
        }
        return newDeck;
    }
}
