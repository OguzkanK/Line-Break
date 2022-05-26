using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class RoomSceneLoader : MonoBehaviour
{
    public Room CurrentRoom;
    public Toggle setToRandomize;
    public PhotonView view;
    private void Start()
    {
        CurrentRoom = PhotonNetwork.CurrentRoom;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void teamSetTrigger(){
        setToRandomize.interactable = true;
    }
    
    public void StartGame()
    {
        int[] TeamA = {-1, -1};
        int[] TeamB = {-1, -1};
        int TeamAScore = 0;
        int TeamBScore = 0;
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 4)
        {
            if(setToRandomize.isOn)
            {
                int[] randomTwoPlayers = GetRandomTwoPlayers();
                for(int i = 1; i < PhotonNetwork.PlayerList.Length + 1; i++){
                    if(randomTwoPlayers.Contains(i)){
                        if(TeamA[0] == -1)
                            TeamA[0] = i;
                        else
                            TeamA[1] = i;
                    }
                    else{
                        if(TeamB[0] == -1)
                            TeamB[0] = i;
                        else
                            TeamB[1] = i;;
                    }
                }
                int drawerRandomizer = Random.Range(0, 1);
                int[] Drawers = new int[]{TeamA[drawerRandomizer], TeamB[drawerRandomizer]};

                CurrentRoom.CustomProperties["TeamA"] = TeamA;
                CurrentRoom.CustomProperties["TeamB"] = TeamB;
                CurrentRoom.CustomProperties["Drawers"] = Drawers;
                CurrentRoom.CustomProperties["CurrentTurn"] = Drawers[0];
            }

            
            view.RPC("resetScores", RpcTarget.AllBuffered, (int) CurrentRoom.CustomProperties["TeamAScore"], (int) CurrentRoom.CustomProperties["TeamBScore"]);
            PhotonNetwork.LoadLevel("DrawingScene");
        }  
    }

    [PunRPC]
    public void resetScores(int teamAScore, int teamBScore){
        Debug.Log($"Team A Score Before: {teamAScore}, \nTeam B Score Before: {teamBScore}");
        Debug.Log($"Team A Score Property Before: {CurrentRoom.CustomProperties["TeamAScore"]}, \nTeam B Score Before: {CurrentRoom.CustomProperties["TeamBScore"]}");
        teamAScore = 0;
        teamBScore = 0;
        if (PhotonNetwork.LocalPlayer.IsMasterClient){
            CurrentRoom.CustomProperties["TeamAScore"] = teamAScore;
            CurrentRoom.CustomProperties["TeamBScore"] = teamBScore;
        }
        Debug.Log($"Team A Score After: {teamAScore}, Team \nB Score After: {teamBScore}");
        Debug.Log($"Team A Score Property After: {CurrentRoom.CustomProperties["TeamAScore"]}, Team \nB Score Property After: {CurrentRoom.CustomProperties["TeamBScore"]}");
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
