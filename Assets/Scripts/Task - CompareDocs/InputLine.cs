using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InputLine : MonoBehaviour
{
    [SerializeField] private TMP_InputField textField;

    private string resText;
    private CompareDocsManager manager;

    public void SetLine(string resText, CompareDocsManager docManager) {
        this.resText = resText;
        manager = docManager;
    }

    public void OnValueChanged() {
        if (textField.text.Equals(resText)) {
            manager.LineCompleted(this);
        } else {
            manager.LineUnCompleted(this);
        }
    }   
}
