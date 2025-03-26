using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolitaireCard : MonoBehaviour
{
    [SerializeField] private GameObject backOfCard;
    private SolitaireManager manager;

    private bool selectable;

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

    public void Reveal() {
        backOfCard.SetActive(false);
        selectable = true;
    }

    public void Unreveal() {
        backOfCard.SetActive(true);
        selectable = false;
    }

    void Start()
    {
        selectable = GetComponent<Selectable>();
    }
}
