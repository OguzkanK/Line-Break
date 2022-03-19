using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("DrawingScene");
        }  
    }
}
