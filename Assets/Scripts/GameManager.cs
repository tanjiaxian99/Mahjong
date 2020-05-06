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

    [Tooltip("The prefab used for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;

    private PlayerManager playerManager;

    [Tooltip("The GameObject used to represent a physical Mahjong table")]
    [SerializeField]
    private GameObject gameTable;

    [Tooltip("Height of the Mahjong table in the local client")]
    [SerializeField]
    private float tableHeight;

    [Tooltip("Width of the Mahjong table in the local client")]
    [SerializeField]
    private float tableWidth;

    [Tooltip("Debugging: The number of players required to start a game")]
    [SerializeField]
    private int numberOfPlayersToStart = 4;

    [Tooltip("The factor by which to scale the tiles such that they fill different screen sizes proportionately")]
    private float scaleFactor = 1f;

    private PunTurnManager turnManager;

    /// <summary>
    /// HashTable where the keys are the string names of tiles and values are the tiles' prefab
    /// </summary>
    private Dictionary<string, GameObject> tilesDict = new Dictionary<string, GameObject>();

    #endregion

    #region OnEvent Fields

    /// <summary>
    /// The Wind Assignment event message byte. Used internally for saving data in local player's custom properties.
    /// </summary>
    public const byte EvAssignWind = 3;

    /// <summary>
    /// The Instantiate Player event message byte. Used internally for instantiating the local player and 
    /// stretching the GameTable to fill the screen.
    /// </summary>
    public const byte EvInstantiatePlayer = 4;

    /// <summary>
    /// The Distribute Tiles event message byte. Used internally for saving data in local player's PlayerManager.
    /// </summary>
    public const byte EvDistributeTiles = 5;

    /// <summary>
    /// The Instantiate Tiles event message byte. Used internally for instantiating the tiles in the local player's hand.
    /// </summary>
    public const byte EvInstantiateTiles = 6;

    /// <summary>
    /// Wind of the player
    /// </summary>
    public static readonly string PlayerWindPropKey = "pw";

    /// <summary>
    /// List of tiles in the walls
    /// </summary>
    public static readonly string WallTileListPropKey = "wt";

    /// <summary>
    /// List of tiles in the discard pool
    /// </summary>
    public static readonly string DiscardTileListPropKey = "dt";

    #endregion

    #region Tiles Prefabs
    [SerializeField]
    private GameObject Character_One;

    [SerializeField]
    private GameObject Character_Two;

    [SerializeField]
    private GameObject Character_Three;

    [SerializeField]
    private GameObject Character_Four;

    [SerializeField]
    private GameObject Character_Five;

    [SerializeField]
    private GameObject Character_Six;

    [SerializeField]
    private GameObject Character_Seven;

    [SerializeField]
    private GameObject Character_Eight;

    [SerializeField]
    private GameObject Character_Nine;

    [SerializeField]
    private GameObject Dot_One;

    [SerializeField]
    private GameObject Dot_Two;

    [SerializeField]
    private GameObject Dot_Three;

    [SerializeField]
    private GameObject Dot_Four;

    [SerializeField]
    private GameObject Dot_Five;

    [SerializeField]
    private GameObject Dot_Six;

    [SerializeField]
    private GameObject Dot_Seven;

    [SerializeField]
    private GameObject Dot_Eight;

    [SerializeField]
    private GameObject Dot_Nine;

    [SerializeField]
    private GameObject Bamboo_One;

    [SerializeField]
    private GameObject Bamboo_Two;

    [SerializeField]
    private GameObject Bamboo_Three;

    [SerializeField]
    private GameObject Bamboo_Four;

    [SerializeField]
    private GameObject Bamboo_Five;

    [SerializeField]
    private GameObject Bamboo_Six;

    [SerializeField]
    private GameObject Bamboo_Seven;

    [SerializeField]
    private GameObject Bamboo_Eight;

    [SerializeField]
    private GameObject Bamboo_Nine;

    [SerializeField]
    private GameObject Wind_One;

    [SerializeField]
    private GameObject Wind_Two;

    [SerializeField]
    private GameObject Wind_Three;

    [SerializeField]
    private GameObject Wind_Four;

    [SerializeField]
    private GameObject Dragon_One;

    [SerializeField]
    private GameObject Dragon_Two;

    [SerializeField]
    private GameObject Dragon_Three;

    [SerializeField]
    private GameObject Season_One;

    [SerializeField]
    private GameObject Season_Two;

    [SerializeField]
    private GameObject Season_Three;

    [SerializeField]
    private GameObject Season_Four;

    [SerializeField]
    private GameObject Flower_One;

    [SerializeField]
    private GameObject Flower_Two;

    [SerializeField]
    private GameObject Flower_Three;

    [SerializeField]
    private GameObject Flower_Four;

    [SerializeField]
    private GameObject Animal_One;

    [SerializeField]
    private GameObject Animal_Two;

    [SerializeField]
    private GameObject Animal_Three;

    [SerializeField]
    private GameObject Animal_Four;

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

            // Set up a HashTable for tiles
            this.InstantiateTilesDict();
            this.OnJoinedRoom();

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

                // Update local player's custom properties
                Hashtable ht = new Hashtable();
                ht.Add(PlayerWindPropKey, wind);
                PhotonNetwork.SetPlayerCustomProperties(ht);

                // Update local player's playerManager
                playerManager.PlayerWind = wind;
                break;

            case EvInstantiatePlayer:
                this.InstantiateLocalPlayer();
                this.StretchGameTable();
                break;

            case EvDistributeTiles:
                // Update local player's playerManager
                playerManager.hand = (List<Tile>)photonEvent.CustomData;
                break;

            case EvInstantiateTiles:
                this.InstantiateLocalTiles();
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
        this.DeterminePlayOrder();
        this.InstantiatePlayers();
        this.GenerateTiles();
        this.DistributeTiles();
        this.InstantiateTiles();
    }


    /// <summary>
    /// Called by MasterClient to assign a wind to each player
    /// </summary>
    public void AssignPlayerWind() {
        Random rand = new Random();
        List<PlayerManager.Wind> winds = ((PlayerManager.Wind[])Enum.GetValues(typeof(PlayerManager.Wind))).ToList();
        PlayerManager.Wind playerWind;

        foreach (Player player in PhotonNetwork.PlayerList) {
            int randomIndex = rand.Next(winds.Count());
            playerWind = winds[randomIndex];
            winds.Remove(winds[randomIndex]);

            PhotonNetwork.RaiseEvent(EvAssignWind, playerWind, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
        }

        Debug.LogFormat("The 4 winds have been assigned to each player");
    }


    /// <summary>
    /// Update the room's custom properties with the play order.
    /// Play order starts from East Wind and ends at South.
    /// </summary>
    public void DeterminePlayOrder() {
        Player[] playOrder = new Player[4];

        foreach (Player player in PhotonNetwork.PlayerList) {
            // PlayerManager.Wind is order from East to South. Retrieving the wind of the player and converting it to an int
            // will give the proper play order
            int index = (int)player.CustomProperties[PlayerWindPropKey];
            playOrder[index] = player;
        }

        Hashtable ht = new Hashtable();
        ht.Add(WallTileListPropKey, playOrder);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }


    /// <summary>
    /// Raise an event telling all players to instantiate their player prefab and stretch their GameTable
    /// </summary>
    public void InstantiatePlayers() {
        PhotonNetwork.RaiseEvent(EvInstantiatePlayer, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All}, SendOptions.SendReliable);
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
        Hashtable ht = new Hashtable();
        ht.Add(WallTileListPropKey, tiles);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    /// <summary>
    /// Distribute tiles to each player depending on the wind seat. The player with the East Wind receives 14 tiles
    /// while the rest receives 13
    /// </summary>
    public void DistributeTiles() {
        Random rand = new Random();
        List<Tile> tiles = (List<Tile>)PhotonNetwork.CurrentRoom.CustomProperties[WallTileListPropKey];

        foreach (Player player in PhotonNetwork.PlayerList) {
            List<Tile> playerTiles = new List<Tile>();

            int i = 0;
            while (i < 14)  {
                // Choose a tile randomly from the complete tiles list and add it to the player's tiles
                int randomIndex = rand.Next(tiles.Count());
                playerTiles.Add(tiles[randomIndex]);
                tiles.Remove(tiles[randomIndex]);

                // Don't give the 14th tile if the player is not the East Wind
                if (!((PlayerManager.Wind)player.CustomProperties[PlayerWindPropKey] == PlayerManager.Wind.EAST)) {
                    break;
                }
            }

            PhotonNetwork.RaiseEvent(EvDistributeTiles, playerTiles, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
        }

        // Reinsert updated tiles list into Room Custom Properties
        Hashtable ht = new Hashtable();
        ht.Add(WallTileListPropKey, tiles);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

        Debug.Log("The tiles from the wall have been distributed");
    }


    /// <summary>
    /// Raise an event telling all players to instantiate the tiles in their hand.
    /// </summary>
    public void InstantiateTiles() {
        PhotonNetwork.RaiseEvent(EvInstantiateTiles, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All}, SendOptions.SendReliable);
    }

    #endregion

    #region Local Player Methods

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }


    /// <summary>
    /// Fill up the tilesDict with the tile prebabs and their string representations
    /// </summary>
    public void InstantiateTilesDict() {
        tilesDict.Add("Character_One", Character_One);
        tilesDict.Add("Character_Two", Character_Two);
        tilesDict.Add("Character_Three", Character_Three);
        tilesDict.Add("Character_Four", Character_Four);
        tilesDict.Add("Character_Five", Character_Five);
        tilesDict.Add("Character_Six", Character_Six);
        tilesDict.Add("Character_Seven", Character_Seven);
        tilesDict.Add("Character_Eight", Character_Eight);
        tilesDict.Add("Character_Nine", Character_Nine);

        tilesDict.Add("Dot_One", Dot_One);
        tilesDict.Add("Dot_Two", Dot_Two);
        tilesDict.Add("Dot_Three", Dot_Three);
        tilesDict.Add("Dot_Four", Dot_Four);
        tilesDict.Add("Dot_Five", Dot_Five);
        tilesDict.Add("Dot_Six", Dot_Six);
        tilesDict.Add("Dot_Seven", Dot_Seven);
        tilesDict.Add("Dot_Eight", Dot_Eight);
        tilesDict.Add("Dot_Nine", Dot_Nine);

        tilesDict.Add("Bamboo_One", Bamboo_One);
        tilesDict.Add("Bamboo_Two", Bamboo_Two);
        tilesDict.Add("Bamboo_Three", Bamboo_Three);
        tilesDict.Add("Bamboo_Four", Bamboo_Four);
        tilesDict.Add("Bamboo_Five", Bamboo_Five);
        tilesDict.Add("Bamboo_Six", Bamboo_Six);
        tilesDict.Add("Bamboo_Seven", Bamboo_Seven);
        tilesDict.Add("Bamboo_Eight", Bamboo_Eight);
        tilesDict.Add("Bamboo_Nine", Bamboo_Nine);

        tilesDict.Add("Wind_One", Wind_One);
        tilesDict.Add("Wind_Two", Wind_Two);
        tilesDict.Add("Wind_Three", Wind_Three);
        tilesDict.Add("Wind_Four", Wind_Four);

        tilesDict.Add("Dragon_One", Dragon_One);
        tilesDict.Add("Dragon_Two", Dragon_Two);
        tilesDict.Add("Dragon_Three", Dragon_Three);

        tilesDict.Add("Season_One", Season_One);
        tilesDict.Add("Season_Two", Season_Two);
        tilesDict.Add("Season_Three", Season_Three);
        tilesDict.Add("Season_Four", Season_Four);

        tilesDict.Add("Flower_One", Flower_One);
        tilesDict.Add("Flower_Two", Flower_Two);
        tilesDict.Add("Flower_Three", Flower_Three);
        tilesDict.Add("Flower_Four", Flower_Four);

        tilesDict.Add("Animal_One", Animal_One);
        tilesDict.Add("Animal_Two", Animal_Two);
        tilesDict.Add("Animal_Three", Animal_Three);
        tilesDict.Add("Animal_Four", Animal_Four);
    }


    /// <summary>
    /// Instantiate the local player in the local client.
    /// </summary>
    public void InstantiateLocalPlayer() {
        // The local player is always seated at the bottom of the screen
        Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Spawn a character for the local player. 
        Instantiate(this.playerPrefab, new Vector3(0, 1, -15f), Quaternion.identity);
    }


    /// <summary>
    /// Stretch the GameTable to fill up the screen
    /// </summary>
    public void StretchGameTable() {
        Camera camera = Camera.main;
        tableHeight = 2f * camera.orthographicSize;
        tableWidth = tableHeight * camera.aspect;

        // Scale the GameTable along z direction
        gameTable.transform.localScale = new Vector3(tableWidth, 1, tableHeight);
    }


    /// <summary>
    /// After the local player receives the tiles, instantiate the tiles.
    /// </summary>
    public void InstantiateLocalTiles() {
        if (playerManager.hand == null) {
            Debug.Log("The player's hand is empty.");
            return;
        }
        
        // Separation between pivot of tiles
        float xSep = 0.83f;
        float xPos = -xSep * 6;

        foreach (Tile tile in playerManager.hand) {
            string tileName = tile.suit + "" + tile.rank;

            Instantiate(tilesDict[tileName], new Vector3(xPos, 1f, -4.4f), Quaternion.Euler(270f, 180f, 0f));
            xPos += xSep;
        }

        Debug.Log(playerManager.hand);
    }

    #endregion
}
