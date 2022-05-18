using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class TurnManager : MonoBehaviour
{
    public TMP_Text TeamNameDrawer, TeamNameGuesser;
    public string currentTeam;
    public bool isMyTurn, isDrawer;
    public PhotonView view;
    public int currentTurn;
    public int[] drawersArray = new int[2];
    public Room CurrentRoom;
    public GameObject drawerUI, guesserUI;

    private void Start(){
        Initialize(true);
    }
    public void Initialize(bool fromStart)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            CurrentRoom = PhotonNetwork.CurrentRoom;
            view.RPC("SyncRoomProperties", RpcTarget.OthersBuffered, 
                (int[]) CurrentRoom.CustomProperties["Drawers"], (int) CurrentRoom.CustomProperties["CurrentTurn"]);
            if(fromStart)
            {
                view.RPC("TeamCheck", RpcTarget.AllBuffered,  (int[]) CurrentRoom.CustomProperties["TeamA"]);
            }
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

    [PunRPC]
    public void TeamCheck(int[] TeamA = null)
    {
        if (TeamA != null)
            if(TeamA.Contains(PhotonNetwork.LocalPlayer.ActorNumber)){
             currentTeam = "A";
            }
            else{
             currentTeam = "B";
            }
    }

    public void IncrementTurn()
    {
        foreach (var id in drawersArray)
        {
            Debug.Log($"increment loop: {id}");
        }
        isMyTurn = false;
        ToggleUI(false, "drawer");
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
            isMyTurn = true;
            ToggleUI(true, "drawer");
        }
    }

    [PunRPC]
    public void IsDrawerCheck()
    {
        foreach(int drawerId in drawersArray)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != drawerId) continue;
            isDrawer = true;
            IsMyTurnCheck();
            break;
        }
        if(!isDrawer)
        {
            ToggleUI(true, "guesser");
            if(currentTeam.Equals("A"))
                TeamNameGuesser.text = $"Team A";
            else
                TeamNameGuesser.text = $"Team B";
        }
        else{
            if(currentTeam.Equals("A"))
                TeamNameDrawer.text = $"Team A";
            else
                TeamNameDrawer.text = $"Team B";
        }
    }

    private void ToggleUI(bool toggleTo, string playerType)
    {
        switch (playerType)
        {
            case "drawer":
                drawerUI.SetActive(toggleTo);
                break;
            case "guesser":
                guesserUI.SetActive(toggleTo);
                break;
        }
    }
}
