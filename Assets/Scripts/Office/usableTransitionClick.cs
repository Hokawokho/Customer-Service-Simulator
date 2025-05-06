using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usableTransitionClick : usableObject
{
    //GameObject invocado al hacer click
    [Tooltip("Objeto que se invoca al hacer click")]
    public GameObject objectToInvoke;

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        var objScript = objectToInvoke.GetComponent<fatherScript>();
        if(objScript != null)
        {
            objScript.EnableAll();
        }
        var parentScript = GetComponentInParent<fatherScript>();
        if (parentScript != null)
        {
            parentScript.DisableAll();
        }
    }
}
