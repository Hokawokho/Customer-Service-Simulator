using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFieldTest : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(text.text.Equals("Hola"));
    }
}
