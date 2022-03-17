using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameSceneCheck : MonoBehaviour
{
    private PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var playersName in PhotonNetwork.PlayerList)
        {
            view = GetComponent<PhotonView>();
            //tell use each player who is in the room
            view.RPC("Chime", RpcTarget.All);
        }
    }

    [PunRPC]
    void Chime()
    {
        foreach (var playersName in PhotonNetwork.PlayerList)
        {
            //tell use each player who is in the room
            Debug.Log(playersName.NickName + " is in the room");
        }
    }
}
