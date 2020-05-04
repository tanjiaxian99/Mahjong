using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

using Random = System.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks, IOnEventCallback {
    #region Private Fields

    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    private PlayerManager playerManager;

    [Tooltip("The GameObject used to represent a physical Mahjong table")]
    [SerializeField]
    private GameObject gameTable;

    [Tooltip("Debugging: The number of players required to start a game")]
    [SerializeField]
    private int numberOfPlayersToStart = 4;

    private PunTurnManager turnManager;

    #endregion

    #region OnEvent Fields

    /// <summary>
    /// The Wind Assignment event message byte. Used internally for saving data in Player Custom Properties
    /// </summary>
    public const byte EvAssignWind = 1;

    /// <summary>
    /// The Wind Assignment event message byte. Used internally for saving data in Player Custom Properties
    /// </summary>
    //public static readonly byte Ev

    /// <summary>
    /// Wind of the player
    /// </summary>
    public static readonly string AssignWindPropKey = "pW";

    #endregion


    #region MonoBehavior Callbacks

    void Start() {
        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
            
            // PunTurnManager settings
            turnManager = this.gameObject.AddComponent<PunTurnManager>();
            turnManager.TurnManagerListener = this;
            this.turnManager.TurnDuration = 1000f;

        }
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    // Register the OnEvent callback handler
    public override void OnEnable() {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }


    // Remove the OnEvent callback handler
    public override void OnDisable() {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    /// <summary>
    /// Called by the local player upon joining a room
    /// </summary>
    public override void OnJoinedRoom() {
        // Move the player to his seat based on his wind
        MoveToWindSeat(playerWind);

        // Change the width of the gameTable based on the player's seat
        StretchGameTable(playerWind);

        // Initialize PlayerManager for local player
        playerManager = playerPrefab.GetComponent<PlayerManager>();

        if (PhotonNetwork.CurrentRoom.PlayerCount == numberOfPlayersToStart) {
            // Players that disconnect and reconnect won't start the game at turn 0
            // Game is initialized by MasterClient
            if (this.turnManager.Turn == 0 && PhotonNetwork.IsMasterClient) {
                this.InitializeGame();
                this.StartTurn();
            }

        } else {
            Debug.Log("Waiting for another player");
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("A new player has arrived");

        if (PhotonNetwork.CurrentRoom.PlayerCount == numberOfPlayersToStart) {
            if (this.turnManager.Turn == 0 && PhotonNetwork.IsMasterClient) {
                this.InitializeGame();
                this.StartTurn();
            }
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.LogFormat("{0} has disconnected! Inactive status: {1}", otherPlayer.NickName, otherPlayer.IsInactive);
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

    #region IPunTurnManagerCallbacks Callbacks

    public void OnTurnBegins(int turn) {
        throw new System.NotImplementedException();
    }

    public void OnTurnCompleted(int turn) {
        throw new System.NotImplementedException();
    }

    public void OnPlayerMove(Player player, int turn, object move) {
        throw new System.NotImplementedException();
    }

    public void OnPlayerFinished(Player player, int turn, object move) {
        throw new System.NotImplementedException();
    }

    public void OnTurnTimeEnds(int turn) {
        throw new System.NotImplementedException();
    }

    #endregion Callbacks Callbacks

    #region IOnEventCallback Callbacks

    public void OnEvent(EventData photonEvent) {
        byte eventCode = photonEvent.Code;

        switch(eventCode) {
            case EvAssignWind:
                PlayerManager.Wind wind = (PlayerManager.Wind)photonEvent.CustomData;

                Hashtable ht = new Hashtable();
                ht.Add(AssignWindPropKey, wind);
                PhotonNetwork.SetPlayerCustomProperties(ht);
                break;
        }
    }

    #endregion

    #region Gameplay Methods

    /// <summary>
    /// Called by the MasterClient to start a new Turn
    /// </summary>
    public void StartTurn() {
        this.turnManager.BeginTurn();
    }

    /// <summary>
    /// Called at the start of every game (when PunTurnManager.Turn == 0) by MasterClient
    /// </summary>
    public void InitializeGame() {
        this.AssignPlayerWind();
        this.GenerateTiles();
        this.DistributeTiles();
    }


    /// <summary>
    /// Called by MasterClient to assign a wind to each player
    /// </summary>
    private void AssignPlayerWind() {
        Random rand = new Random();
        List<PlayerManager.Wind> winds = ((PlayerManager.Wind[])Enum.GetValues(typeof(PlayerManager.Wind))).ToList();
        PlayerManager.Wind playerWind;

        foreach (Player player in PhotonNetwork.PlayerList) {
            int randomIndex = rand.Next(winds.Count());
            playerWind = winds[randomIndex];
            winds.Remove(winds[randomIndex]);

            PhotonNetwork.RaiseEvent(EvAssignWind, playerWind, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
            // TODO: Each player has to store their own winds after receiving event
        }

        Debug.LogFormat("The 4 winds have been assigned to each player");
    }


    /// <summary>
    /// Create 4 copies of each tile, giving 148 tiles
    /// </summary>
    public void GenerateTiles() {
        List<Tile> tiles = new List<Tile>();

        // Create 4 copies of each tile
        for (int k = 0; k < 4; k++) {

            foreach (Tile.Suit suit in Enum.GetValues(typeof(Tile.Suit))) {

                switch (suit) {
                    // Generate the tiles for Character, Dot and Bamboo suits
                    case Tile.Suit.Character:
                    case Tile.Suit.Dot:
                    case Tile.Suit.Bamboo:
                        foreach (Tile.Rank rank in Enum.GetValues(typeof(Tile.Rank))) {
                            tiles.Add(new Tile(suit, rank));
                        }
                        break;

                    // Generate the tiles for Wind, Season, Flower and Animal suits
                    case Tile.Suit.Wind:
                    case Tile.Suit.Season:
                    case Tile.Suit.Flower:
                    case Tile.Suit.Animal:
                        foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(4)) {
                            tiles.Add(new Tile(suit, rank));
                        }
                        break;

                    // Generate the tiles for Dragon suit
                    case Tile.Suit.Dragon:
                        foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(3)) {
                            tiles.Add(new Tile(suit, rank));
                        }
                        break;
                }
            }
        }

        if (tiles.Count != 148) {
            Debug.LogError("The number of tiles created isn't 148");
        }

        // Add to Room Custom Properties
        Hashtable tilesHT = new Hashtable();
        tilesHT.Add("tiles", tiles);
        PhotonNetwork.CurrentRoom.SetCustomProperties(tilesHT);
    }

    /// <summary>
    /// Distribute tiles to each player depending on the wind seat. The player with the East Wind receives 14 tiles
    /// while the rest receives 13
    /// </summary>
    public void DistributeTiles() {
        Random rand = new Random();
        List<Tile> tiles = (List<Tile>)PhotonNetwork.CurrentRoom.CustomProperties["tiles"];

        foreach (Player player in PhotonNetwork.PlayerList) {
            List<Tile> playerTiles = new List<Tile>();

            int i = 0;
            while (i < 14)  {
                // Choose a tile randomly from the complete tiles list and add it to the player's tiles
                int randomIndex = rand.Next(tiles.Count());
                playerTiles.Add(tiles[randomIndex]);
                tiles.Remove(tiles[randomIndex]);

                // Don't give the 14th tile if the player is not the East Wind
                if (!((PlayerManager.Wind)player.CustomProperties["playerWind"] == PlayerManager.Wind.EAST)) {
                    break;
                }
            }

            // TODO: RaiseEvent
        }

        // Reinsert updated tiles list into Room Custom Properties
        Hashtable tilesHT = new Hashtable();
        tilesHT.Add("tiles", tiles);
        PhotonNetwork.CurrentRoom.SetCustomProperties(tilesHT);
    }

    #endregion

    #region Public Methods

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    // Instantiate player at the wind seat based on playerWind
    private void MoveToWindSeat(PlayerManager.Wind playerWind) {
        Vector3 playerPos;
        Quaternion cameraRotation;

        // Determine seat location based on playerWind and rotate main camera accordingly.
        switch (playerWind) {
            case PlayerManager.Wind.EAST:
                playerPos = new Vector3(15f, 1, 0);
                cameraRotation = Quaternion.Euler(90f, 0f, 90f);
                break;
            case PlayerManager.Wind.SOUTH:
                playerPos = new Vector3(0, 1, -15f);
                cameraRotation = Quaternion.Euler(90f, 0f, 0f);
                break;
            case PlayerManager.Wind.WEST:
                playerPos = new Vector3(-15f, 1, 0);
                cameraRotation = Quaternion.Euler(90f, 0f, 270f);
                break;
            default:
                playerPos = new Vector3(0, 1, 15f);
                cameraRotation = Quaternion.Euler(90f, 0f, 180f);
                break;
        }
        Camera.main.transform.rotation = cameraRotation;

        // Spawn a character for the local player. It gets synced by using PhotonNetwork.Instantiate
        PhotonNetwork.Instantiate(this.playerPrefab.name, playerPos, Quaternion.identity, 0);
    }


    // Stretch the GameTable to fill up the screen, depending on the player's seat
    private void StretchGameTable(PlayerManager.Wind playerWind) {
        Camera camera = Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        // Scale the GameTable along the x-z axes
        if (playerWind == PlayerManager.Wind.NORTH || playerWind == PlayerManager.Wind.SOUTH) {
            gameTable.transform.localScale = new Vector3(width, 1, height);
        } else {
            gameTable.transform.localScale = new Vector3(height, 1, width);
        }
    }
    
    #endregion
}
