using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = System.Random;
using UnityEngine.SocialPlatforms;

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

    private static readonly Random random = new Random();

    private static readonly object syncLock = new object();

    private PunTurnManager turnManager;

    /// <summary>
    /// Dictionary containing a player's actor number and his playerWind in integer form
    /// </summary>
    private Dictionary<int, int> windsDict = new Dictionary<int, int>();

    /// <summary>
    /// Dictionary containing string names of tiles and their respective prefab
    /// </summary>
    private Dictionary<Tile, GameObject> tilesDict = new Dictionary<Tile, GameObject>();

    #endregion

    #region OnEvent Fields

    /// <summary>
    /// The Screen View Adjustment event message byte. Used internally for pointing the camera towards the GameTable and
    /// stretching the GameTable to fill the screen.
    /// </summary>
    public const byte EvScreenViewAdjustment = 4;

    /// <summary>
    /// The Distribute Tiles event message byte. Used internally for saving data in local player's PlayerManager.
    /// </summary>
    public const byte EvDistributeTiles = 5;

    /// <summary>
    /// The Initial Instantiation event message byte. Used internally for converting the local player's bonus tiles into normal tiles.
    /// Afterwards, instantiate the local player's hand and open tiles
    /// </summary>
    public const byte EvInitialInstantiation = 6;


    /// <summary>
    /// The Player Turn event message byte. Used internally to track if it is the local player's turn.
    /// </summary>
    public const byte EvPlayerTurn = 11;


    /// <summary>
    /// Dictionary containing actor numbers and wind assignments
    /// </summary>
    public static readonly string WindDictPropKey = "wd";

    /// <summary>
    /// Wind of the player
    /// </summary>
    public static readonly string PlayerWindPropKey = "pw";

    /// <summary>
    /// Play order sequence
    /// </summary>
    public static readonly string PlayOrderPropkey = "po";

    /// <summary>
    /// List of tiles in the walls
    /// </summary>
    public static readonly string WallTileListPropKey = "wt";

    /// <summary>
    /// The discarder's actor number, the tile discarded, and the discard position
    /// </summary>
    public static readonly string DiscardTilePropKey = "dt";

    /// <summary>
    /// Number of tiles in the player's hand
    /// </summary>
    public static readonly string HandTilesCountPropKey = "no";

    /// <summary>
    /// The local player's open tiles
    /// </summary>
    public static readonly string OpenTilesPropKey = "ot";

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

            // Register customType Tile and List<Tile>
            PhotonPeer.RegisterType(typeof(List<Tile>), 255, SerializeTilesList, DeserializeTilesList);
            PhotonPeer.RegisterType(typeof(Tuple<int, Tile, float>), 254, SerializeTuple, DeserializeTuple);
        }
    }

    void Update() {
        // The local player has done a move only when it is his turn and he clicked the left mouse button.
        if (playerManager.myTurn && Input.GetMouseButtonDown(0)) {
            this.OnLocalPlayerMove();
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
                StartCoroutine("InitializeGame");
            }

        } else {
            Debug.Log("Waiting for another player");
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("A new player has arrived");

        if (PhotonNetwork.CurrentRoom.PlayerCount == numberOfPlayersToStart) {
            if (this.turnManager.Turn == 0 && PhotonNetwork.IsMasterClient) {
                StartCoroutine("InitializeGame");
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


    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
        if (propertiesThatChanged.ContainsKey(WindDictPropKey)) {
            windsDict = (Dictionary<int, int>) PhotonNetwork.CurrentRoom.CustomProperties[WindDictPropKey];
            PlayerManager.Wind wind = (PlayerManager.Wind)windsDict[PhotonNetwork.LocalPlayer.ActorNumber];

            // Update local player's custom properties
            Hashtable ht = new Hashtable();
            ht.Add(PlayerWindPropKey, wind);
            PhotonNetwork.SetPlayerCustomProperties(ht);
            // Update local player's playerManager
            playerManager.PlayerWind = wind;

        } else if (propertiesThatChanged.ContainsKey(DiscardTilePropKey)) {
            Tuple<int, Tile, float> discardTile = (Tuple<int, Tile, float>) PhotonNetwork.CurrentRoom.CustomProperties[DiscardTilePropKey];
            Player player = PhotonNetwork.LocalPlayer.Get(discardTile.Item1);
            Tile tile = discardTile.Item2;
            Debug.Log(tile);
            float hPos = discardTile.Item3;

            // Only instantiate the tile if a remote player threw it
            if (player == PhotonNetwork.LocalPlayer) {
                return;
            }

            this.InstantiateRemoteDiscardTile(player, tile, hPos);
        }
    }


    /// <summary>
    /// Called when a remote player's hand or open tiles changes
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
        if (changedProps.ContainsKey(HandTilesCountPropKey) && targetPlayer != PhotonNetwork.LocalPlayer) {
            this.InstantiateRemoteHand(targetPlayer);

        } else if (changedProps.ContainsKey(OpenTilesPropKey) && targetPlayer != PhotonNetwork.LocalPlayer) {
            this.InstantiateRemoteOpenTiles(targetPlayer);
        }
    }

    #endregion

    #region IPunTurnManagerCallbacks Callbacks

    // We only care about the first turn
    public void OnTurnBegins(int turn) {
        if (turn > 1) {
            return;
        }
        Debug.LogFormat("Turn {0} has begun", turn);

        Player[] playOrder = (Player[])PhotonNetwork.CurrentRoom.CustomProperties[PlayOrderPropkey];
        PhotonNetwork.RaiseEvent(EvPlayerTurn, null, new RaiseEventOptions() { TargetActors = new int[] { playOrder[0].ActorNumber } }, SendOptions.SendReliable);
    }

    public void OnTurnCompleted(int turn) {
        this.StartTurn();
    }

    // What the local client does when a remote player performs a move
    public void OnPlayerMove(Player player, int turn, object move) {
        throw new System.NotImplementedException();
    }

    // What the local client does when a remote player finishes a move
    public void OnPlayerFinished(Player player, int turn, object move) {
        
    }

    // TODO: Does time refers to time of a single player's turn?
    public void OnTurnTimeEnds(int turn) {
        throw new System.NotImplementedException();
    }

    #endregion Callbacks Callbacks

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
    IEnumerator InitializeGame() {
        // At this point, Start hasn't been called yet. Wait a frame before proceeding with the Coroutine
        yield return null;
        this.AssignPlayerWind();
        this.DeterminePlayOrder();
        this.ScreenViewAdjustment();
        this.GenerateTiles();
        // Delay for WallTileListPropKey and PlayerWindPropKey related custom properties to update
        yield return new WaitForSeconds(1.5f);
        this.DistributeTiles();
        //this.InstantiateDiscardTilesList();
        StartCoroutine("InitialInstantiation");
        this.StartTurn();
        yield return null;
    }


    /// <summary>
    /// Create a Room Custom Property with a list of discard tiles
    /// </summary>
    // TODO: For the local client to keep track of discard tiles and their positions?
    //public void InstantiateDiscardTilesList() {
    //    Hashtable ht = new Hashtable();
    //    ht.Add(DiscardTilePropKey, new List<Tile>());
    //    PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    //}


    /// <summary>
    /// Called by MasterClient to assign a wind to each player
    /// </summary>
    public void AssignPlayerWind() {
        List<PlayerManager.Wind> winds = ((PlayerManager.Wind[])Enum.GetValues(typeof(PlayerManager.Wind))).ToList();
        PlayerManager.Wind playerWind;

        foreach (Player player in PhotonNetwork.PlayerList) {
            int randomIndex = (int) PlayerManager.Wind.EAST;
            if (numberOfPlayersToStart > 1) {
                randomIndex = RandomNumber(winds.Count());
            }

            playerWind = winds[randomIndex];
            winds.Remove(winds[randomIndex]);
            windsDict.Add(player.ActorNumber, (int) playerWind);
        }

        Hashtable ht = new Hashtable();
        ht.Add(WindDictPropKey, windsDict);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

        Debug.LogFormat("The 4 winds have been assigned to each player");
    }


    /// <summary>
    /// Update the room's custom properties with the play order. Play order starts from East Wind and ends at South.
    /// </summary>
    public void DeterminePlayOrder() {
        // TODO: An array containing the nicknames might be sufficient.
        Player[] playOrder = new Player[numberOfPlayersToStart];

        foreach (int actorNumber in windsDict.Keys) {
            // The values of windsDict are PlayerManager.Wind types, which are order from East:0 to South: 3
            // The integer value of windsDict themselves is the order of play
            int index = windsDict[actorNumber];
            playOrder[index] = PhotonNetwork.LocalPlayer.Get(actorNumber);
        }

        Hashtable ht = new Hashtable();
        ht.Add(PlayOrderPropkey, playOrder);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }


    /// <summary>
    /// Raise an event telling all players to point their camera towards the GameTable and stretch the GameTable
    /// </summary>
    public void ScreenViewAdjustment() {
        PhotonNetwork.RaiseEvent(EvScreenViewAdjustment, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All}, SendOptions.SendReliable);
    }


    /// <summary>
    /// Create 4 copies of each tile, giving 148 tiles
    /// </summary>
    public void GenerateTiles() {
        List<Tile> tiles = new List<Tile>();

        foreach (Tile.Suit suit in Enum.GetValues(typeof(Tile.Suit))) {

            switch (suit) {
                // Generate the tiles for Character, Dot and Bamboo suits
                case Tile.Suit.Character:
                case Tile.Suit.Dot:
                case Tile.Suit.Bamboo:
                    foreach (Tile.Rank rank in Enum.GetValues(typeof(Tile.Rank))) {
                        for (int i = 0; i < 4; i++) {
                            tiles.Add(new Tile(suit, rank));
                        }
                    }
                    break;

                // Generate the tiles for Wind suit
                case Tile.Suit.Wind:
                    foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(4)) {
                        for (int i = 0; i < 4; i++) {
                            tiles.Add(new Tile(suit, rank));
                        }
                    }
                    break;

                // Generate the tiles for Dragon suit
                case Tile.Suit.Dragon:
                    foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(3)) {
                        for (int i = 0; i < 4; i++) {
                            tiles.Add(new Tile(suit, rank));
                        }
                    }
                    break;

                // Generate the tiles for Season, Flower and Animal suits. Only generates 1 tile for each rank.
                case Tile.Suit.Season:
                case Tile.Suit.Flower:
                case Tile.Suit.Animal:
                    foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(4)) {
                        tiles.Add(new Tile(suit, rank));
                    }
                    break;
            }
        }

        if (tiles.Count != 148) {
            Debug.LogErrorFormat("{0} tiles have been created instead of 148", tiles.Count);
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
        List<Tile> tiles = (List<Tile>)PhotonNetwork.CurrentRoom.CustomProperties[WallTileListPropKey];

        foreach (Tile tile in tiles) {
        }

        foreach (Player player in PhotonNetwork.PlayerList) {
            List<Tile> playerTiles = new List<Tile>();
            Debug.LogFormat("Own player's wind: {0}", playerManager.PlayerWind);
            for (int i = 0; i < 14; i++) {
                // Choose a tile randomly from the complete tiles list and add it to the player's tiles
                int randomIndex = RandomNumber(tiles.Count());
                playerTiles.Add(tiles[randomIndex]);
                tiles.Remove(tiles[randomIndex]);

                // Don't give the 14th tile if the player is not the East Wind
                Debug.Log(player.CustomProperties[PlayerWindPropKey]);
                if (i == 12 && (PlayerManager.Wind)player.CustomProperties[PlayerWindPropKey] != PlayerManager.Wind.EAST) {
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
    /// Convert the bonus tiles to normal tiles and instantiate the local player's hand and open tiles. Done in playOrder sequence.
    /// </summary>
    IEnumerator InitialInstantiation() {
        foreach (Player player in (Player[]) PhotonNetwork.CurrentRoom.CustomProperties[PlayOrderPropkey]) {
            PhotonNetwork.RaiseEvent(EvInitialInstantiation, null, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion

    #region IOnEventCallback Callbacks

    public void OnEvent(EventData photonEvent) {
        byte eventCode = photonEvent.Code;

        switch (eventCode) {
            case EvScreenViewAdjustment:
                this.LocalScreenViewAdjustment();
                break;

            case EvDistributeTiles:
                // Update local player's playerManager
                playerManager.hand = (List<Tile>)photonEvent.CustomData;
                break;

            case EvInitialInstantiation:
                this.InitialLocalInstantiation();
                break;

            case EvPlayerTurn:
                playerManager.myTurn = true;
                this.PlayerStartTurn();
                break;
        }
    }

    #endregion

    #region Methods called by Local Player

    /// <summary>
    /// Generates a random number. lock() makes it thread-safe
    /// </summary>
    // https://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
    public static int RandomNumber(int max) {
        lock (syncLock) { // synchronize
            return random.Next(max);
        }
    }

    /// <summary>
    /// Fill up the tilesDict with the tile prebabs and their string representations
    /// </summary>
    public void InstantiateTilesDict() {
        tilesDict.Add(new Tile("Character_One"), Character_One);
        tilesDict.Add(new Tile("Character_Two"), Character_Two);
        tilesDict.Add(new Tile("Character_Three"), Character_Three);
        tilesDict.Add(new Tile("Character_Four"), Character_Four);
        tilesDict.Add(new Tile("Character_Five"), Character_Five);
        tilesDict.Add(new Tile("Character_Six"), Character_Six);
        tilesDict.Add(new Tile("Character_Seven"), Character_Seven);
        tilesDict.Add(new Tile("Character_Eight"), Character_Eight);
        tilesDict.Add(new Tile("Character_Nine"), Character_Nine);

        tilesDict.Add(new Tile("Dot_One"), Dot_One);
        tilesDict.Add(new Tile("Dot_Two"), Dot_Two);
        tilesDict.Add(new Tile("Dot_Three"), Dot_Three);
        tilesDict.Add(new Tile("Dot_Four"), Dot_Four);
        tilesDict.Add(new Tile("Dot_Five"), Dot_Five);
        tilesDict.Add(new Tile("Dot_Six"), Dot_Six);
        tilesDict.Add(new Tile("Dot_Seven"), Dot_Seven);
        tilesDict.Add(new Tile("Dot_Eight"), Dot_Eight);
        tilesDict.Add(new Tile("Dot_Nine"), Dot_Nine);

        tilesDict.Add(new Tile("Bamboo_One"), Bamboo_One);
        tilesDict.Add(new Tile("Bamboo_Two"), Bamboo_Two);
        tilesDict.Add(new Tile("Bamboo_Three"), Bamboo_Three);
        tilesDict.Add(new Tile("Bamboo_Four"), Bamboo_Four);
        tilesDict.Add(new Tile("Bamboo_Five"), Bamboo_Five);
        tilesDict.Add(new Tile("Bamboo_Six"), Bamboo_Six);
        tilesDict.Add(new Tile("Bamboo_Seven"), Bamboo_Seven);
        tilesDict.Add(new Tile("Bamboo_Eight"), Bamboo_Eight);
        tilesDict.Add(new Tile("Bamboo_Nine"), Bamboo_Nine);

        tilesDict.Add(new Tile("Wind_One"), Wind_One);
        tilesDict.Add(new Tile("Wind_Two"), Wind_Two);
        tilesDict.Add(new Tile("Wind_Three"), Wind_Three);
        tilesDict.Add(new Tile("Wind_Four"), Wind_Four);

        tilesDict.Add(new Tile("Dragon_One"), Dragon_One);
        tilesDict.Add(new Tile("Dragon_Two"), Dragon_Two);
        tilesDict.Add(new Tile("Dragon_Three"), Dragon_Three);

        tilesDict.Add(new Tile("Season_One"), Season_One);
        tilesDict.Add(new Tile("Season_Two"), Season_Two);
        tilesDict.Add(new Tile("Season_Three"), Season_Three);
        tilesDict.Add(new Tile("Season_Four"), Season_Four);

        tilesDict.Add(new Tile("Flower_One"), Flower_One);
        tilesDict.Add(new Tile("Flower_Two"), Flower_Two);
        tilesDict.Add(new Tile("Flower_Three"), Flower_Three);
        tilesDict.Add(new Tile("Flower_Four"), Flower_Four);

        tilesDict.Add(new Tile("Animal_One"), Animal_One);
        tilesDict.Add(new Tile("Animal_Two"), Animal_Two);
        tilesDict.Add(new Tile("Animal_Three"), Animal_Three);
        tilesDict.Add(new Tile("Animal_Four"), Animal_Four);
    }


    /// <summary>
    /// Point the camera towards the GameTable and stretch the GameTable to fill up the screen
    /// </summary>
    public void LocalScreenViewAdjustment() {
        Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        Camera camera = Camera.main;
        tableHeight = 2f * camera.orthographicSize;
        tableWidth = tableHeight * camera.aspect;

        // Scale the GameTable along z direction
        gameTable.transform.localScale = new Vector3(tableWidth, 1, tableHeight);
    }


    /// <summary>
    /// Called by the local player during the initial instantiation of tiles. Bonus tiles are converted, before the
    /// player's hand and open tiles are instantiated.
    /// </summary>
    public void InitialLocalInstantiation() {
        if (playerManager.hand == null) {
            Debug.LogError("The player's hand is empty.");
        }

        if (playerManager.openTiles.Any()) {
            Debug.LogError("The player has open tiles leftover from the previous game.");
        }

        // Check the local player's hand for bonus tiles. If there are, convert them to normal tiles.
        while (true) {
            bool haveBonusTile = false;

            for (int i = 0; i < playerManager.hand.Count; i++) {
                if (playerManager.hand[i].IsBonus()) {
                    // Add tile to bonus tiles list, which are instantiated separately
                    playerManager.bonusTiles.Add(playerManager.hand[i]);
                    playerManager.hand[i] = this.DrawTile();
                    haveBonusTile = true;
                }
            }

            if (!haveBonusTile) {
                break;
            }
        }
        
        playerManager.UpdateOpenTiles();

        // Add the local player's open tiles to custom properties
        Hashtable ht = new Hashtable();
        ht.Add(OpenTilesPropKey, playerManager.openTiles);
        PhotonNetwork.SetPlayerCustomProperties(ht);

        this.InstantiateLocalHand();
        this.InstantiateLocalOpenTiles();
    }


    /// <summary>
    /// Called immediately when myTurn is set to true. The only times it is not called is when the East Wind makes the first move, or
    /// when the player Chow/Pong/Kong
    /// </summary>
    public void PlayerStartTurn() {
        if (turnManager.Turn == 1 && playerManager.PlayerWind == PlayerManager.Wind.EAST) {
            return;
        }

        List<Tile> hand = playerManager.hand;
        hand.Add(this.DrawTile());

        this.ConvertLocalBonusTiles();
        this.InstantiateLocalHand();
        this.InstantiateLocalOpenTiles();
    }


    /// <summary>
    /// Called when the local player wants to select a tile
    /// </summary>
    public void OnLocalPlayerMove() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;        

        if (Physics.Raycast(ray, out hit)) {
            GameObject hitObject = hit.transform.gameObject;
            
            if (hitObject.transform.parent == null) {
                return;
            }

            // If the GameObject hit is a child object of a tile from the player's hand, remove that tile.
            if (hitObject.transform.parent.tag == "Hand") {

                // hitObject.transform.name will only give the name of the child object. E.g. group_0_16777215
                string tileName = hitObject.transform.parent.name;

                // Remove the phrase '(Clone)' from the back of the name
                tileName = tileName.Substring(0, tileName.Length - 7);
                Tile tile = new Tile(tileName);

                if (playerManager.hand.Contains(tile)) {
                    playerManager.myTurn = false;
                    playerManager.hand.Remove(tile);
                    
                    this.InstantiateLocalHand();
                    this.DiscardTile(tile, hitObject.transform.position.x);

                    // TODO: Integrate more turnManager.SendMove
                    turnManager.SendMove(null, true);
                    this.nextPlayersTurn();
                }
            }
        }
    }
    // TODO: Create a dictionary with discarded tile and current location. In the event that a player disconnects and reconnects,
    // he can reconstruct the scene.


    /// <summary>
    /// Convert the bonus (Season, Flower and Animal) tile into a normal tile.
    /// </summary>
    public void ConvertLocalBonusTiles() {
        List<Tile> hand = playerManager.hand;
        
        while (true) {
            Tile tile = hand[hand.Count - 1];

            // Check if the tile is a bonusTile
            if (!tile.IsBonus()) {
                break;
            }
            playerManager.bonusTiles.Add(tile);
            hand[hand.Count - 1] = this.DrawTile();
        }
        
        playerManager.UpdateOpenTiles();

        // Update the list of open tiles on the local player's custom properties
        Hashtable ht = new Hashtable();
        ht.Add(OpenTilesPropKey, playerManager.openTiles);
        PhotonNetwork.SetPlayerCustomProperties(ht);
    }


    /// <summary>
    /// Draw a new tile. No distinction made between front end or back end of Wall Tiles.
    /// </summary>
    // TODO: To be called when converting bonus tiles
    public Tile DrawTile() {
        List<Tile> tiles = (List<Tile>)PhotonNetwork.CurrentRoom.CustomProperties[WallTileListPropKey];

        int randomIndex = RandomNumber(tiles.Count());
        Tile tile = tiles[randomIndex];
        tiles.Remove(tiles[randomIndex]);

        // Reinsert updated tiles list into Room Custom Properties
        Hashtable ht = new Hashtable();
        ht.Add(WallTileListPropKey, tiles);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

        return tile;
    }


    /// <summary>
    /// Called whenever there is an update to the player's hand.
    /// </summary>
    public void InstantiateLocalHand() {
        // Update the local player's hand count custom property
        Hashtable ht = new Hashtable();
        ht.Add(HandTilesCountPropKey, playerManager.hand.Count);
        PhotonNetwork.SetPlayerCustomProperties(ht);


        // Separation between pivot of tiles
        float xSepHand = 0.83f;
        // x-coordinate of the position where the tile will be instantiated
        float xPosHand = 0;
        // The offset specifically for the drawn tile
        float xOffset = 0.30f;
        int handSize = playerManager.hand.Count;

        // taggedHand represents the tiles currently on the GameTable. It represents the hand one move prior to the current hand.
        GameObject[] taggedHand = GameObject.FindGameObjectsWithTag("Hand");

        // Initial instantiation of tiles
        if (taggedHand.Length == 0) {
            // Both formulas will yield the same xPosHand
            if (handSize == 13) {
                xPosHand = -((handSize - 1) / 2f) * xSepHand;
            } else if (handSize == 14){
                xPosHand = -((handSize - 2) / 2f) * xSepHand;
            }
            
            // Sort the player's hand when it is first received
            playerManager.hand = playerManager.hand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();

            for (int i = 0; i < playerManager.hand.Count; i++) {
                // Add an offset so that the 14th tile will be placed away from the other tiles.
                if (i == 13) {
                    xPosHand += xOffset;
                }

                this.InstantiateSingleTile(playerManager.hand[i], xPosHand);
                xPosHand += xSepHand;
            }
            return;
        }

        // When the player draws a tile, there will be one more tile in the player's hand than on the GameTable. Instantiate the drawn tile.
        // handSize is either 2, 5, 8, 11 or 14.
        if (taggedHand.Length + 1 == handSize) {
            xPosHand = (handSize / 2f) * xSepHand + xOffset;
            Tile tile = playerManager.hand[handSize - 1];
            this.InstantiateSingleTile(tile, xPosHand);
            return;
        }


        // When the player discards a tile, there will be one less tile in the player's hand than on the GameTable. 
        // Destroy all tiles, sort the player's hand, and instantiate the new tiles.
        // handSize is either 1, 4, 7, 10 or 13.
        if (taggedHand.Length - 1 == handSize) {
            xPosHand = -((handSize - 1) / 2f) * xSepHand;

            // Destroy tiles on the GameTable
            foreach (GameObject tileGameObject in taggedHand) {
                Destroy(tileGameObject);
            }

            // Sort the player's hand after discarding a tile
            playerManager.hand = playerManager.hand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();

            foreach (Tile tile in playerManager.hand) {
                this.InstantiateSingleTile(tile, xPosHand);
                xPosHand += xSepHand;
            }
            return;
        }


        // When the player Chow, Pong or Kong, there will be two less tiles in the player's hand than on the GameTable. 
        // Destroy all tiles and instantiate the new tiles (there is no need to sort since Chow/Pong/Kong won't mess up the order)
        // handSize is either 2, 5, 8, 11 or 14.
        if (taggedHand.Length - 2 == handSize) {
            xPosHand = -((handSize - 1) / 2f) * xSepHand;

            // Destroy tiles on the GameTable
            foreach (GameObject tileGameObject in taggedHand) {
                Destroy(tileGameObject);
            }
            
            for (int i = 0; i < playerManager.hand.Count; i++) {
                // Add an offset so that the last tile will be placed away from the other tiles.
                if (i == playerManager.hand.Count - 1) {
                    xPosHand += xOffset;
                }

                this.InstantiateSingleTile(playerManager.hand[i], xPosHand);
                xPosHand += xSepHand;
            }
            return;
        }

        Debug.LogErrorFormat("Size of hand: {0}. Number of tiles on GameTable: {1}", handSize, taggedHand.Length);
    }


    /// <summary>
    /// Helper method for InstantiateLocalHand to instantiate a single tile and tagging the children GameObjects
    /// </summary>
    public void InstantiateSingleTile(Tile tile, float xPosHand) {
        GameObject tileGameObject = Instantiate(tilesDict[tile], new Vector3(xPosHand, 0.85f, -4.4f), Quaternion.Euler(270f, 180f, 0f));
        tileGameObject.tag = "Hand";
    }

    
    /// <summary>
    /// Instantiate the open tiles of the local player. Called when there is an update to the bonusTiles/comboTiles list.
    /// </summary>
    public void InstantiateLocalOpenTiles() {
        int openSize = playerManager.openTiles.Count;
        float xSepOpen = 0.83f * 0.5f;
        float xPosOpen = -(openSize - 1) / 2f * xSepOpen;
        
        // taggedOpen represents the tiles currently on the GameTable. It represents the hand one move prior to the current hand.
        GameObject[] taggedOpen = GameObject.FindGameObjectsWithTag("Open");

        foreach (GameObject gameObject in taggedOpen) {
            Destroy(gameObject);
        }

        foreach (Tile tile in playerManager.openTiles) {
            GameObject newTile = Instantiate(tilesDict[tile], new Vector3(xPosOpen, 1f, -3.5f), Quaternion.Euler(270f, 180f, 0f));
            newTile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newTile.tag = "Open";

            xPosOpen += xSepOpen;
        }
    }


    /// <summary>
    /// Update the Room's Custom Properties with the discarded tile and tosses the tile to the middle of the GameTable.
    /// </summary>
    // TODO: detect changes in CustomProperties which contains the word discardtilepropkey
    public void DiscardTile(Tile tile, float xPos) {
        // Add the discarded tile to the DiscardTileList
        Hashtable ht = new Hashtable();
        Tuple<int, Tile, float> tuple = new Tuple<int, Tile, float>(PhotonNetwork.LocalPlayer.ActorNumber, tile, xPos);
        ht.Add(DiscardTilePropKey, tuple);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

        GameObject tileGameObject = Instantiate(tilesDict[tile], new Vector3(xPos, 0.652f, -2.8f), Quaternion.Euler(270f, 180f, 0f));
        tileGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        foreach (Transform child in tileGameObject.transform) {
            child.GetComponent<MeshCollider>().convex = true;
        }

        // tan(α) = zPos / xPos = zForce / xForce
        // xForce ** 2 + zforce ** 2 = rForce ** 2
        // Small offsets have been added to xForce and zForce to give more force to tiles tossed from the sides
        double rForce = 8;
        double tanα = 2.8 / (xPos + 0.1);
        double xForce = Math.Sqrt(Math.Pow(rForce, 2) / (1 + Math.Pow(tanα, 2))) + Math.Abs(xPos / 1.5f);
        double zForce = Math.Abs(xForce * tanα) + Math.Pow(xPos / 4, 2);
        if (xPos > 0) {
            xForce = -xForce;
        }

        Rigidbody rb = tileGameObject.AddComponent<Rigidbody>();
        rb.AddForce(new Vector3((float) xForce, 0f, (float) zForce), ForceMode.Impulse);
    }

   
    /// <summary>
    /// Called by the local player to inform the next player that it is his turn
    /// </summary>
    public void nextPlayersTurn() {
        Player[] playOrder = (Player[]) PhotonNetwork.CurrentRoom.CustomProperties[PlayOrderPropkey];
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


        

        PhotonNetwork.RaiseEvent(EvPlayerTurn, null, new RaiseEventOptions() { TargetActors = new int[] { nextPlayer.ActorNumber } }, SendOptions.SendReliable);
    }

    #endregion

    #region Methods called by Remote Player

    /// <summary>
    /// Called by the remote player to instantiate the hand of remotePlayer on the local client.
    /// </summary>
    public void InstantiateRemoteHand(Player remotePlayer) {
        int remoteHandSize = (int)remotePlayer.CustomProperties[HandTilesCountPropKey];
        PlayerManager.Wind wind = (PlayerManager.Wind) windsDict[remotePlayer.ActorNumber];

        // Represents the tiles currently on the GameTable which the remote player had
        GameObject[] taggedRemoteHand = GameObject.FindGameObjectsWithTag(wind + "_" + "Hand");
        // Destroy the remote player's hand tiles
        foreach (GameObject tileGameObject in taggedRemoteHand) {
            Destroy(tileGameObject);
        }

        List<Tile> remoteHand = new List<Tile>();
        for (int i = 0; i < remoteHandSize; i++) {
            remoteHand.Add(new Tile(0, 0));
        }
        InstantiateRemoteTiles(wind, remoteHand, this.RelativePlayerPosition(remotePlayer), "Hand");
    }


    /// <summary>
    /// Called by the remote player to instantiate the hand of remotePlayer on the local client.
    /// </summary>
    public void InstantiateRemoteOpenTiles(Player remotePlayer) {
        List<Tile> remoteOpenTiles = (List<Tile>) remotePlayer.CustomProperties[OpenTilesPropKey];
        PlayerManager.Wind wind = (PlayerManager.Wind) windsDict[remotePlayer.ActorNumber];

        // Represents the tiles currently on the GameTable which the remote player had
        GameObject[] taggedRemoteOpenTiles = GameObject.FindGameObjectsWithTag(wind + "_" + "Open");

        // Destroy the remote player's hand tiles
        foreach (GameObject tileGameObject in taggedRemoteOpenTiles) {
            Destroy(tileGameObject);
        }

        InstantiateRemoteTiles(wind, remoteOpenTiles, this.RelativePlayerPosition(remotePlayer), "Open");
    }


    /// <summary>
    /// Helper function for InstantiateRemoteHand and InstantiateRemoteOpenTiles
    /// </summary>
    public void InstantiateRemoteTiles(PlayerManager.Wind wind, List<Tile> remoteTiles, string remotePosition, string tileType) {
        // Starting position to instantiate the tiles
        float pos;

        // Separation between tiles
        float sep = 0.83f * 0.5f;

        // Offset for the drawn tile
        float offset = 0.30f * 0.5f;

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        int negativeConversion; 
        int remoteTilesSize = remoteTiles.Count;



        // Determine whether negativeConversion is -1 or 1
        if (remotePosition.Equals("Left") || remotePosition.Equals("Opposite")) {
            negativeConversion = 1;
        } else if (remotePosition.Equals("Right")) {
            negativeConversion = -1;
        } else {
            Debug.LogError("Invalid remote position. Only accepted remote positions are 'Left', 'Right' and 'Opposite'");
            return;
        }


        // Calculating the position of the first tile
        if (tileType.Equals("Hand") && new[] { 2, 5, 8, 11, 14 }.Contains(remoteTilesSize)) {
            pos = negativeConversion * 0.83f * 0.5f * (remoteTilesSize - 2) / 2;
        } else {
            pos = negativeConversion * 0.83f * 0.5f * (remoteTilesSize - 1) / 2;
        }


        // General formula for instantiating remote tiles
        for (int i = 0; i < remoteTilesSize; i++) {

            // Instantiate the last hand tile with an offset
            if (tileType.Equals("Hand") && new[] { 2, 5, 8, 11, 14 }.Contains(remoteTilesSize) && remoteTilesSize - 1 == i) {
                pos += -negativeConversion * offset;
            }


            // Calculate the position and rotation of each tile
            if (tileType.Equals("Hand")) {
                if (remotePosition.Equals("Left")) {
                    position = new Vector3(-tableWidth / 2 + 0.5f, 1f, pos);
                    rotation = Quaternion.Euler(0f, -90f, 0f);

                } else if (remotePosition.Equals("Right")) {
                    position = new Vector3(tableWidth / 2 - 0.5f, 1f, pos);
                    rotation = Quaternion.Euler(0f, 90f, 0f);

                } else if (remotePosition.Equals("Opposite")) {
                    position = new Vector3(pos, 1f, 4.4f);
                    rotation = Quaternion.Euler(0f, 0f, 0f);

                }

            } else if (tileType.Equals("Open")) {
                if (remotePosition.Equals("Left")) {
                    position = new Vector3(-tableWidth / 2 + 0.5f + 0.7f, 1f, pos);
                    rotation = Quaternion.Euler(-90f, -90f, 0f);

                } else if (remotePosition.Equals("Right")) {
                    position = new Vector3(tableWidth / 2 - 0.5f - 0.7f, 1f, pos);
                    rotation = Quaternion.Euler(-90f, 90f, 0f);

                } else if (remotePosition.Equals("Opposite")) {
                    position = new Vector3(pos, 1f, 4.4f - 0.7f);
                    rotation = Quaternion.Euler(-90f, 0f, 0f);
                }

            } else {
                Debug.LogError("Invalid tile type. Only accepted tile types are 'Hand' and 'Open'");
                return;
            }


            GameObject newTile = Instantiate(tilesDict[remoteTiles[i]], position, rotation);
            newTile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newTile.tag = wind + "_" + tileType;

            pos += -negativeConversion * sep;
        }

        return;
    }


    /// <summary>
    /// Determine the relative position of the remotePlayer with respect to the local player.
    /// </summary>
    public string RelativePlayerPosition(Player remotePlayer) {
        Player[] playOrder = (Player[])PhotonNetwork.CurrentRoom.CustomProperties[PlayOrderPropkey];

        // Retrieve the local and remote players' positions
        int localPlayerPos = 0;
        int remotePlayerPos = 0;
        for (int i = 0; i < playOrder.Length; i++) {
            if (playOrder[i] == PhotonNetwork.LocalPlayer) {
                localPlayerPos = i;
            }

            if (playOrder[i] == remotePlayer) {
                remotePlayerPos = i;
            }
        }

        // If the remote player is sitting on the left, the (localPlayerPos, remotePlayerPos) combinations are (1, 4), (2, 1), (3, 2), (4, 3)
        if (remotePlayerPos - localPlayerPos == 3 || localPlayerPos - remotePlayerPos == 1) {
            return "Left";
        }

        // If the remote player is sitting on the right, the (localPlayerPos, remotePlayerPos) combinations are (1, 2), (2, 3), (3, 4), (4, 1)
        if (localPlayerPos - remotePlayerPos == 3 || remotePlayerPos - localPlayerPos == 1) {
            return "Right";
        }

        // If the remote player is sitting on the opposite side
        // (localPlayerPos, remotePlayerPos) combinations are (1, 3), (2, 4), (3, 1), (4, 2)
        if (Math.Abs(localPlayerPos - remotePlayerPos) == 2) {
            return "Opposite";
        }

        Debug.LogErrorFormat("Invalid combination of localPlayerPos({0}) and remotePlayerPos({1})", localPlayerPos, remotePlayerPos);
        return "";
    }


    /// <summary>
    /// Called by the remote player to instantiate the discarded tile.
    /// </summary>
    /// <param name="hPos">The horizontal position of the tile from the perspective of the remote player</param>
    public void InstantiateRemoteDiscardTile(Player remotePlayer, Tile discardedTile, float hPos) {
        // v and h represents vertical and horizontal directions with respect to the perspective of the remote player
        // tan(α) = vPos / hPos = vForce / hForce; hForce ** 2 + vforce ** 2 = rForce ** 2
        // Small offsets have been added to xForce and zForce to give more force to tiles tossed from the sides
        double rForce = 8;
        double tanα = 2.8 / (hPos + 0.1);
        double hForce = Math.Sqrt(Math.Pow(rForce, 2) / (1 + Math.Pow(tanα, 2))) + Math.Abs(hPos / 1.5f);
        double vForce = Math.Abs(hForce * tanα) + Math.Pow(hPos / 4, 2);

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        // Instantiation position and rotation depends on where the remote player is sitting relative to the local player
        if (RelativePlayerPosition(remotePlayer).Equals("Left")) {
            position = new Vector3(-tableWidth / 2 + 0.5f + 0.7f + 0.6f, 1f, hPos);
            rotation = Quaternion.Euler(-90f, -90f, 0f);
            if (hPos < 0) {
                hForce = -hForce;
            }

        } else if (RelativePlayerPosition(remotePlayer).Equals("Right")) {
            position = new Vector3(tableWidth / 2 - 0.5f - 0.7f - 0.6f, 1f, hPos);
            rotation = Quaternion.Euler(0f, 90f, 0f);
            vForce = -vForce;
            if (hPos > 0) {
                hForce = -hForce;
            }

        } else if (RelativePlayerPosition(remotePlayer).Equals("Opposite")) {
            position = new Vector3(hPos, 1f, 4.4f - 0.7f - 0.6f);
            rotation = Quaternion.Euler(-90f, 0f, 0f);
            vForce = -vForce;
            if (hPos < 0) {
                hForce = -hForce;
            }

        }

        GameObject tileGameObject = Instantiate(tilesDict[discardedTile], position, rotation);
        tileGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        foreach (Transform child in tileGameObject.transform) {
            child.GetComponent<MeshCollider>().convex = true;
        }

        Rigidbody rb = tileGameObject.AddComponent<Rigidbody>();

        // The application of hForce and VForce on the x-z axes depends on the remote player's position
        if (RelativePlayerPosition(remotePlayer).Equals("Left") || RelativePlayerPosition(remotePlayer).Equals("Right")) {
            rb.AddForce(new Vector3((float) vForce, 0f, (float) hForce), ForceMode.Impulse);

        } else if (RelativePlayerPosition(remotePlayer).Equals("Opposite")) {
            rb.AddForce(new Vector3((float) hForce, 0f, (float) vForce), ForceMode.Impulse);
        }
    }

    #endregion

    #region Custom Types

    /// <summary>
    /// Serialize List<Tile> into a byteStream
    /// </summary>
    public static byte[] SerializeTilesList(object customType) {
        var tilesList = (List<Tile>)customType;
        byte[] byteArray = new byte[tilesList.Count];

        for (int i = 0; i < tilesList.Count; i++) {
            byteArray[i] = tilesList[i].Id;
        }
        return byteArray;
    }


    /// <summary>
    /// Deserialize the byteStream into a List<Tile>
    /// </summary>
    public static object DeserializeTilesList(byte[] data) {
        List<Tile> tilesList = new List<Tile>();
        
        foreach (byte tileByte in data) {
            Tile tile = new Tile(0, 0);
            tile.Id = tileByte;
            tilesList.Add(tile);
        }

        return tilesList;
    }


    /// <summary>
    /// Serialize Tuple<int, Tile, float> into a byteStream
    /// </summary>
    public static readonly byte[] memTuple = new byte[5 + 2 + 5];
    public static short SerializeTuple (StreamBuffer outStream, object customobject) {
        var tuple = (Tuple<int, Tile, float>) customobject;
        
        lock (memTuple) {
            byte[] bytes = memTuple;
            int index = 0;

            Protocol.Serialize(tuple.Item1, bytes, ref index);
            Protocol.Serialize(tuple.Item2.Id, bytes, ref index);
            Protocol.Serialize(tuple.Item3, bytes, ref index);
            outStream.Write(bytes, 0, 5 + 2 + 5);
        }

        return 5 + 2 + 5;
    }


    /// <summary>
    /// Deserialize the byteStream into a Tuple<int, Tile, float>
    /// </summary>
    private static object DeserializeTuple (StreamBuffer inStream, short length) {
        Tuple<int, Tile, float> tuple = new Tuple<int, Tile, float>(0, new Tile(0, 0), 0f);
        int actorNumber;
        int tileId;
        Tile tile = new Tile(0, 0);
        float pos;
        
        lock (memTuple) {
            inStream.Read(memTuple, 0, 5 + 2 + 5);
            int index = 0;
            
            Protocol.Deserialize(out actorNumber, memTuple, ref index);
            Protocol.Deserialize(out tileId, memTuple, ref index);
            Protocol.Deserialize(out pos, memTuple, ref index);

            
            tile.Id = (byte) tileId;
        }

        return new Tuple<int, Tile, float>(actorNumber, tile, pos);
    }


    #endregion
}
