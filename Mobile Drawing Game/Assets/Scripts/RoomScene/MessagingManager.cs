using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;

public class MessagingManager : MonoBehaviour
{
    public String username;
    public int maxMessages = 10;
    public TMP_InputField chatInput;
    public GameObject chatPanel, textObject;
    public TMP_Text placeholderText, inputText;
    public Color playerMessageColor, infoColor;
    
    [SerializeField] private PhotonView view;
    
    private float _changeSizeFontTo = Screen.height / 40;
    [SerializeField]
    List<Message> messageList = new List<Message>();
    
    private void Start()
    {
        chatInput.characterLimit = 75;
        placeholderText.fontSize = _changeSizeFontTo;
        inputText.fontSize = _changeSizeFontTo;
        username = PhotonNetwork.NickName;
        view.RPC("SendMessageToChat", RpcTarget.AllBuffered, username + " Joined!", Message.MessageType.Info);
    }

    public void SendButton()
    {
        if (chatInput.text == "") return;
        view.RPC("SendMessageToChat", RpcTarget.AllBuffered, username + ": " + chatInput.text, Message.MessageType.PlayerMessage);
        chatInput.text = "";
    }
    
    [PunRPC]
    public void SendMessageToChat(String text, Message.MessageType messageType)
    {
        if(messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].messageTextObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        
        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, chatPanel.transform);
            
        newMessage.messageTextObject = newText.GetComponent<TMP_Text>();
        newMessage.messageTextObject.text = newMessage.text;
        newMessage.messageTextObject.fontSize = _changeSizeFontTo;
        newMessage.messageTextObject.color = MessageTypeColor(messageType);
        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = messageType switch
        {
            Message.MessageType.PlayerMessage => playerMessageColor,
            _ => infoColor
        };
        return color;
    }
}

[System.Serializable]
public class Message
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