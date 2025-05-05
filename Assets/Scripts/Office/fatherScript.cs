using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fatherScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GeneralManager.Instance.pause = !GeneralManager.Instance.pause;

            if (GeneralManager.Instance.pause) {
                DisableSons();
            } else {
                EnableSons();
            }
        }
    }

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
        foreach (var child in GetComponentsInChildren<usableObject>(true))
        {
            child.DisableMe();
        }
    }
    public void EnableSons()
    {
        foreach (var child in GetComponentsInChildren<usableObject>(true))
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
