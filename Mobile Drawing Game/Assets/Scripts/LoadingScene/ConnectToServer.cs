using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // Connect to the server
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = PlayerPrefs.GetString("playerUsername");
    }

    public override void OnConnectedToMaster() // Callback function, automatically called when you connect to the server
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() // Callback function, automatically called when you join the lobby
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
