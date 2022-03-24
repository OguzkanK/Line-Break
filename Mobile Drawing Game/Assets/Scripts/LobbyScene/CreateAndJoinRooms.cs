using System;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    private Hashtable _customProperties = new Hashtable();
    private Photon.Realtime.RoomOptions _roomOptions = new Photon.Realtime.RoomOptions(); 
    public TMP_InputField CreateInput; // Create room name input field
    public TMP_InputField JoinInput; // Join room name input field

    private void Start()
    {
        CreateInput.characterLimit = 10;
        JoinInput.characterLimit = 10;
    }

    public void CreateRoom() // Use CreateRoom method from PhotonNetwork to create a room, set the user to host
    {
        _customProperties.Add("Drawers", new int[]{});
        _customProperties.Add("CurrentTurn", 0);
        _roomOptions.CustomRoomProperties = _customProperties;
        _roomOptions.MaxPlayers = 6;
        _roomOptions.PlayerTtl = 300;
        _roomOptions.EmptyRoomTtl = 300;
        PhotonNetwork.CreateRoom(CreateInput.text, _roomOptions);
    }

    public void JoinRoom()// Use JoinRoom method from PhotonNetwork to join a room, set the user to joined
    {
        Debug.Log(JoinInput.text);
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom() // Callback function, automatically called when you join a room
    {
        PhotonNetwork.LoadLevel("RoomScene");
    }
}
