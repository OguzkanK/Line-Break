using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPlayersTransform;
    public TMP_Text lobbyName;

    private void Start()
    {
        Vector2 spawnPosition;
        var position = spawnPlayersTransform.position;
        
        spawnPosition.x = position.x + (PhotonNetwork.CurrentRoom.PlayerCount - 1) * 0.5f;
        spawnPosition.y = position.y;

        lobbyName.text = PhotonNetwork.CurrentRoom.Name;
        
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
