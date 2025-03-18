using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireManager : MonoBehaviour
{
    private static readonly string[] cardTypes = new string[] { "H", "D", "C", "S" };
    private static readonly string[] cardValues = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "0", "J", "Q", "K" };

    [SerializeField] private SolitaireCard cardPrefab;
    [SerializeField] private Sprite[] cardFaces;

    private List<SolitaireCard> deck;

    //~ SETUP ~//
    void Start()
    {
        PrepareBoard();
    }

    public void PrepareBoard() {
        deck = GenerateDeck();
        Shuffle(deck);

        //-DEBUG
        foreach (SolitaireCard card in deck) {
            Debug.Log(card.name);
        }

        DealCards();
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

    private void DealCards() {
        foreach (SolitaireCard card in deck) {
            card.Reveal();
        }
    }

    //~ UTILITIES ~//
    void Shuffle<T> (List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int r = UnityEngine.Random.Range(i, list.Count);
            T tmp = list[i];
            list [i] = list[r];
            list[r] = tmp;
        }
    }
}
