using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usablePrinter : usableObject
{
    //Sonido a reproducir al hacer click
    [Tooltip("Sonido a reproducir al hacer click")]
    public AudioClip soundToPlay;

    private AudioSource audioSource;
    protected override void Start()
    {
        base.Start();
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    protected override void OnMouseEnter()
    {
        Debug.Log("Mouse entered " + gameObject.name);
        if(sr != null){
            sr.color = originalColor;
        }
        isBlinking = false;
    }
    protected override void OnMouseExit()
    {
        Debug.Log("Mouse exited " + gameObject.name);
        if(sr != null){
            sr.color = originalColor;
        }
        isBlinking = true;
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        transform.localScale = originalScale * highlightSize;
        if (audioSource != null && soundToPlay != null)
        {
            audioSource.PlayOneShot(soundToPlay);
        }
        else
        {
            Debug.LogWarning("No AudioSource or AudioClip found on " + gameObject.name);
        }
        //Implementar funcionalidad de impresion aqui
        //Animar el folio entrando, generar el objeto con datos, animar el folio saliendo
    }

    protected void OnMouseUp()
    {
        transform.localScale = originalScale;
    }

    protected override void Update()
    {
        if(isBlinking && sr != null)
        {
            float phase = (Mathf.Sin(Time.time * blinkFrequency) + 1f)/2f;
            sr.color = new Color(originalColor.r, originalColor.g - blinkIntensity * phase, originalColor.b, 1f);
        }
    }
}
