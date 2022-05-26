using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConfirmButton : MonoBehaviour
{
    public PhotonView view;
    public GameObject brush, brushStrokes;
    public Material brushMaterial;
    public DrawManager drawManager;
    public void ConfirmLastState(List<GameObject> strokesInState, int currentRenderQueue, List<float> thicknessesOfStrokes) // Confirm function for the confirm button
    {
        float[] rgbValuesFromSource = new float[4];
        int index = 0;
        foreach (GameObject stroke in strokesInState)
        {
            LineRenderer lineRendererFromSource = stroke.GetComponent<LineRenderer>();
            Vector3[] positionsFromSource = new Vector3[lineRendererFromSource.positionCount];
            lineRendererFromSource.GetPositions(positionsFromSource);
            Color materialColor = lineRendererFromSource.material.color;
            rgbValuesFromSource[0] = materialColor.r * 255f;
            rgbValuesFromSource[1] = materialColor.g * 255f;
            rgbValuesFromSource[2] = materialColor.b * 255f;
            rgbValuesFromSource[3] = materialColor.a * 255f;
            
            view.RPC("SyncCreatedLine", RpcTarget.OthersBuffered, stroke.transform.position, positionsFromSource,
                rgbValuesFromSource[0], rgbValuesFromSource[1], rgbValuesFromSource[2], rgbValuesFromSource[3], currentRenderQueue,
                thicknessesOfStrokes[index]);
            index++;
        }
    }
    
    [PunRPC]
    public void SyncCreatedLine(Vector3 sourcePosition, Vector3[] positionsFromSource, float r, float g, float b, float a,
                                int renderQueueFromSource, float thicknessesOfStrokes)
    {
        GameObject brushInstance = Instantiate(brush, brushStrokes.transform);
        Material materialInstance = Instantiate(brushMaterial, brushInstance.transform);
        LineRenderer lineRendererToCopy = brushInstance.GetComponent<LineRenderer>();
        lineRendererToCopy.positionCount -= 2;
        lineRendererToCopy.widthMultiplier = thicknessesOfStrokes;
        
        materialInstance.color = new Color32(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b), Convert.ToByte(a));
        materialInstance.renderQueue = renderQueueFromSource;
        drawManager.SetCurrentRenderQueue(renderQueueFromSource);
        lineRendererToCopy.material = materialInstance;
        int index = 0;
        foreach (Vector3 point in positionsFromSource)
        {
            lineRendererToCopy.positionCount++;
            lineRendererToCopy.SetPosition(index, point);
            index++;
        }
    }
}
