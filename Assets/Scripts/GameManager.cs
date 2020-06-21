﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = System.Random;

public class GameManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks {
    #region Private Fields

    [SerializeField]
    private GameObject scriptManager;

    private PlayerManager playerManager;

    public PunTurnManager turnManager;

    [Tooltip("The GameObject used to represent a physical Mahjong table")]
    [SerializeField]
    public GameObject gameTable;

    [Tooltip("Height of the Mahjong table in the local client")]
    [SerializeField]
    public float tableHeight;

    [Tooltip("Width of the Mahjong table in the local client")]
    [SerializeField]
    public float tableWidth;

    [Tooltip("Debugging: The number of players required to start a game")]
    [SerializeField]
    private int numberOfPlayers = 4;

    [Tooltip("The factor by which to scale the tiles such that they fill different screen sizes proportionately")]
    private float scaleFactor = 1f;

    private static readonly Random random = new Random();

    /// <summary>
    /// Dictionary containing a player's actor number and his playerWind in integer form
    /// </summary>
    public Dictionary<int, int> windsDict = new Dictionary<int, int>();

    /// <summary>
    /// The latest tile to be discarded. Null if the player drew a tile
    /// </summary>
    public Tile latestDiscardTile;

    /// <summary>
    /// The number of tiles left in the wall
    /// </summary>
    public int numberOfTilesLeft;

    public Player discardPlayer;

    public Dictionary<string, int> settingsDict;

    /// <summary>
    /// The prevailing wind of the current round
    /// </summary>
    public PlayerManager.Wind prevailingWind = PlayerManager.Wind.EAST;

    public Dictionary<Player, List<Tile>> openTilesDict;

    public List<Tile> discardTiles;

    public Player kongPlayer;

    public Tile latestKongTile;

    public Player bonusPlayer;

    public Tile latestBonusTile;

    public bool isFreshTile;

    #endregion

    #region UI fields

    [SerializeField]
    private GameObject pointsGameObject;

    #endregion

    #region MonoBehavior Callbacks

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start() {
        Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);

        // PunTurnManager settings
        turnManager = this.gameObject.AddComponent<PunTurnManager>();
        turnManager.TurnManagerListener = this;
        this.turnManager.TurnDuration = 1000f;

        this.openTilesDict = new Dictionary<Player, List<Tile>>();
        this.discardTiles = new List<Tile>();

        // Had to be called manually since PhotonNetwork wasn't calling it
        this.OnJoinedRoom();

        // Register customType Tile and List<Tile>
        PhotonPeer.RegisterType(typeof(List<Tile>), 255, CustomTypes.SerializeTilesList, CustomTypes.DeserializeTilesList);
        PhotonPeer.RegisterType(typeof(Tuple<int, Tile, float>), 254, CustomTypes.SerializeDiscardTileInfo, CustomTypes.DeserializeDiscardTileInfo);
    }

    void Update() {
        if (playerManager.myTurn && playerManager.canTouchHandTiles && Input.GetMouseButtonDown(0)) {
            playerManager.OnLocalPlayerMove();
        }

        Text pointsText = pointsGameObject.GetComponent<Text>();
        pointsText.text = playerManager.points + "";
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    /// <summary>
    /// Called by the local player upon joining a room
    /// </summary>
    public override void OnJoinedRoom() {
        // Initialize PlayerManager for local player
        playerManager = scriptManager.GetComponent<PlayerManager>();

        Settings settings = scriptManager.GetComponent<Settings>();
        settingsDict = settings.settingsDict;

        if (PhotonNetwork.CurrentRoom.PlayerCount == numberOfPlayers) {
            // Players that disconnect and reconnect won't start the game at turn 0
            // Game is initialized by MasterClient
            if (this.turnManager.Turn == 0 && PhotonNetwork.IsMasterClient) {
                StartCoroutine(InitializeRound.InitializeGame(this, numberOfPlayers));
            }

        } else {
            Debug.Log("Waiting for another player");
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("A new player has arrived");

        if (PhotonNetwork.CurrentRoom.PlayerCount == numberOfPlayers) {
            if (this.turnManager.Turn == 0 && PhotonNetwork.IsMasterClient) {
                StartCoroutine(InitializeRound.InitializeGame(this, numberOfPlayers));
            }
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.LogFormat("{0} has disconnected! Inactive status: {1}", otherPlayer.NickName, otherPlayer.IsInactive);
    }


    /// <summary>
    /// Call to leave the room
    /// </summary>
    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    /// <summary>
    /// When the player leaves the room, call the Launcher scene
    /// </summary>
    public override void OnLeftRoom() {
        Debug.Log("The local player has left the room");
        SceneManager.LoadScene(0);
    }


    public override void OnDisconnected(DisconnectCause cause) {
        // Implement disconnectedPanel UI
        Debug.LogFormat("Local player has disconnected due to cause {0}", cause);
    }

    #endregion

    /// <summary>
    /// Called by the MasterClient to start a new Turn
    /// </summary>
    public void StartTurn() {
        this.turnManager.BeginTurn();
    }


    /// <summary>
    /// Generates a random number. lock() makes it thread-safe
    /// </summary>
    // https://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
    private static readonly object syncLock = new object();
    public static int RandomNumber(int max) {
        lock (syncLock) { // synchronize
            return random.Next(max);
        }
    }


    /// <summary>
    /// Called by the local player to inform the next player that it is his turn
    /// </summary>
    public void nextPlayersTurn() {
        Player[] playOrder = PropertiesManager.GetPlayOrder();
        int localPlayerPos = 0;
        Player nextPlayer;

        for (int i = 0; i < playOrder.Length; i++) {
            if (playOrder[i] == PhotonNetwork.LocalPlayer) {
                localPlayerPos = i;
                break;
            }
        }

        // If there is only one player, call the local player again
        if (playOrder.Length == 1) {
            nextPlayer = PhotonNetwork.LocalPlayer;
        } else if (localPlayerPos == playOrder.Length - 1) {
            // Call the first player if the local player is the last player in the play order
            nextPlayer = playOrder[0];
        } else {
            nextPlayer = playOrder[localPlayerPos + 1];
        }

        // Update Room Custom Properties with the next player
        PropertiesManager.SetNextPlayer(nextPlayer);
    }


    /// <summary>
    /// Update the allPlayersOpenTiles dictionary
    /// </summary>
    public void UpdateAllPlayersOpenTiles(Player player, List<Tile> openTiles) {
        openTilesDict[player] = openTiles;
    }


    /// <summary>
    /// Returns the list of all open tiles
    /// </summary>
    /// <returns></returns>
    public List<Tile> AllPlayersOpenTiles() {
        List<Tile> allPlayersOpenTiles = new List<Tile>();
        foreach (List<Tile> openTiles in openTilesDict.Values) {
            allPlayersOpenTiles.AddRange(openTiles);
        }
        return allPlayersOpenTiles;
    }


    /// <summary>
    /// Called to end the round
    /// </summary>
    public void EndRound() {
        Debug.LogError("End of Round");
        // TODO
    }

    #region IPunTurnManagerCallbacks Callbacks

    public void OnTurnBegins(int turn) {
    }

    public void OnTurnCompleted(int turn) {
    }

    public void OnPlayerMove(Player player, int turn, object move) {
    }

    public void OnPlayerFinished(Player player, int turn, object move) {
    }

    public void OnTurnTimeEnds(int turn) {
    }

    #endregion
}
