using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

public class DrawManager : MonoBehaviour
{
    public Camera mainCamera; // Main camera
    public GameObject brush, brushStrokes; // Brush prefab and the object which acts as a parent to created brush instances
    public Material brushMaterial; // BrushMaterial prefab
    public Slider inkBar; // inkBar slider
    
    public bool isMouseInWalls; // Bool to check if input happens within collider

    public List<GameObject> strokesInState; // All the strokes created in one turn
    public List<float> lengthInState; // Lengths of the strokes created in one turn
    private LineRenderer _currentLineRenderer; // LineRenderer for the last created brush instance
    private Color _materialColor = Color.black; // Color selected for materials created from the prefab, defaults to black
    private Vector2 _lastPos; // Position of the last point created by the lineRenderer
    private int _currentRenderQueue = 2000; // Render order for the strokes, starts from 2000
    private float _lastLineLength = 0f; // Length of the last line created
    private bool _confirmState = false; // Bool to check if the game is in the confirmation state

    private void Start()
    {
    }

    private void Update()
    {
        if(!_confirmState && inkBar.value > 0){ // Calls Draw function if the game is not in confirmation state and inkBar still has value larger than 0
            Draw();
        }// end if
    }// end Update

    private void Draw()
    {
        if(isMouseInWalls){
            if (Input.GetMouseButtonDown(0))
            {
                _lastPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);;
                CreateBrush();
            }// end if

            if (!Input.GetMouseButton(0)) return; // Returns if input does not exist
            
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            if (mousePos == _lastPos) return; // Returns if the input position is also the last position
            
            _lastLineLength += (mousePos - _lastPos).magnitude; // Get the length of last created line
            inkBar.value -= (mousePos - _lastPos).magnitude; // Subtract the last length from inkBar value
            AddAPoint(mousePos); // Add a point to the input position
            _lastPos = mousePos; // Assign input position to last position
            
            if (!(inkBar.value <= 0)) return; // Returns if inkBar is depleted
            
            AddLineToList();
            _confirmState = true; // Set the game to confirm state so that no more lines can be created until the confirm state is resolved
        }// end if
        else
        {
            _currentLineRenderer = null;
        }// end else
    }// end Draw

    public void ClearLastLine() // Clear function for the clear button
    {
        if(strokesInState.Count > 0 && lengthInState.Count > 0){
            Destroy(strokesInState.Last()); // Destroy the last object in the strokes list
            strokesInState.RemoveAt(strokesInState.Count - 1); // Remove the destroyed object from the list
            inkBar.value += lengthInState.Last(); // Add the length of the deleted line back to the bar
            lengthInState.RemoveAt(lengthInState.Count - 1); // Remove the length added back to the bar
            _confirmState = false; // Resolve confirm state
        }// end if
        if(strokesInState.Count != lengthInState.Count)
            lengthInState.Insert(0, 0f);
    }// End ClearLastLine

    public void ConfirmLastState() // Confirm function for the confirm button
    {
        strokesInState.Clear(); // Clear strokes list
        lengthInState.Clear(); // Clear lengths list
        _confirmState = false; // Resolve confirm state
        inkBar.value = 5f; // Reset inkBar value
        _lastLineLength = 0f; // Reset _lastLineLength value
        isMouseInWalls = false; // Reset isMouseInWalls variable
    }// End ConfirmLastState

    public void SetIsMouseInWalls(bool input)
    {
        isMouseInWalls = input;
    }// End SetMouseInWalls

    private void AddStrokeToList(GameObject strokeInput)
    {
        strokesInState.Add(strokeInput);
    }// End AddStrokeToList
    
    public void AddLineToList()
    {
        if (_confirmState) return; // Returns the function if the game is in the confirmation state
        if (_lastLineLength < 0.5f) // If the created line length is too small, default its length to 0.5f
        {
            inkBar.value += _lastLineLength;
            _lastLineLength = 0.5f;
            inkBar.value -= _lastLineLength;
        }
        lengthInState.Add(_lastLineLength);
        _lastLineLength = 0f;
    }// End AddLineToList
    public void SetBrushColor(string rgbHex) // SetBrushColor function for color changing buttons
    {
        // Convert the hex input to rgb values as bytes and create a new color
        var red = Convert.ToByte(int.Parse(rgbHex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber));
        var green = Convert.ToByte(int.Parse(rgbHex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber));
        var blue = Convert.ToByte(int.Parse(rgbHex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
        _materialColor = new Color32(red,green,blue,1);
    }// End SetBrushColor

    private void CreateBrush()
    {
        // Create a brush instance and a material instance
        var brushInstance = Instantiate(brush, brushStrokes.transform);
        var materialInstance = Instantiate(brushMaterial, brushInstance.transform);
        
        AddStrokeToList(brushInstance); //Add stroke to the list
        
        // Set material color and render queue
        materialInstance.color = _materialColor;
        materialInstance.renderQueue = _currentRenderQueue;
        
        _currentRenderQueue++; // Increment render queue for future strokes so the strokes created later appear on top
        _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        _currentLineRenderer.material = materialInstance;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        _currentLineRenderer.SetPosition(0, mousePos);
        _currentLineRenderer.SetPosition(1, mousePos);
    }// End CreateBrush

    private void AddAPoint(Vector2 pointPos)
    {
        _currentLineRenderer.positionCount++;
        var positionIndex = _currentLineRenderer.positionCount - 1;
        _currentLineRenderer.SetPosition(positionIndex, pointPos);
    }// End AddAPoint
}