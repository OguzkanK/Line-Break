using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DrawingSceneLoader : MonoBehaviour
{
    public PhotonView view;
    
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void GoBackToRoomButton()
    {
        PhotonNetwork.LoadLevel("RoomScene");
    }
}
