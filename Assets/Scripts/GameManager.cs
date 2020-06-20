using ExitGames.Client.Photon;
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

    private TilesManager tilesManager;

    [Tooltip("The GameObject used to represent a physical Mahjong table")]
    [SerializeField]
    private GameObject gameTable;

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

    private static readonly object syncLock = new object();

    public PunTurnManager turnManager;

    /// <summary>
    /// Dictionary containing a player's actor number and his playerWind in integer form
    /// </summary>
    public Dictionary<int, int> windsDict = new Dictionary<int, int>();

    /// <summary>
    /// Cache for Chow Combinations return value
    /// </summary>
    public List<object[]> chowTiles;

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

    public PayAllDiscard payAllDiscard;

    public Payment payment;

    public List<Tile> discardTiles;

    public Player kongPlayer;

    public Tile latestKongTile;

    public Player bonusPlayer;

    public Tile latestBonusTile;

    public bool isFreshTile;

    public ChowManager chowManager;

    public PongManager pongManager;

    public KongManager kongManager;

    public SacredDiscardManager sacredDiscardManager;

    public MissedDiscardManager missedDiscardManager;

    public WinManager winManager;

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
            this.OnLocalPlayerMove();
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
        tilesManager = scriptManager.GetComponent<TilesManager>();
        chowManager = scriptManager.GetComponent<ChowManager>();
        pongManager = scriptManager.GetComponent<PongManager>();
        kongManager = scriptManager.GetComponent<KongManager>();
        sacredDiscardManager = scriptManager.GetComponent<SacredDiscardManager>();
        missedDiscardManager = scriptManager.GetComponent<MissedDiscardManager>();
        payment = scriptManager.GetComponent<Payment>();
        winManager = scriptManager.GetComponent<WinManager>();

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
    /// Called by the local player to display the initial hand before Bonus Tile have been converted
    /// </summary>
    public void LocalHiddenPayouts() {
        if (tilesManager.hand == null) {
            Debug.LogError("The player's hand is empty.");
        }

        if (tilesManager.openTiles.Any()) {
            Debug.LogError("The player has open tiles leftover from the previous game.");
        }

        List<Tile> initialOpenTiles = new List<Tile>();
        foreach (Tile tile in tilesManager.hand) {
            if (tile.IsBonus()) {
                initialOpenTiles.Add(tile);
            }
        }
        // Local Hidden Payout check
        payment.InstantPayout(PhotonNetwork.LocalPlayer, initialOpenTiles, turnManager.Turn, numberOfTilesLeft, isFreshTile, discardPlayer, playerManager.seatWind);

        // Remote Hidden Payout check
        PropertiesManager.SetOpenTiles(initialOpenTiles);
    }


    /// <summary>
    /// Called by the local player once Hidden Instant Payouts have been settled. Bonus tiles are converted
    /// </summary>
    public void InitialLocalInstantiation() {
        // Check the local player's hand for bonus tiles. If there are, convert them to normal tiles.
        while (true) {
            bool haveBonusTile = false;

            for (int i = 0; i < tilesManager.hand.Count; i++) {
                if (tilesManager.hand[i].IsBonus()) {
                    // Add tile to bonus tiles list, which are instantiated separately
                    tilesManager.bonusTiles.Add(tilesManager.hand[i]);
                    tilesManager.hand[i] = this.DrawTile();
                    haveBonusTile = true;
                }
            }

            if (!haveBonusTile) {
                break;
            }
        }

        // Initial sort. Afterwards, hand will only be sorted after discarding a tile.
        tilesManager.hand = tilesManager.hand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();

        this.InstantiateLocalHand();
        this.InstantiateLocalOpenTiles();
    }


    /// <summary>
    /// Called immediately when myTurn is set to true. The only times it is not called is when the East Wind makes the first move, or
    /// when the player Pong or Kong
    /// </summary>
    public IEnumerator OnPlayerTurn() {
        // If there are only 15 tiles left, end the game
        if (numberOfTilesLeft == 15) {
            this.EndRound();
            yield break;
        }

        List<Tile> hand = tilesManager.hand;
        if (playerManager.seatWind == PlayerManager.Wind.EAST) {

            // Start of Turn 2 will definitely have at least one discard tile
            if (turnManager.Turn == 1 && discardTiles.Count == 0) {
                // Check to see if the player can win based on the East Wind's initial 14 tiles
                if (winManager.CanWin()) {
                    winManager.WinUI();
                    yield break;
                }

                if (tilesManager.ConcealedKongTiles().Count != 0) {
                    kongManager.KongUI(tilesManager.ConcealedKongTiles());
                }
                yield break;
            } else {
                this.StartTurn();
                // DEBUG
                this.StartTurn();
                // The wait ensures the local player's turn number is updated
                yield return new WaitForSeconds(0.2f);
            }
        }


        // Check if the discarded tile could be Chowed
        Tuple<int, Tile, float> discardTileInfo = PropertiesManager.GetDiscardTile();
        Tile tile = discardTileInfo.Item2;
        chowTiles = tilesManager.ChowCombinations(tile);

        if (chowTiles.Count != 0) {
            chowManager.ChowUI(chowTiles);
            yield break;
        }

        hand.Add(this.DrawTile());
        latestDiscardTile = null;
        discardPlayer = null;

        this.ConvertLocalBonusTiles();
        this.InstantiateLocalHand();
        this.InstantiateLocalOpenTiles();

        // Check if the player can win based on the drawn tile
        if (winManager.CanWin()) {
            winManager.WinUI();
            yield break;
        }

        // Check if the player can Kong the drawn tile
        if (tilesManager.ExposedKongTiles().Count != 0 || tilesManager.ConcealedKongTiles().Count != 0) {
            kongManager.KongUI(tilesManager.ExposedKongTiles().Concat(tilesManager.ConcealedKongTiles()).ToList());
        }
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

                if (tilesManager.hand.Contains(tile)) {
                    playerManager.numberOfReplacementTiles = 0;
                    playerManager.numberOfKong = 0;

                    playerManager.myTurn = false;
                    playerManager.canTouchHandTiles = false;
                    tilesManager.hand.Remove(tile);

                    this.InstantiateLocalHand();
                    this.DiscardTile(tile, hitObject.transform.position.x);
                    missedDiscardManager.ResetMissedDiscard();
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
        List<Tile> hand = tilesManager.hand;

        while (true) {
            // If there are only 15 tiles left, end the game
            if (numberOfTilesLeft == 15) {
                this.EndRound();
                return;
            }

            Tile tile = hand[hand.Count - 1];

            // Check if the tile is a bonusTile
            if (!tile.IsBonus()) {
                break;
            }
            tilesManager.bonusTiles.Add(tile);

            hand[hand.Count - 1] = this.DrawTile();
            playerManager.numberOfReplacementTiles++;
        }
    }


    /// <summary>
    /// Draw a new tile. No distinction made between front end or back end of Wall Tiles.
    /// </summary>
    public Tile DrawTile() {
        // DEBUG 
        List<Tile> tiles = PropertiesManager.GetWallTileList();

        Tile tile = tiles[0];
        tiles.Remove(tiles[0]);

        numberOfTilesLeft = tiles.Count;

        // Reinsert updated tiles list into Room Custom Properties
        PropertiesManager.SetWallTileList(tiles);

        // Inform remote players that the local player has drawn a tile, and discardTile and discardPlayer can be reset to null
        PropertiesManager.SetDiscardTile(new Tuple<int, Tile, float>(-2, new Tile(0, 0), 0));

        // Inform remote players that the local player has drawn a bonus tile. 
        if (tile.IsBonus()) {
            PropertiesManager.SetSpecialTile(new Tuple<int, Tile, float>(PhotonNetwork.LocalPlayer.ActorNumber, tile, 1));
        }

        return tile;

        //List<Tile> tiles = (List<Tile>)PhotonNetwork.CurrentRoom.CustomProperties[WallTileListPropKey];

        //int randomIndex = RandomNumber(tiles.Count());
        //Tile tile = tiles[randomIndex];
        //tiles.Remove(tiles[randomIndex]);

        //numberOfTilesLeft = tiles.Count;

        //// Reinsert updated tiles list into Room Custom Properties
        //Hashtable ht = new Hashtable();
        //ht.Add(WallTileListPropKey, tiles);
        //PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

        //// Inform remote players that the local player has drawn a tile, and discardTile and discardPlayer can be reset to null
        //ht = new Hashtable();
        //ht.Add(DiscardTilePropKey, new Tuple<int, Tile, float>(-2, new Tile(0, 0), 0));
        //PhotonNetwork.CurrentRoom.SetCustomProperties(ht);

        //// Inform remote players that the local player has drawn a bonus tile. 
        //if (tile.IsBonus()) {
        //    ht = new Hashtable();
        //    ht.Add(SpecialTilePropKey, new Tuple<int, Tile, float>(PhotonNetwork.LocalPlayer.ActorNumber, tile, 1));
        //    PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        //}

        //return tile;
    }


    /// <summary>
    /// Called whenever there is an update to the player's hand.
    /// </summary>
    public void InstantiateLocalHand() {
        // Update the local player's hand count custom property
        PropertiesManager.SetHandTilesCount(tilesManager.hand.Count);

        // Separation between pivot of tiles
        float xSepHand = 0.83f;
        // x-coordinate of the position where the tile will be instantiated
        float xPosHand = 0;
        // The offset specifically for the drawn tile
        float xOffset = 0.30f;
        int handSize = tilesManager.hand.Count;

        // taggedHand represents the tiles currently on the GameTable. It represents the hand one move prior to the current hand.
        GameObject[] taggedHand = GameObject.FindGameObjectsWithTag("Hand");

        foreach (GameObject gameObject in taggedHand) {
            Destroy(gameObject);
        }

        if ((handSize + 1) % 3 == 0) {
            xPosHand = -(handSize - 2) / 2f * xSepHand;

        } else {
            xPosHand = -(handSize - 1) / 2f * xSepHand;
            // Sort the player's hand after discarding a tile
            tilesManager.hand = tilesManager.hand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();
        }

        for (int i = 0; i < handSize; i++) {
            // If the last tile of the hand is a drawn tile, instantiate it away from the other tiles.
            if (i == handSize - 1 && (handSize + 1) % 3 == 0) {
                xPosHand += xOffset;
            }
            
            GameObject newTile = Instantiate(DictManager.Instance.tilesDict[tilesManager.hand[i]], new Vector3(xPosHand, 0.85f, -4.4f), Quaternion.Euler(270f, 180f, 0f));
            newTile.tag = "Hand";

            xPosHand += xSepHand;
        }

    }


    /// <summary>
    /// Instantiate the open tiles of the local player. Called when there is an update to the bonusTiles/comboTiles list.
    /// </summary>
    public void InstantiateLocalOpenTiles() {
        tilesManager.UpdateOpenTiles();
        this.UpdateAllPlayersOpenTiles(PhotonNetwork.LocalPlayer, tilesManager.openTiles);
        payment.InstantPayout(PhotonNetwork.LocalPlayer, tilesManager.openTiles, turnManager.Turn, numberOfTilesLeft, isFreshTile, discardPlayer, playerManager.seatWind);

        // Update the list of open tiles on the local player's custom properties
        PropertiesManager.SetOpenTiles(tilesManager.openTiles);

        int openSize = tilesManager.openTiles.Count;
        foreach (Tile tile in tilesManager.openTiles) {
            if (tile.kongType == 3) {
                openSize -= 1;
            }
        }

        float xSepOpen = 0.83f * 0.5f;
        float xPosOpen = -(openSize - 1) / 2f * xSepOpen;

        // taggedOpen represents the tiles currently on the GameTable. It represents the hand one move prior to the current hand.
        GameObject[] taggedOpen = GameObject.FindGameObjectsWithTag("Open");

        foreach (GameObject gameObject in taggedOpen) {
            Destroy(gameObject);
        }

        foreach (Tile tile in tilesManager.openTiles) {
            GameObject newTile;

            // Instantiate the last Concealed Kong tile one tile above the other 3 Concealed Kong tiles.
            if (tile.kongType == 3) {
                xPosOpen -= xSepOpen;
                newTile = Instantiate(DictManager.Instance.tilesDict[tile], new Vector3(xPosOpen, 1f + 0.3f, -3.5f), Quaternion.Euler(270f, 180f, 0f));
            } else {
                newTile = Instantiate(DictManager.Instance.tilesDict[tile], new Vector3(xPosOpen, 1f, -3.5f), Quaternion.Euler(270f, 180f, 0f));
            }

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
        // Update the properties of the discard tile
        PropertiesManager.SetDiscardTile(new Tuple<int, Tile, float>(PhotonNetwork.LocalPlayer.ActorNumber, tile, xPos));

        // The tile is now the Sacred Discard
        sacredDiscardManager.sacredDiscard = tile;

        // Remove the tag of the tile discarded before the current tile
        GameObject previousDiscard = GameObject.FindGameObjectWithTag("Discard");
        if (previousDiscard != null) {
            previousDiscard.tag = "Untagged";
        }

        GameObject tileGameObject;
        // For the edge case where the local client aspect ratio is 4:3 and the 14th tile is discarded
        if (tableWidth / tableHeight < 1.34 && xPos > 0.83 * 6 + 0.30) {
            tileGameObject = Instantiate(DictManager.Instance.tilesDict[tile], new Vector3(xPos, 0.65f, -3.5f), Quaternion.Euler(270f, 180f, 0f));
        } else {
            tileGameObject = Instantiate(DictManager.Instance.tilesDict[tile], new Vector3(xPos, 0.65f, -2.8f), Quaternion.Euler(270f, 180f, 0f));
        }

        tileGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        // Tagging is necessary to reference the last tile discarded when Chow/Pong/Kong is called
        tileGameObject.tag = "Discard";

        foreach (Transform child in tileGameObject.transform) {
            child.GetComponent<MeshCollider>().convex = true;
        }

        // tan(α) = zPos / xPos = zForce / xForce
        // xForce ** 2 + zforce ** 2 = rForce ** 2
        // Small offsets have been added to xForce and zForce to give more force to tiles tossed from the sides
        double zPos = 2.8;
        double rForce = 9;
        double tanα = zPos / (xPos + 0.1);
        double xForce = Math.Sqrt(Math.Pow(rForce, 2) / (1 + Math.Pow(tanα, 2))) + Math.Abs(xPos / 3f);
        double zForce = Math.Abs(xForce * tanα) + Math.Pow(xPos / 3.5d, 2);
        if (xPos > 0) {
            xForce = -xForce;
        }

        Rigidbody rb = tileGameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;
        rb.AddForce(new Vector3((float)xForce, 0f, (float)zForce), ForceMode.VelocityChange);
    }


    /// <summary>
    /// Checks to see if the local player can Pong/Kong.
    /// </summary>
    public void CheckPongKong() {
        // Check for Pong/Kong against discard tile
        if (tilesManager.CanPong(latestDiscardTile)) {
            pongManager.PongUI(latestDiscardTile);
            return;
        }

        if (tilesManager.CanDiscardKong(latestDiscardTile)) {
            pongManager.PongUI(latestDiscardTile);
            kongManager.KongUI(new List<Tile>() { latestDiscardTile });
            return;
        }

        missedDiscardManager.UpdateMissedDiscard(discardPlayer, latestDiscardTile);

        // Inform Master Client that the local player can't Pong/Kong
        EventsManager.EventCanPongKong(false);
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

    #endregion

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
