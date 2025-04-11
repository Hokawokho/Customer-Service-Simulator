using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usableObject : MonoBehaviour
{
    //Variable que controla cuanto crece el objeto al apuntarlo
    [Tooltip("Tamaño del objeto al apuntarlo")]
    public float highlightSize = 1.1f;
    //Variable que controla lo rapido que crece el objeto al apuntarlo
    [Tooltip("Velocidad de crecimiento del objeto al apuntarlo")]
    public float highlightSpeed = 5f;
    //Variable que controla el offset en X de la posicion del objeto al apuntarlo
    [Tooltip("Offset de la posicion del objeto en X al apuntarlo")]
    public float highlightOffsetX = 0.1f;
    //Variable que controla el offset en Y de la posicion del objeto al apuntarlo
    [Tooltip("Offset de la posicion del objeto en Y al apuntarlo")]
    public float highlightOffsetY = 0.1f;

    //Variable que controla la frecuencia de parpadeo del objeto
    [Tooltip("Frecuencia de parpadeo del objeto")]
    public float blinkFrequency = 2f;
    //Variable que controla la intensidad del parpadeo
    [Tooltip("Intensidad del parpadeo del objeto")]
    public float blinkIntensity = 0.15f;

    protected Vector3 originalScale;
    protected Vector3 targetScale;
    protected Vector3 originalPosition;
    protected Vector3 targetOffset;
    protected Vector3 targetPosition;

    protected SpriteRenderer sr;
    protected Color originalColor;
    protected bool isBlinking = true;
    protected Collider2D col;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        col = GetComponent<Collider2D>();
        originalScale = transform.localScale;
        targetScale = originalScale;
        originalPosition = transform.localPosition;
        targetOffset = new Vector3(highlightOffsetX, highlightOffsetY, 0);
        targetPosition = originalPosition;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
        else
        {
            Debug.LogWarning("No SpriteRenderer found on " + gameObject.name);
        }
    }

    protected virtual void OnMouseEnter()
    {
        Debug.Log("Mouse entered " + gameObject.name);
        if(sr != null){
            sr.color = originalColor;
        }
        isBlinking = false;
        targetScale = originalScale * highlightSize;
        targetPosition = originalPosition + targetOffset;

    }

    protected virtual void OnMouseExit()
    {
        Debug.Log("Mouse exited " + gameObject.name);
        isBlinking = true;
        targetScale = originalScale;
        targetPosition = originalPosition;
    }

    protected virtual void OnMouseDown()
    {
        // Agregar aqui la logica de interaccion con el objeto
        Debug.Log("Interacting with " + gameObject.name);
    }

    protected virtual void OnDisable()
    {
        if(sr != null){
            sr.color = originalColor;
        }
        isBlinking = false;
        targetScale = originalScale;
        targetPosition = originalPosition;
        if(col != null){
            col.enabled = false;
        }
    }

    protected virtual void OnEnable()
    {
        if(col != null){
            col.enabled = true;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * highlightSpeed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * highlightSpeed);
        if(isBlinking && sr != null)
        {
            float phase = (Mathf.Sin(Time.time * blinkFrequency) + 1f)/2f;
            sr.color = new Color(1f - blinkIntensity * phase, 1f, 1f - blinkIntensity * phase, 1f);
        }
    }

    public virtual void DisableMe()
    {
        gameObject.SetActive(false);
    }
    public virtual void EnableMe()
    {
        gameObject.SetActive(true);
    }
}
