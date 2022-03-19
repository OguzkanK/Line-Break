using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab; // Player prefab
    public Transform spawnPlayersTransform; // Spawn point

    private void Start()
    {
        // Create and set spawn position based on number of players
        Vector2 spawnPosition;
        var position = spawnPlayersTransform.position;
        
        spawnPosition.x = position.x + (PhotonNetwork.CurrentRoom.PlayerCount - 1) * 0.5f;
        spawnPosition.y = position.y;
        
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
