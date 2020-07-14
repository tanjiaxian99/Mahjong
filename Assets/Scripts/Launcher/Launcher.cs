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

    [Tooltip("The Control Panel that allows the player to enter a name and click play")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the player that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    [SerializeField]
    private GameObject roomListPanel;

    [SerializeField]
    private GameObject createRoomPanel;

    [SerializeField]
    private GameObject roomPanel;

    #endregion

    #region MonoBehaviour Callbacks

    void Awake() {
        // All clients in the same room will automatically load the same level as Master Client
        PhotonNetwork.AutomaticallySyncScene = true;
        DefaultUI();
    }

    void Start() {
        // Show name inputField and 'Play' button
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        //// DEBUG
        //this.Connect();
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster() {
        Debug.Log("Mahjong/Launcher: OnConnectedToMaster() was called by PUN");
    }

    /// <summary>
    /// Called by PUN when JoinRandomRoom succeeds
    /// </summary>
    public override void OnJoinedRoom() {
        Debug.Log("Mahjong/Launcher: OnJoinedRoom() called by PUN. The client is in a room.");

        createRoomPanel.SetActive(false);
        roomPanel.SetActive(true);

        //// Let the first player load the room. Every other player will sync with the loaded level 
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
        //    Debug.Log("We load the GameRoom");
        //    PhotonNetwork.LoadLevel("GameRoom");
        //}
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarningFormat("Mahjong/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        // Turn off 'connecting...' progressLabel
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Start the connection process upon clicking the 'Play' Button
    /// If connected, join a random room. Else, connect to server
    /// </summary>
    public void Connect() {
        // Show 'connecting... progressLabel'
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }

        progressLabel.SetActive(false);
        roomListPanel.SetActive(true);
    }

    #endregion

    #region Private Methods

    private void DefaultUI() {
        controlPanel.SetActive(true);
        progressLabel.SetActive(false);
        roomListPanel.SetActive(false);
    }

    #endregion
}