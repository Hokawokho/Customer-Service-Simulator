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
        foreach (var child in GetComponentsInChildren<usableObject>())
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
}
