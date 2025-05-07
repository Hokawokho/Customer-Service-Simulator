using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fatherScript : MonoBehaviour
{
    public void DisableMe()
    {
        gameObject.SetActive(false);
    }
    public void EnableMe()
    {
        gameObject.SetActive(true);
    }
    public void DisableSons()
    {
        foreach (var child in GetComponentsInChildren<usableObject>())
        {
            child.DisableMe();
        }
    }
    public void EnableSons()
    {
        foreach (var child in GetComponentsInChildren<usableObject>(includeInactive: true))
        {
            child.EnableMe();
        }
    }
    public void DisableAll()
    {
        DisableSons();
        DisableMe();
    }
    public void EnableAll()
    {
        EnableMe();
        EnableSons();
    }

    //Deberia llamarse a esta funcion cuando se abra el menu de pausa
    public void freezeSons()
    {
        foreach (var child in GetComponentsInChildren<usableObject>())
        {
            child.freezeMe();
        }
    }
    //Deberia llamarse a esta funcion cuando se cierre el menu de pausa
    public void unfreezeSons()
    {
        foreach (var child in GetComponentsInChildren<usableObject>())
        {
            child.unfreezeMe();
        }
    }
}
