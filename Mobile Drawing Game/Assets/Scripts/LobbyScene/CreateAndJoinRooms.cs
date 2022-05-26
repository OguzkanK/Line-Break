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
    public TMP_InputField CreateInput; 
    public TMP_InputField JoinInput;

    private void Start()
    {
        CreateInput.characterLimit = 10;
        JoinInput.characterLimit = 10;
    }

    public void CreateRoom()
    {
        if (CreateInput.text.Length < 4) return;
        
        _customProperties.Add("Drawers", new int[] { });
        _customProperties.Add("TeamA", new int[] { });
        _customProperties.Add("TeamB", new int[] { });
        _customProperties.Add("TeamAScore", 0);
        _customProperties.Add("TeamBScore", 0);
        _customProperties.Add("CurrentTurn", 0);
        _roomOptions.CustomRoomProperties = _customProperties;
        _roomOptions.MaxPlayers = 4;

        _roomOptions.PlayerTtl = 300;
        _roomOptions.EmptyRoomTtl = 300;
        PhotonNetwork.CreateRoom(CreateInput.text, _roomOptions);
    }

    public void JoinRoom()
    {
        Debug.Log(JoinInput.text);
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("RoomScene");
    }
}
