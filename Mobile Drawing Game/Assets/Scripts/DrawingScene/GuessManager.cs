using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GuessManager : MonoBehaviour
{
    // TODO:
    // Limit the input to 10 characters
    // Make the input field ignore space key (also the other input fields)
    
    public TMP_Text guessPlaceholder, guessInputText, selectedWord, winnerName, teamAScoreText, teamBScoreText;
    public TMP_InputField guessInputField;
    public PhotonView view;
    public Room CurrentRoom;
    public string currentTeam;
    public int maxGuesses = 10;
    public int teamAScore, teamBScore;
    public GameObject guessPanel, textObject, 
        gameUI, endingUI, goBackToRoom, startNextRound; // Area where guesses appear, guess text prefab
    public TurnManager turnManager;
    private float _changeSizeFontTo = Screen.height / 40;
    private string _username;
    private List<GuessMessage> _guessList = new List<GuessMessage>();
    void Start()
    {
        CurrentRoom = PhotonNetwork.CurrentRoom;
        guessInputField.characterLimit = 75;
        _username = PlayerPrefs.GetString("playerUsername");
        guessPlaceholder.fontSize = _changeSizeFontTo;
        guessInputText.fontSize = _changeSizeFontTo;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            view.RPC("TeamCheck", RpcTarget.AllBuffered,  (int[]) CurrentRoom.CustomProperties["TeamA"]);
            view.RPC("setScores", RpcTarget.AllBuffered, (int) CurrentRoom.CustomProperties["TeamAScore"], (int) CurrentRoom.CustomProperties["TeamBScore"]);
        }
    }
    
    public void SendButton() // Function for to call RPC method with the send button
    {
        if (guessInputField.text == "") return;
        if (guessInputField.text.Equals(selectedWord.text, StringComparison.OrdinalIgnoreCase))
        {
            view.RPC("GuessedRight", RpcTarget.AllBuffered, currentTeam, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            view.RPC("SendGuess", RpcTarget.AllBuffered, _username + ": " + guessInputField.text);
        }
        // Reset input field after sending the message
        guessInputField.text = "";
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

    [PunRPC]
    public void GuessedRight(string winningTeam, int winningPlayerId)
    {
        // Make the game enter the win state, let every player see the stat screen. only the host can make everyone continue.
        // Give points to whoever guessed, and drawers
        // Return to room scene
        Debug.Log($"Team A Score Property Before: {CurrentRoom.CustomProperties["TeamAScore"]}, \nTeam B Score Before: {CurrentRoom.CustomProperties["TeamBScore"]}");
        if(winningTeam.Equals("A")){
            teamAScore = teamAScore + 1;
            CurrentRoom.CustomProperties["TeamAScore"] = teamAScore;
            winnerName.text = $"Team A";
            Debug.Log($"Team A score: {teamAScore} \nTeam B score: {teamBScore}");
        }
        else{
            teamBScore = teamBScore + 1;
            CurrentRoom.CustomProperties["TeamBScore"] = teamBScore;
            winnerName.text = $"Team B";
            Debug.Log($"Team A score: {teamAScore} \nTeam B score: {teamBScore}");
        }
        
        Debug.Log($"Team A Score Property After: {CurrentRoom.CustomProperties["TeamAScore"]}, \nTeam B Score After: {CurrentRoom.CustomProperties["TeamBScore"]}");
        
        teamAScoreText.text = $"Team A: {teamAScore}";
        teamBScoreText.text = $"Team B: {teamBScore}";
        gameUI.SetActive(false);
        endingUI.SetActive(true);
        //winnerName.text = winningPlayer;
        if (turnManager.isDrawer)
        {
            turnManager.isDrawer = false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            goBackToRoom.SetActive(true);
            startNextRound.SetActive(true);
        }
    }
    
    [PunRPC] // RPC method for sending guesses
    public void SendGuess(string text)
    {
        if(_guessList.Count >= maxGuesses) // Destroy last message if the max amount is reached
        {
            Destroy(_guessList[0].guessTextObject.gameObject);
            _guessList.Remove(_guessList[0]);
        }
        
        // Create a new GuessMessage object
        var newGuess = new GuessMessage {text = text};
        
        // Instantiate the text prefab under content
        var newText = Instantiate(textObject, guessPanel.transform);
            
        newGuess.guessTextObject = newText.GetComponent<TMP_Text>();
        newGuess.guessTextObject.text = newGuess.text;
        newGuess.guessTextObject.fontSize = _changeSizeFontTo;
        
        // Add the message to the list
        _guessList.Add(newGuess);
    }

    [PunRPC]
    public void setScores(int teamAScoreProperty, int teamBScoreProperty){
        teamAScore = teamAScoreProperty;
        teamBScore = teamBScoreProperty;
    }
}



[System.Serializable]
public class GuessMessage // Guess Message class
{
    public string text;
    public TMP_Text guessTextObject;
}
