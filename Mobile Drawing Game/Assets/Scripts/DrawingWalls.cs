using UnityEngine;

public class DrawingWalls : MonoBehaviour
{
    public DrawManager drawManager;

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
