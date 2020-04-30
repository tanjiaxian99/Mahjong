using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    #region Private Fields

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion.
    /// </summary>
    string gameVersion = "1";

    // #Future Adjust to accommodate spectators
    [SerializeField]
    private byte maxPlayersPerRoom = 4;
    #endregion

    #region MonoBehaviour Callbacks

    void Awake() {
        // All clients in the same room will automatically load the same level as Master Client
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start() {
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster() {
        Debug.Log("Mahjong/Launcher: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinRandomRoom();
    }


    /// <summary>
    /// Called by PUN when JoinRandomRoom succeeds
    /// </summary>
    public override void OnJoinedRoom() {
        Debug.Log("Mahjong/Launcher: OnJoinedRoom() called by PUN. The client is in a room.");
    }


    /// <summary>
    /// Called by PUN when JoinRandomRoom fails
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("Mahjong/Launcher: OnJoinRandomFailed() was called by PUN. No random room available. \nCall PhotonNetwork.CreateRoom");
        // Create a new room with no name
        PhotonNetwork.CreateRoom(null, new RoomOptions {
            MaxPlayers = maxPlayersPerRoom});
    }


    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarningFormat("Mahjong/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Start the connection process
    /// If connected, join a random room. Else, connect to server
    /// </summary>
    public void Connect() {
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRandomRoom();
        } else {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion
}