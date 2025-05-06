using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usableExit : usableTransitionClick
{
    [Tooltip("Hijo que contiene el sprite indicador")]
    public GameObject indicatorSprite;
    protected override void Start()
    {
        base.Start();
        if(indicatorSprite != null){
            indicatorSprite.SetActive(false);
        }
        isBlinking = false;
    }

    protected override void OnMouseEnter()
    {
        Debug.Log("Mouse entered " + gameObject.name);
        if(indicatorSprite != null){
            indicatorSprite.SetActive(true);
        }
        isBlinking = true;
    }

    protected override void OnMouseExit()
    {
        Debug.Log("Mouse exited " + gameObject.name);
        if(indicatorSprite != null){
            indicatorSprite.SetActive(false);
        }
        isBlinking = false;
    }

    protected override void Update()
    {
        if(isBlinking && indicatorSprite != null)
        {
            indicatorSprite.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
