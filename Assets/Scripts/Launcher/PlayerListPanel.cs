using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private Transform playerListPanel;

    [SerializeField]
    private PlayerListing playerPrefab;

    private List<PlayerListing> playerList;

    #region Singleton Initialization

    private static PlayerListPanel _instance;

    public static PlayerListPanel Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        playerList = new List<PlayerListing>();
    }

    #endregion

    public void SetInitialPlayerList() {
        Player[] playerList = PhotonNetwork.PlayerList;
        foreach (Player player in playerList) {
            OnPlayerEnteredRoom(player);
        }    
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        PlayerListing playerListing = Instantiate(playerPrefab, playerListPanel);
        if (this.playerList != null) {
            playerListing.SetPlayerInfo(newPlayer);
            this.playerList.Add(playerListing);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        int index = playerList.FindIndex(x => x.Player == otherPlayer);
        if (index != -1) {
            Destroy(playerList[index].gameObject);
            playerList.RemoveAt(index);
        }
    }
}
