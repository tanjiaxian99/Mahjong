using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks {
    #region Private Fields

    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    #endregion

    #region MonoBehavior Callbacks

    void Start() {
        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 1f, -5.5f), Quaternion.identity, 0);
            assignPlayerWind();
        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    /// <summary>
    /// When the player leaves the room, call the Launcher scene
    /// </summary>
    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }


    #endregion

    #region Private Methods

    // Assign a random player wind to the local player without clashing with the winds of other players
    private void assignPlayerWind() {
        // Retrieve winds of other players
        Player[] playerList = PhotonNetwork.PlayerListOthers;
        List<PlayerManager.Wind> winds = new List<PlayerManager.Wind>();
        foreach (Player player in playerList) {
            winds.Add((PlayerManager.Wind)player.CustomProperties["playerWind"]);
        }
        
        // Ensure local player doesn't have the same wind as another player
        var rand = new System.Random();
        PlayerManager.Wind playerWind;
        while (true) {
            switch (rand.Next(4)) {
                case 0:
                    playerWind = PlayerManager.Wind.EAST;
                    break;
                case 1:
                    playerWind = PlayerManager.Wind.SOUTH;
                    break;
                case 2:
                    playerWind = PlayerManager.Wind.WEST;
                    break;
                default:
                    playerWind = PlayerManager.Wind.NORTH;
                    break;
            }

            if (!winds.Contains(playerWind)) {
                break;
            }
        }

        // Add local player's wind to customProperties
        Hashtable customProperties = new Hashtable();
        customProperties.Add("playerWind", playerWind);
        PhotonNetwork.SetPlayerCustomProperties(customProperties);
        Debug.Log(playerWind);
    }

    #endregion

    #region Public Methods


    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
