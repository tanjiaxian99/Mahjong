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

    /// <summary>
    /// Only true when the player connects to the server for the first time. Prevents calling OnConnectedToMaster()
    /// automatically when leaving a room.
    /// </summary>
    bool firstConnection;

    #endregion

    #region MonoBehaviour Callbacks

    void Awake() {
        // All clients in the same room will automatically load the same level as Master Client
        PhotonNetwork.AutomaticallySyncScene = true;
    }


    void Start() {
        // Show name inputField and 'Play' button
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        // For debugging
        this.Connect();
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster() {
        Debug.Log("Mahjong/Launcher: OnConnectedToMaster() was called by PUN");

        // Automatically connect to a random room when firstConnection is true, which is when the player
        // connects to the Photon server for the first time.
        if (firstConnection) {
            // If numberOfPlayersToStart == 1 and a second client calls JoinRandomRoom, an exception is raised:
            // Deserialization failed for Operation Response. System.Exception: Read failed. Custom type not found: 255.
            PhotonNetwork.JoinRandomRoom();
            firstConnection = false;
        }
    }


    /// <summary>
    /// Called by PUN when JoinRandomRoom succeeds
    /// </summary>
    public override void OnJoinedRoom() {
        Debug.Log("Mahjong/Launcher: OnJoinedRoom() called by PUN. The client is in a room.");
        // Let the first player load the room. Every other player will sync with the loaded level 
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            Debug.Log("We load the GameRoom");
            // Load the Room Level.
            PhotonNetwork.LoadLevel("GameRoom");
        }
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
        // Turn off 'connecting...' progressLabel
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        // Restart the firstConnection
        firstConnection = false;
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

        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRandomRoom();
        } else {
            firstConnection = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion
}