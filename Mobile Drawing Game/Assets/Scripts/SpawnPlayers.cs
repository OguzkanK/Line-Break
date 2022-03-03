using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPlayersTransform;

    private void Start()
    {
        Vector2 spawnPosition;
        spawnPosition.x = spawnPlayersTransform.position.x + (PhotonNetwork.CurrentRoom.PlayerCount - 1) * 0.5f;
        spawnPosition.y = spawnPlayersTransform.position.y;
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}
