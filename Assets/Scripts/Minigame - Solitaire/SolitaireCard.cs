using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireCard : MonoBehaviour
{
    [SerializeField] private GameObject backOfCard;
    private SolitaireManager manager;

    //~ GETTERS/SETTERS ~//
    public string id {
        get; private set;
    }

    public void SetCard(string cardId, Sprite img, SolitaireManager mng) {
        name = cardId;
        id = cardId;
        GetComponent<SpriteRenderer>().sprite = img;
        manager = mng;
    }

    public bool IsFaceUp {
        get { return !backOfCard.activeSelf; }
    }

    public void Reveal(bool cond) {
        backOfCard.SetActive(!cond);
    }

    void Start()
    {
        
    }
}
