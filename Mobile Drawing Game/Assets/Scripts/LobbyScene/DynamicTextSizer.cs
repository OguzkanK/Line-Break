using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DynamicTextSizer : MonoBehaviour
{
    public TMP_Text createPlaceholder, createInput, joinPlaceholder, joinInput; 
    private float changeSizeFontTo = Screen.height / 40;
    void Start()
    {
        createPlaceholder.fontSize = changeSizeFontTo;
        createInput.fontSize = changeSizeFontTo;
        joinPlaceholder.fontSize = changeSizeFontTo;
        joinInput.fontSize = changeSizeFontTo;
    }
}
