using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class DrawingSceneLoader : MonoBehaviour
{
    public PhotonView view;
    public int[] teamA = new int[2];
    public int[] teamB = new int[2];
    public Room CurrentRoom;

    void Start()
    {
        CurrentRoom = PhotonNetwork.CurrentRoom;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            view.RPC("SetTeams", RpcTarget.AllBuffered,  (int[]) CurrentRoom.CustomProperties["TeamA"], (int[]) CurrentRoom.CustomProperties["TeamB"]);
        }
    }

    public void GoBackToRoomButton()
    {
        PhotonNetwork.LoadLevel("RoomScene");
    }

    [PunRPC]
    public void SetTeams(int[] teamAInput, int[] teamBInput){
        teamA = teamAInput;
        teamB = teamBInput;
    }

    [PunRPC]
    public void StartNextRound(int[] teamAInput, int[] teamBInput, int[] drawers){
        int[] newDrawers = new int[2];
        if(teamAInput[0] == drawers[0]){
            Debug.Log($"In if: Team A in 1 is {teamAInput[0]} \nDrawer 1 is {drawers[0]}.");
            newDrawers[0] = teamAInput[1];
        }
        else{
            Debug.Log($"In else: Team A in 2 is {teamAInput[1]} \nDrawer 1 is {drawers[0]}.");
            newDrawers[0] = teamAInput[0];
        }
        if(teamBInput[0] == drawers[1]){
            newDrawers[1] = teamBInput[1];
        }
        else{
            newDrawers[1] = teamBInput[0];
        }
        int orderRandomizer = UnityEngine.Random.Range(0, 1);
        for(int i = 0; i < 2; i++){
            Debug.Log($"Team A in {i + 1} is {teamAInput[i]} \nTeam B in {i + 1} is {teamBInput[i]}.");
        }
        for(int i = 0; i < 2; i++){
            Debug.Log($"Drawers {i + 1} is {newDrawers[i]}.");
        }

        CurrentRoom.CustomProperties["Drawers"] = newDrawers;
        CurrentRoom.CustomProperties["CurrentTurn"] = newDrawers[orderRandomizer];
            
        PhotonNetwork.LoadLevel("DrawingScene");
        
    }

    public void StartNextRoundButtonHandler(){
        view.RPC("StartNextRound", RpcTarget.AllBuffered, (int[]) CurrentRoom.CustomProperties["TeamA"], (int[]) CurrentRoom.CustomProperties["TeamB"],
        (int[]) CurrentRoom.CustomProperties["Drawers"]);
    }
}
