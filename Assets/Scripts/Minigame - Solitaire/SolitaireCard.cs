using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SolitaireCard : MonoBehaviour
{
    [SerializeField] private GameObject backOfCard;
    private SolitaireManager manager;

    private bool _isDragged = false;
    private bool _isMoving = false;
    private Vector3 _mouseDragStartPos;
    private Vector3 _cardDragStartPos;
    private bool _draggable = true;


    public delegate void DragEndedDelegate(Transform transform);
    public DragEndedDelegate dragEndedDelegate;

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

    public void SetDraggable(bool cond) {
        _draggable = cond;
    }

    public void SetMoving(bool cond) {
        _isMoving = cond;
    }

    public bool IsMoving {
        get { return _isMoving; }
    }

    //~ EVENT FUNCTIONS ~//
    void OnMouseDown()
    {
        if (!IsFaceUp || !_draggable) return;

        _isDragged = true;
        _mouseDragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _cardDragStartPos = transform.position;

        ChangeCardsSortLayer();
    }

    void OnMouseDrag()
    {
        if (!IsFaceUp || !_draggable) return;

        if (_isDragged) {
            transform.position = _cardDragStartPos + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - _mouseDragStartPos);
        }
    }

    void OnMouseUp()
    {
        if (!IsFaceUp || !_draggable) return;
        
        _isDragged = false;
        ChangeCardsSortLayer();
        dragEndedDelegate(transform);
    }

    //~ UTILITIES ~//
    public void ChangeCardsSortLayer() {
        int currentSortOrder = transform.GetComponent<SpriteRenderer>().sortingOrder;
        int obj = currentSortOrder == 0 ? 1 : 0;

        transform.GetComponent<SpriteRenderer>().sortingOrder = obj;
        Transform currentCard = transform;
        while (currentCard.childCount > 1) {
            currentCard.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = obj;
            currentCard = currentCard.GetChild(1);
        }
    }
}
