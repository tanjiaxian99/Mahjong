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

    public List<PlayerListing> playerList;

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

    private void Update() {
        UpdateHostStar();
    }

    public void UpdateHostStar() {
        foreach (Transform child in playerListPanel) {
            PlayerListing playerListing = child.GetComponent<PlayerListing>();
            Player masterClient = PhotonNetwork.MasterClient;

            if (playerListing.Player == masterClient) {
                playerListing.ShowHostStar(true);
            } else {
                playerListing.ShowHostStar(false);
            }
        }
    }

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

    public void ClearPlayerList() {
        playerList.Clear();
        foreach (Transform child in playerListPanel) {
            Destroy(child.gameObject);
        }
    }
}
