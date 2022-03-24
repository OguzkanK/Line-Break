using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerListManager : MonoBehaviour
{
    public GameObject listPanel, textObject;
    
    [SerializeField] private List<Player> playerList = new List<Player>();
    [SerializeField] private PhotonView view;
    private bool _isHost;
    

    private void Start()
    {
        _isHost = PhotonNetwork.IsMasterClient;
        view.RPC("AddToTheList", RpcTarget.AllBuffered,
            PhotonNetwork.NickName, PhotonNetwork.LocalPlayer.ActorNumber, _isHost);
    }
    
    [PunRPC]
    public void AddToTheList(string username, int playerId, bool isHost)
    {
        var player = new Player(username, playerId, isHost);
        
        GameObject newText = Instantiate(textObject, listPanel.transform);
            
        player.textObject = newText.GetComponent<TMP_Text>();
        player.textObject.text = player.id + " - " + player.username;
        player.textObject.fontSize = (Screen.height / 20);
        player.textObject.color = PlayerColor(player.playerType);
        
        playerList.Add(player);
    }
    
    private Color PlayerColor(Player.PlayerType playerType)
    {
        Color color = playerType switch
        {
            Player.PlayerType.Host => new Color32(0, 255, 68, 255),
            Player.PlayerType.Joined => new Color32(0, 68, 255, 255),
            _ => new Color32(80, 19, 212, 255)
        };
        
        return color;
    }
}

[System.Serializable]
public class Player
{
    public string username;
    public int id;
    public TMP_Text textObject;
    public PlayerType playerType;

    public enum PlayerType
    {
        Host,
        Joined
    }

    public Player(string username, int id, bool isHost)
    {
        this.username = username;
        this.id = id;
        this.playerType = isHost switch
        {
            false => PlayerType.Joined,
            true => PlayerType.Host
        };
    }
    
}
