using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class RoomSceneLoader : MonoBehaviour
{
    public Room CurrentRoom;
    private void Start()
    {
        CurrentRoom = PhotonNetwork.CurrentRoom;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 2)
        {
            int[] randomTwoPlayers = GetRandomTwoPlayers();
            CurrentRoom.CustomProperties["Drawers"] = randomTwoPlayers;
            CurrentRoom.CustomProperties["CurrentTurn"] = randomTwoPlayers[0];
            PhotonNetwork.LoadLevel("DrawingScene");
        }  
    }

    public int[] GetRandomTwoPlayers()
    {
        int[] returnArray = new int[2]
        {
            Random.Range(1, PhotonNetwork.PlayerList.Length + 1),
            Random.Range(1, PhotonNetwork.PlayerList.Length + 1)
        };
        while (returnArray[0] == returnArray[1])
        {
            returnArray[1] = Random.Range(1, PhotonNetwork.PlayerList.Length + 1);
        }
        return returnArray;
    }
}
