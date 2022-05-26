using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class DrawManager : MonoBehaviour
{
    public PhotonView view;
    public Room CurrentRoom;
    public Outline smallOutline, regularOutline, largeOutline;
    public GameObject regularButton, largeButton;
    public Camera mainCamera;
    public GameObject brush, brushStrokesParent; 
    public Material brushMaterial; 
    public Slider inkBar; 
    public ConfirmButton confirmButton;
    public List<GameObject> strokesInState; 
    public List<float> thicknessesOfStrokes;
    public List<float> lengthInState; 
    public TurnManager turnManager;

    private float _lineThickness = 1, _lineMultiplier = 1; 
    private LineRenderer _currentLineRenderer;
    private Color _materialColor = Color.black; 
    private Vector2 _lastPos;
    private int _currentRenderQueue = 2000; 
    private float _lastLineLength;
    private bool _confirmState, _isMouseInWalls;
    
    private void Start(){
        CurrentRoom = PhotonNetwork.CurrentRoom;
    }
    private void Update()
    {
        if(!_confirmState && inkBar.value > 0 && turnManager.isMyTurn && turnManager.isDrawer){ 
            Draw();
        }
    }

    private void Draw()
    {
        if(_isMouseInWalls){
            if (Input.GetMouseButtonDown(0))
            {
                _lastPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                CreateBrush();
            }

            if (!Input.GetMouseButton(0)) return;
            
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            if (mousePos == _lastPos) return;
            
            _lastLineLength += (mousePos - _lastPos).magnitude * _lineMultiplier;
            inkBar.value -= (mousePos - _lastPos).magnitude * _lineMultiplier;
            AddAPoint(mousePos);
            _lastPos = mousePos;
            
            if (!(inkBar.value <= 0)) return;
            
            AddLineToList();
            _confirmState = true;
        }
        else
        {
            _currentLineRenderer = null;
        }
    }

    public void ClearLastLine() 
    {
        if (!strokesInState.Any() || !lengthInState.Any()) return; 
        
        Destroy(strokesInState.Last());
        strokesInState.RemoveAt(strokesInState.Count - 1); 
        thicknessesOfStrokes.RemoveAt(thicknessesOfStrokes.Count - 1);
        inkBar.value += lengthInState.Last(); 
        lengthInState.RemoveAt(lengthInState.Count - 1); 
        _confirmState = false;
        if(inkBar.value >= 0.75f && inkBar.value < 1f)
        {
            regularButton.SetActive(true);
        }
        else if(inkBar.value >= 1f)
        {
            regularButton.SetActive(true);
            largeButton.SetActive(true);
        }
    }

    public void ConfirmButtonHandler()
    {
        if (PhotonNetwork.IsConnected)
        {
            confirmButton.ConfirmLastState(strokesInState, _currentRenderQueue, thicknessesOfStrokes);
        }

        ResetGameState();
    }

    private void ResetGameState()
    {
        strokesInState.Clear();
        thicknessesOfStrokes.Clear();
        lengthInState.Clear(); 
        _confirmState = false;
        _lastLineLength = 0f; 
        _isMouseInWalls = false;
        inkBar.value = 5f; 
        regularButton.SetActive(true); 
        largeButton.SetActive(true);
    }

    public void SetCurrentRenderQueue(int input)
    {
        _currentRenderQueue = input;
    } 
    
    public void SetIsMouseInWalls(bool input)
    {
        _isMouseInWalls = input;
    }

    private void AddStrokeToList(GameObject strokeInput)
    { 
        strokesInState.Add(strokeInput);
        thicknessesOfStrokes.Add(_lineThickness);
    }
    
    public void AddLineToList()
    {
        if (_confirmState) return;
        if (_lastLineLength < 0.5f) 
        {
            if(inkBar.value - 0.5f * _lineMultiplier + _lastLineLength > 0) 
            {
                inkBar.value -= 0.5f * _lineMultiplier - _lastLineLength;
                _lastLineLength = 0.5f * _lineMultiplier;
            }
            else if(inkBar.value != 0)
            {
                _lastLineLength = inkBar.value;
                inkBar.value = 0;
            }
        }
        lengthInState.Add(_lastLineLength);
        if (inkBar.value < 0.75f) 
        {
            SetBrushSize(1);
            regularButton.SetActive(false);
            largeButton.SetActive(false);
        }
        else if (inkBar.value < 1f)
        {
            SetBrushSize(1);
            largeButton.SetActive(false);
        }
        _lastLineLength = 0f;
    }
    
    public void SetBrushColor(string rgbHex)
    {
        byte red = Convert.ToByte(int.Parse(rgbHex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber));
        byte green = Convert.ToByte(int.Parse(rgbHex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber));
        byte blue = Convert.ToByte(int.Parse(rgbHex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
        _materialColor = new Color32(red,green,blue,255);
    }
    
    public void SetBrushSize(float size)
    {
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
    }

    private void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush, brushStrokesParent.transform);
        Material materialInstance = Instantiate(brushMaterial, brushInstance.transform);
        
        AddStrokeToList(brushInstance); 
        
        materialInstance.color = _materialColor;
        materialInstance.renderQueue = ++_currentRenderQueue;
        
        _currentRenderQueue++; 
        _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        _currentLineRenderer.widthMultiplier = _lineThickness;
        _currentLineRenderer.material = materialInstance;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        _currentLineRenderer.SetPosition(0, mousePos);
        _currentLineRenderer.SetPosition(1, mousePos);
    }

    private void AddAPoint(Vector2 pointPos)
    {
        _currentLineRenderer.positionCount++;
        int positionIndex = _currentLineRenderer.positionCount - 1;
        _currentLineRenderer.SetPosition(positionIndex, pointPos);
    }
}