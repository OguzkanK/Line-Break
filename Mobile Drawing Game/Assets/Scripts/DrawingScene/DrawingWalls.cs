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

    private void OnMouseDown() // Call function when input starts within the collider
    {
        drawManager.SetIsMouseInWalls(true);
    }

    private void OnMouseExit() // Call function when input escapes the collider
    {
        drawManager.SetIsMouseInWalls(false);
    }

    private void OnMouseUp() // Call functions when input stops
    {
        drawManager.AddLineToList();
        drawManager.SetIsMouseInWalls(false);
    }
}
