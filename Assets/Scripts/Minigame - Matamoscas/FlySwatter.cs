using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FlySwatter : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float reloadingTime = 1f;
    public float hitFXTime = 0.5f;
    private Vector3 mousePos;
    private Vector2 pos;

    private bool _reloading;
    private List<GameObject> flies = new List<GameObject>();

    [SerializeField] private Sprite[] reloadingSprites;
    [SerializeField] private Sprite[] hitFXSprites;
    private SpriteRenderer headSprRenderer;
    private SpriteRenderer hitSprRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        headSprRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        hitSprRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!_reloading && Input.GetMouseButtonDown(0)) {
            foreach (GameObject fly in flies) {
                fly.GetComponent<Fly>().Kill();
            }
            flies.Clear();

            StartCoroutine(Reloading());
            StartCoroutine(HitFX());
        }
    }

    void FixedUpdate()
    {
        FollowMouse();
    }

    private void FollowMouse() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = Vector2.Lerp(transform.position, mousePos, moveSpeed);
        rb.MovePosition(pos);
    }

    private IEnumerator Reloading() {
        _reloading = true;
        float waitTime = reloadingTime / (reloadingSprites.Length + 1);
        
        // Start reload
        headSprRenderer.sprite = reloadingSprites[0];
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 0.5f;
        GetComponent<SpriteRenderer>().color = tmp;
        tmp = headSprRenderer.color;
        tmp.a = 1f;
        headSprRenderer.color = tmp;

        for (int i = 1; i < reloadingSprites.Length; i++) {
            yield return new WaitForSeconds(waitTime);
            headSprRenderer.sprite = reloadingSprites[i];
        }
        tmp.a = 0f;
        headSprRenderer.color = tmp;
        yield return new WaitForSeconds(waitTime);
        
        // End reload
        tmp = GetComponent<SpriteRenderer>().color;
        tmp.a = 1f;
        GetComponent<SpriteRenderer>().color = tmp;

        _reloading = false;
    }

    private IEnumerator HitFX() {
        float waitTime = hitFXTime / (hitFXSprites.Length + 1);
        
        hitSprRenderer.sprite = hitFXSprites[0];
        Color tmp = hitSprRenderer.color;
        tmp.a = 1f;
        hitSprRenderer.color = tmp;

        for (int i = 1; i < hitFXSprites.Length; i++) {
            yield return new WaitForSeconds(waitTime);
            hitSprRenderer.sprite = hitFXSprites[i];
        }
        tmp.a = 0f;
        hitSprRenderer.color = tmp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fly")) {
            Debug.Log("Fly enter");
            flies.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Fly exit");
        flies.Remove(other.gameObject);
    }
}
