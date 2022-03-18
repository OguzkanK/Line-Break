using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawingWalls : MonoBehaviour
{
    public DrawManager drawManager;

    private void OnMouseDown()
    {
        drawManager.SetIsMouseInWalls(true);
    }

    private void OnMouseExit()
    {
        drawManager.SetIsMouseInWalls(false);
    }

    private void OnMouseUp()
    {
        drawManager.AddLineToList("length");
        drawManager.SetIsMouseInWalls(false);
    }
}
