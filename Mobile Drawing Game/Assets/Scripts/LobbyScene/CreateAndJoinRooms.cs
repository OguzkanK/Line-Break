using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField CreateInput; // Create room name input field
    public TMP_InputField JoinInput; // Join room name input field

    public void CreateRoom() // Use CreateRoom method from PhotonNetwork to create a room, set the user to host
    {
        PlayerPrefs.SetInt("isHost", 1);
        PhotonNetwork.CreateRoom(CreateInput.text);
    }

    public void JoinRoom()// Use JoinRoom method from PhotonNetwork to join a room, set the user to joined
    {
        Debug.Log(JoinInput.text);
        PlayerPrefs.SetInt("isHost", 0);
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom() // Callback function, automatically called when you join a room
    {
        PhotonNetwork.LoadLevel("RoomScene");
    }
}
