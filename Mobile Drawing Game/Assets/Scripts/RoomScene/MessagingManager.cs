using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;

public class MessagingManager : MonoBehaviour
{
    public String username; // Players username
    public int maxMessages = 10; // Max number of messages
    public TMP_InputField chatInput; // Chat input field
    public GameObject chatPanel, textObject; // Area where messages appear, message text prefab
    public TMP_Text placeholderText, inputText;
    public Color playerMessageColor, infoColor; // Colors for message types
    
    [SerializeField] private PhotonView view; // Photon view
    
    private float _changeSizeFontTo = Screen.height / 40;
    [SerializeField]
    List<Message> messageList = new List<Message>(); // List of current max amount of messages
    
    private void Start()
    {
        chatInput.characterLimit = 75;
        placeholderText.fontSize = _changeSizeFontTo;
        inputText.fontSize = _changeSizeFontTo;
        username = PhotonNetwork.NickName; // Get Nickname from Photon Network
        // User arrived message
        view.RPC("SendMessageToChat", RpcTarget.AllBuffered, username + " Joined!", Message.MessageType.Info);
    }

    public void SendButton() // Function for to call RPC method with the send button
    {
        if (chatInput.text == "") return;
        view.RPC("SendMessageToChat", RpcTarget.AllBuffered, username + ": " + chatInput.text, Message.MessageType.PlayerMessage);
        // Reset input field after sending the message
        chatInput.text = "";
    }
    
    [PunRPC] // RPC method for sending messages
    public void SendMessageToChat(String text, Message.MessageType messageType)
    {
        if(messageList.Count >= maxMessages) // Destroy last message if the max amount is reached
        {
            Destroy(messageList[0].messageTextObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        
        // Create a new Message object
        var newMessage = new Message {text = text};
        
        // Instantiate the text prefab under content
        var newText = Instantiate(textObject, chatPanel.transform);
            
        newMessage.messageTextObject = newText.GetComponent<TMP_Text>();
        newMessage.messageTextObject.text = newMessage.text;
        newMessage.messageTextObject.fontSize = _changeSizeFontTo;
        newMessage.messageTextObject.color = MessageTypeColor(messageType);
        
        // Add the message to the list
        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        // Create a color variable and assign the color with a switch expression and return it
        Color color = messageType switch
        {
            Message.MessageType.PlayerMessage => playerMessageColor,
            _ => infoColor
        };
        return color;
    }
}

[System.Serializable]
public class Message // Message class
{
    public string text;
    public TMP_Text messageTextObject;
    public MessageType messageType;
    
    public enum MessageType
    {
        PlayerMessage,
        Info
    }
}