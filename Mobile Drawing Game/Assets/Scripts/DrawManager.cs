using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class DrawManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject brush, brushStrokes;
    public Material brushMaterial;
    public Slider inkBar;
    
    public bool isMouseInWalls;

    public List<GameObject> _strokesInState;
    public List<float> _lengthInState;
    private LineRenderer _currentLineRenderer;
    private GameObject _lastStroke;
    private Material _currentMaterial;
    private Color _materialColor = Color.black;
    private Vector2 _lastPos;
    private int _currentRenderQueue = 2000;
    private float _lastLineLength = 0f;
    public bool _confirmState = false;

    private void Start()
    {
    }

    private void Update()
    {
        if(!_confirmState && inkBar.value > 0){
            Draw();
        }
    }

    private void Draw()
    {
        if(isMouseInWalls){
            if (Input.GetMouseButtonDown(0))
            {
                _lastPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);;
                CreateBrush();
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                if (mousePos != _lastPos)
                {
                    _lastLineLength += (mousePos - _lastPos).magnitude;
                    inkBar.value -= (mousePos - _lastPos).magnitude;
                    AddAPoint(mousePos);
                    _lastPos = mousePos;
                    if (inkBar.value <= 0)
                    {
                        AddLineToList("length");
                        _confirmState = true;
                    }
                }
            }
        }
        else
        {
            _currentLineRenderer = null;
        }
    }

    public void ClearLastLine()
    {
        if(_strokesInState.Count > 0 && _lengthInState.Count > 0){
            Destroy(_strokesInState.Last()); // Destroy the last object in the array
            _strokesInState.RemoveAt(_strokesInState.Count - 1);
            inkBar.value += _lengthInState.Last();
            _lengthInState.RemoveAt(_lengthInState.Count - 1);
            _confirmState = false;
        }
        if(_strokesInState.Count != _lengthInState.Count)
            _lengthInState.Insert(0, 0f);
    }

    public void ConfirmLastLine()
    {
        _strokesInState.Clear();
        _lengthInState.Clear();
        _confirmState = false;
        ResetState();
    }

    public void SetIsMouseInWalls(bool input)
    {
        isMouseInWalls = input;
    }
    
    public void AddLineToList(string type)
    {
        switch (type)
        {
            case "stroke":
                _strokesInState.Add(_lastStroke);
                break;
            case "length":
                Debug.Log(_lastLineLength + " " + _confirmState);
                if (!_confirmState)
                {
                    Debug.Log("in if");
                    if (_lastLineLength < 0.5f)
                    {
                        inkBar.value += _lastLineLength;
                        _lastLineLength = 0.5f;
                        inkBar.value -= _lastLineLength;
                    }
                    _lengthInState.Add(_lastLineLength);
                    _lastLineLength = 0f;
                }
                break;
        }
    }

    private void ResetState()
    {
        inkBar.value = 5f;
        _lastLineLength = 0f;
        isMouseInWalls = false;
    }

    public void SetBrushColor(string rgbHex)
    {
        var red = Convert.ToByte(int.Parse(rgbHex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber));
        var green = Convert.ToByte(int.Parse(rgbHex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber));
        var blue = Convert.ToByte(int.Parse(rgbHex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
        _materialColor = new Color32(red,green,blue,1);
    }

    private void CreateBrush()
    {
        var brushInstance = Instantiate(brush, brushStrokes.transform);
        _lastStroke = brushInstance;
        AddLineToList("stroke");
        var materialInstance = Instantiate(brushMaterial, brushInstance.transform);
        materialInstance.color = _materialColor;
        materialInstance.renderQueue = _currentRenderQueue;
        _currentRenderQueue++;
        _currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        _currentLineRenderer.material = materialInstance;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        _currentLineRenderer.SetPosition(0, mousePos);
        _currentLineRenderer.SetPosition(1, mousePos);
    }

    private void AddAPoint(Vector2 pointPos)
    {
        _currentLineRenderer.positionCount++;
        var positionIndex = _currentLineRenderer.positionCount - 1;
        _currentLineRenderer.SetPosition(positionIndex, pointPos);
    }
}