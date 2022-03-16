using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerListManager : MonoBehaviour
{
    public GameObject listPanel, textObject;
    public Color hostColor, joinedColor, otherColor; 
    
    private PhotonView _view;
    
    [SerializeField] private List<Player> playerList = new List<Player>();

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        _view.RPC("AddToTheList", RpcTarget.AllBuffered,
            PhotonNetwork.NickName, PhotonNetwork.LocalPlayer.ActorNumber,
            PlayerPrefs.GetInt("isHost"));
    }
    
    [PunRPC]
    public void AddToTheList(string username, int playerId, int isHost)
    {
        var player = new Player(username, playerId, isHost);
        
        GameObject newText = Instantiate(textObject, listPanel.transform);
            
        player.TextObject = newText.GetComponent<TMP_Text>();
        player.TextObject.text = player.ID + " - " + player.Username;
        player.TextObject.color = PlayerColor(player.playerType);
        
        playerList.Add(player);
    }
    
    public Color PlayerColor(Player.PlayerType playerType)
    {
        Color color = playerType switch
        {
            Player.PlayerType.Host => hostColor,
            Player.PlayerType.Joined => joinedColor,
            _ => otherColor
        };
        return color;
    }
}

[System.Serializable]
public class Player
{
    public string Username;
    public int ID;
    public TMP_Text TextObject;
    public PlayerType playerType;

    public enum PlayerType
    {
        Host,
        Joined,
        Other
    }

    public Player(string username, int id, int isHost)
    {
        this.Username = username;
        this.ID = id;
        this.playerType = isHost switch
        {
            0 => PlayerType.Joined,
            1 => PlayerType.Host,
            _ => PlayerType.Other
        };
    }
    
}
