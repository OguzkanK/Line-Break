using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField CreateInput;
    public TMP_InputField JoinInput;

    public void CreateRoom()
    {
        Debug.Log(CreateInput.text);
        PhotonNetwork.CreateRoom(CreateInput.text);
    }

    public void JoinRoom()
    {
        Debug.Log(JoinInput.text);
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom() //Callback function, automatically called when you join a room
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
