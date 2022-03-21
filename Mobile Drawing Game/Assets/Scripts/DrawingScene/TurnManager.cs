using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class TurnManager : MonoBehaviour
{
    public bool isMyTurn, isDrawer;
    public PhotonView view;
    public int currentTurn;
    public int[] drawersArray = new int[2];
    public Room CurrentRoom;
    public GameObject confirmButton, clearButton, inkBar, colors;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CurrentRoom = PhotonNetwork.CurrentRoom;
            view.RPC("SyncRoomProperties", RpcTarget.OthersBuffered, 
                (int[]) CurrentRoom.CustomProperties["Drawers"], (int) CurrentRoom.CustomProperties["CurrentTurn"]);
            drawersArray = (int[]) CurrentRoom.CustomProperties["Drawers"];
            currentTurn = (int) CurrentRoom.CustomProperties["CurrentTurn"];
            Debug.Log($"Drawers array: {drawersArray[0]}, {drawersArray[1]}\n" +
                      $"Current turn value: {CurrentRoom.CustomProperties["CurrentTurn"]}");
            view.RPC("IsDrawerCheck", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void SyncRoomProperties(int[] drawersUpdated = null, int currentTurnUpdated = -1)
    {
        if (drawersUpdated != null)
            drawersArray = drawersUpdated;
        if (currentTurnUpdated != -1)
            currentTurn = currentTurnUpdated;
    }

    public void IncrementTurn()
    {
        foreach (var id in drawersArray)
        {
            Debug.Log($"increment loop: {id}");
        }
        isMyTurn = false;
        ToggleUI(false);
        int nextDrawer = Array.IndexOf(drawersArray, currentTurn) + 1 == drawersArray.Length
            ? 0
            : Array.IndexOf(drawersArray, currentTurn) + 1;
        currentTurn = drawersArray[nextDrawer];
        Debug.Log($"Turn changed to {currentTurn} at index: {nextDrawer}");
        view.RPC("SyncRoomProperties", RpcTarget.OthersBuffered, null, currentTurn);
        view.RPC("IsMyTurnCheck", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void IsMyTurnCheck()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == currentTurn)
        {
            Debug.Log("My Turn.");
            isMyTurn = true;
            ToggleUI(true);
        }
    }

    [PunRPC]
    public void IsDrawerCheck()
    {
        foreach(int drawerId in drawersArray)
        {
            Debug.Log($"DrawerID for the current state of the loop: {drawerId}");
            if (PhotonNetwork.LocalPlayer.ActorNumber == drawerId)
            {
                isDrawer = true;
                IsMyTurnCheck();
                break;
            }
        }
    }

    private void ToggleUI(bool toggleTo)
    {
        Debug.Log($"Set UI to {toggleTo}");
        confirmButton.SetActive(toggleTo);
        clearButton.SetActive(toggleTo);
        inkBar.SetActive(toggleTo);
        colors.SetActive(toggleTo);
    }
}
