using System;
using UnityEngine;

public class DrawingWalls : MonoBehaviour
{
    public DrawManager drawManager;
    public Camera mainCamera;
    public Transform transformToChange;

    private void Start()
    {
        float orthographicSize = mainCamera.orthographicSize;
        float dynamicSizingX = orthographicSize * 2 * Screen.width / Screen.height;
        float dynamicSizingY = orthographicSize;
        transformToChange.localScale = new Vector3(dynamicSizingX, dynamicSizingY, 1f);
    }

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
        drawManager.AddLineToList();
        drawManager.SetIsMouseInWalls(false);
    }
}
