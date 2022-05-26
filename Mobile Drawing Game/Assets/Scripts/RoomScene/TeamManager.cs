using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class TeamManager : MonoBehaviour
{
    public TMP_Dropdown[] playerTeamSelections = new TMP_Dropdown[4];
    public TMP_Text[] playerTeamLabels = new TMP_Text[4], viewportTemplateLabels = new TMP_Text[4];
    public int[] playerIDs = new int[4];
    public TMP_Text setTeamText;
    public Button setTeamButton;
    public Room CurrentRoom;
    public RoomSceneLoader roomSceneLoader;
    private float _changeSizeFontTo = Screen.height / 40;
    
    private void Start(){
        CurrentRoom = PhotonNetwork.CurrentRoom;
        for(int i = 0; i < 4; i++){
            viewportTemplateLabels[i].fontSize = _changeSizeFontTo;
        }
    }

    public void SetPlayers(string[] playerNames, int[] playerIDsInput){
         if(PhotonNetwork.LocalPlayer.IsMasterClient){
            for(int i = 0; i < 4; i++){
                playerTeamLabels[i].text = playerNames[i];
                playerIDs[i] = playerIDsInput[i];
            }
         }
    }
    public void validityCheck(){
        int NumberOfA = 0, NumberOfB = 0;
        for(int i = 0; i < 4; i++){
            if(playerTeamSelections[i].value == 0){
                NumberOfA++;
            }
            else
            {
                NumberOfB++;
            }
        }
        if(NumberOfA == 2 && NumberOfB == 2){
            setTeamButton.interactable = true;
            setTeamText.color = new Color32(217, 151, 34, 255);
        }
        else{
            setTeamButton.interactable = false;
            setTeamText.color = new Color32(82, 82, 82, 255); 
        }
    }
    
    public void setTeamsButtonHandler(){
        int[] teamA = new int[2];
        int[] teamB = new int[2];
        teamA[0] = -1;
        teamB[0] = -1;
        int[] drawers = new int[2];

        for(int i = 0; i < 4; i++){
            if(playerTeamSelections[i].value == 0){
                if(teamA[0] == -1){
                    teamA[0] = playerIDs[i];
                }
                else{
                    teamA[1] = playerIDs[i];
                }
            }
            else
            {
                if(teamB[0] == -1){
                    teamB[0] = playerIDs[i];
                }
                else{
                    teamB[1] = playerIDs[i];
                }
            }
        }
        drawers[0] = teamA[0];
        drawers[1] = teamB[0];
        
        CurrentRoom.CustomProperties["TeamA"] = teamA;
        CurrentRoom.CustomProperties["TeamB"] = teamB;
        CurrentRoom.CustomProperties["Drawers"] = drawers;
        CurrentRoom.CustomProperties["CurrentTurn"] = drawers[0];
        Debug.Log($"Team A: {teamA[0]}, {teamA[1]} \nTeam B: {teamB[0]}, {teamB[1]} ");
        roomSceneLoader.setToRandomize.isOn = false;
    }

    
}
