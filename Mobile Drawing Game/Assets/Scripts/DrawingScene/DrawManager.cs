using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class DrawManager : MonoBehaviour
{
    public Outline smallOutline, regularOutline, largeOutline; // Outlines of the thickness buttons
    public GameObject regularButton, largeButton; // Thickness buttons
    public Camera mainCamera; // Main camera
    public GameObject brush, brushStrokesParent; // Brush prefab and the object which acts as a parent to created brush instances
    public Material brushMaterial; // BrushMaterial prefab
    public Slider inkBar; // inkBar slider
    public ConfirmButton confirmButton;
    public List<GameObject> strokesInState; // All the strokes created in one turn
    public List<float> lengthInState; // Lengths of the strokes created in one turn
    public TurnManager turnManager; // Script used for track the turns

    private float _lineThickness = 1, _lineMultiplier = 1; // Thickness of the line and the multiplier for decreasing the inkBar properly
    private LineRenderer _currentLineRenderer; // LineRenderer for the last created brush instance
    private Color _materialColor = Color.black; // Color selected for materials created from the prefab, defaults to black
    private Vector2 _lastPos; // Position of the last point created by the lineRenderer
    private int _currentRenderQueue = 2000; // Render order for the strokes, starts from 2000
    private float _lastLineLength; // Length of the last line created
    private bool _confirmState, _isMouseInWalls; // Bool to check if the game is in the confirmation state and to check if input happens within collider
    
    private void Update()
    {
        if(!_confirmState && inkBar.value > 0 && turnManager.isMyTurn && turnManager.isDrawer){ // Calls Draw function if the game is not in confirmation state and inkBar still has value larger than 0
            Draw();
        }// end if
    }// end Update

    private void Draw()
    {
        if(_isMouseInWalls){
            if (Input.GetMouseButtonDown(0))
            {
                _lastPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                CreateBrush();
            }// end if

            if (!Input.GetMouseButton(0)) return; // Returns if input does not exist
            
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            if (mousePos == _lastPos) return; // Returns if the input position is also the last position
            
            _lastLineLength += (mousePos - _lastPos).magnitude * _lineMultiplier; // Get the length of last created line
            inkBar.value -= (mousePos - _lastPos).magnitude * _lineMultiplier; // Subtract the last length from inkBar value
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
        if (!strokesInState.Any() || !lengthInState.Any()) return; // Return if either strokesInState or lengthInState are empty
        
        Destroy(strokesInState.Last()); // Destroy the last object in the strokes list
        strokesInState.RemoveAt(strokesInState.Count - 1); // Remove the destroyed object from the list
        inkBar.value += lengthInState.Last(); // Add the length of the deleted line back to the bar
        lengthInState.RemoveAt(lengthInState.Count - 1); // Remove the length added back to the bar
        _confirmState = false; // Resolve confirm state
        if(inkBar.value >= 0.75f && inkBar.value < 1f) // Enable regular and large thicknesses if they can be used
        {
            regularButton.SetActive(true);
        }// end if
        else if(inkBar.value >= 1f)
        {
            regularButton.SetActive(true);
            largeButton.SetActive(true);
        }// end else if
    }// End ClearLastLine
    
    public void ConfirmButtonHandler()
    {
        if(PhotonNetwork.IsConnected)
        {
            confirmButton.ConfirmLastState(strokesInState, _currentRenderQueue);
        }
        ResetGameState();
    } // End ConfirmButtonHandler

    private void ResetGameState()
    {
        strokesInState.Clear(); // Clear strokes list
        lengthInState.Clear(); // Clear lengths list
        _confirmState = false; // Resolve confirm state
        _lastLineLength = 0f; // Reset _lastLineLength value
        _isMouseInWalls = false; // Reset isMouseInWalls variable
        inkBar.value = 5f; // Reset inkBar value
        regularButton.SetActive(true); // Activate thickness buttons
        largeButton.SetActive(true);
    } // End ResetGameState

    public void SetCurrentRenderQueue(int input)
    {
        _currentRenderQueue = input;
    } // SetCurrentRenderQueue
    
    public void SetIsMouseInWalls(bool input)
    {
        _isMouseInWalls = input;
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
            if(inkBar.value - 0.5f * _lineMultiplier + _lastLineLength > 0) // See if the point left on the canvas uses more ink than what is left in the inkBar
            {
                inkBar.value -= 0.5f * _lineMultiplier - _lastLineLength;
                _lastLineLength = 0.5f * _lineMultiplier;
            }// end if
            else if(inkBar.value != 0)
            {
                _lastLineLength = inkBar.value;
                inkBar.value = 0;
            }// end else if
        }// end if
        lengthInState.Add(_lastLineLength); // Add the length of the last drawn line to access it during ClearLastLine function
        if (inkBar.value < 0.75f) // Disable regular and large thicknesses if they cannot be used
        {
            SetBrushSize(1); // Set the thickness to small
            regularButton.SetActive(false);
            largeButton.SetActive(false);
        }// end if
        else if (inkBar.value < 1f)
        {
            SetBrushSize(1);
            largeButton.SetActive(false);
        }// end else
        _lastLineLength = 0f;
    }// End AddLineToList
    
    public void SetBrushColor(string rgbHex) // SetBrushColor function for color changing buttons
    {
        // Convert the hex input to rgb values as bytes and create a new color
        byte red = Convert.ToByte(int.Parse(rgbHex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber));
        byte green = Convert.ToByte(int.Parse(rgbHex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber));
        byte blue = Convert.ToByte(int.Parse(rgbHex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
        _materialColor = new Color32(red,green,blue,255);
    }// End SetBrushColor
    
    public void SetBrushSize(float size) // SetBrushColor function for color changing buttons
    {
        // Set the thickness multiplier
        _lineThickness = size;
        switch (size)
        {
            case 1:
                _lineMultiplier = 1f;
                smallOutline.effectColor = Color.red;
                regularOutline.effectColor = Color.black;
                largeOutline.effectColor = Color.black;
                break;
            case 2:
                _lineMultiplier = 1.5f;
                smallOutline.effectColor = Color.black;
                regularOutline.effectColor = Color.red;
                largeOutline.effectColor = Color.black;
                break;
            case 3:
                _lineMultiplier = 2f;
                smallOutline.effectColor = Color.black;
                regularOutline.effectColor = Color.black;
                largeOutline.effectColor = Color.red;
                break;
            
        }
    }// End SetBrushSize

    private void CreateBrush()
    {
        // Create a brush instance and a material instance
        GameObject brushInstance = Instantiate(brush, brushStrokesParent.transform);
        Material materialInstance = Instantiate(brushMaterial, brushInstance.transform);
        
        AddStrokeToList(brushInstance); //Add stroke to the list
        
        // Set material color and render queue
        materialInstance.color = _materialColor;
        materialInstance.renderQueue = ++_currentRenderQueue;
        
        _currentRenderQueue++; // Increment render queue for future strokes so the strokes created later appear on top
        _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        _currentLineRenderer.widthMultiplier = _lineThickness;
        _currentLineRenderer.material = materialInstance;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        _currentLineRenderer.SetPosition(0, mousePos);
        _currentLineRenderer.SetPosition(1, mousePos);
    }// End CreateBrush

    private void AddAPoint(Vector2 pointPos)
    {
        _currentLineRenderer.positionCount++;
        int positionIndex = _currentLineRenderer.positionCount - 1;
        _currentLineRenderer.SetPosition(positionIndex, pointPos);
    }// End AddAPoint
}