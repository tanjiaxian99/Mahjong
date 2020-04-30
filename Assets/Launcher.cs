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

    #endregion

    #region MonoBehaviour Callbacks

    void Awake() {
        // All clients in the same room will automatically load the same level as Master Client
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start() {
        Connect();
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster() {
        Debug.Log("Mahjong/Launcher: OnConnectedToMaster() was called by PUN");
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