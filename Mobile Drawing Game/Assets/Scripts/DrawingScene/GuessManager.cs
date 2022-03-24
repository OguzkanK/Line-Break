using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GuessManager : MonoBehaviour
{
    // TODO:
    // Limit the input to 10 characters
    // Make the input field ignore space key (also the other input fields)
    
    public TMP_Text guessPlaceholder, guessInputText, selectedWord, winnerName;
    public TMP_InputField guessInputField;
    public PhotonView view;
    public int maxGuesses = 10;
    public GameObject guessPanel, textObject, 
        gameUI, endingUI, goBackToRoom; // Area where guesses appear, guess text prefab
    public TurnManager turnManager;
    private float _changeSizeFontTo = Screen.height / 40;
    private string _username;
    private List<GuessMessage> _guessList = new List<GuessMessage>();
    void Start()
    {
        guessInputField.characterLimit = 75;
        _username = PlayerPrefs.GetString("playerUsername");
        guessPlaceholder.fontSize = _changeSizeFontTo;
        guessInputText.fontSize = _changeSizeFontTo;
    }
    
    public void SendButton() // Function for to call RPC method with the send button
    {
        if (guessInputField.text == "") return;
        if (guessInputField.text.Equals(selectedWord.text, StringComparison.OrdinalIgnoreCase))
        {
            view.RPC("GuessedRight", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
        else
        {
            view.RPC("SendGuess", RpcTarget.AllBuffered, _username + ": " + guessInputField.text);
        }
        // Reset input field after sending the message
        guessInputField.text = "";
    }

    [PunRPC]
    public void GuessedRight(string winningPlayer)
    {
        // Make the game enter the win state, let every player see the stat screen. only the host can make everyone continue.
        // Give points to whoever guessed, and drawers
        // Return to room scene
        Debug.Log(winningPlayer);
        gameUI.SetActive(false);
        endingUI.SetActive(true);
        winnerName.text = winningPlayer;
        if (turnManager.isDrawer)
        {
            turnManager.isDrawer = false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            goBackToRoom.SetActive(true);
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
}



[System.Serializable]
public class GuessMessage // Guess Message class
{
    public string text;
    public TMP_Text guessTextObject;
}
