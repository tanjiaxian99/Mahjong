using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour {

    public enum Wind {
        EAST,
        SOUTH,
        WEST,
        NORTH
    }

    public Wind seatWind { get; set; }

    public bool myTurn = false;

    public bool canTouchHandTiles = false;

    public int numberOfReplacementTiles { get; set; }

    public int numberOfKong { get; set; }

    public int points = 200;

    public string payForAll = "";

    public int fanTotal;

    public List<string> winningCombos;

    /// <summary>
    /// Cache for Chow Combinations return value
    /// </summary>
    public List<object[]> chowTiles;

    #region Managers Initialization

    [SerializeField]
    private GameObject scriptManager;

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private ChowManager chowManager;

    private PongManager pongManager;

    private KongManager kongManager;

    private SacredDiscardManager sacredDiscardManager;

    private MissedDiscardManager missedDiscardManager;
    
    private Payment payment;

    private WinManager winManager;

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
        chowManager = scriptManager.GetComponent<ChowManager>();
        pongManager = scriptManager.GetComponent<PongManager>();
        kongManager = scriptManager.GetComponent<KongManager>();
        sacredDiscardManager = scriptManager.GetComponent<SacredDiscardManager>();
        missedDiscardManager = scriptManager.GetComponent<MissedDiscardManager>();
        payment = scriptManager.GetComponent<Payment>();
        winManager = scriptManager.GetComponent<WinManager>();
    }

    #endregion 

    /// <summary>
    /// Point the camera towards the GameTable and stretch the GameTable to fill up the screen
    /// </summary>
    public void LocalScreenViewAdjustment() {
        Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        Camera camera = Camera.main;
        gameManager.tableHeight = 2f * camera.orthographicSize;
        gameManager.tableWidth = gameManager.tableHeight * camera.aspect;

        // Scale the GameTable along z direction
        gameManager.gameTable.transform.localScale = new Vector3(gameManager.tableWidth, 1, gameManager.tableHeight);
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
        payment.InstantPayout(PhotonNetwork.LocalPlayer, initialOpenTiles, gameManager.turnManager.Turn, gameManager.numberOfTilesLeft, gameManager.isFreshTile, gameManager.discardPlayer, playerManager.seatWind);

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
        if (gameManager.numberOfTilesLeft == 15) {
            EndRound.EndGame(null, 0, null, tilesManager);
            EventsManager.EventEndRound();
            yield break;
        }

        List<Tile> hand = tilesManager.hand;
        if (playerManager.seatWind == PlayerManager.Wind.EAST) {

            // Start of Turn 2 will definitely have at least one discard tile
            if (gameManager.turnManager.Turn == 1 && gameManager.discardTiles.Count == 0) {
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
                gameManager.StartTurn();
                // DEBUG
                gameManager.StartTurn();
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
        gameManager.latestDiscardTile = null;
        gameManager.discardPlayer = null;

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
                    gameManager.nextPlayersTurn();
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
            if (gameManager.numberOfTilesLeft == 15) {
                EndRound.EndGame(null, 0, null, tilesManager);
                EventsManager.EventEndRound();
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

        gameManager.numberOfTilesLeft = tiles.Count;

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
        gameManager.UpdateAllPlayersOpenTiles(PhotonNetwork.LocalPlayer, tilesManager.openTiles);
        payment.InstantPayout(PhotonNetwork.LocalPlayer, tilesManager.openTiles, gameManager.turnManager.Turn, gameManager.numberOfTilesLeft, gameManager.isFreshTile, gameManager.discardPlayer, playerManager.seatWind);

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
        if (gameManager.tableWidth / gameManager.tableHeight < 1.34 && xPos > 0.83 * 6 + 0.30) {
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
        Tile latestDiscardTile = gameManager.latestDiscardTile;

        if (tilesManager.CanPong(latestDiscardTile)) {
            pongManager.PongUI(latestDiscardTile);
            return;
        }

        if (tilesManager.CanDiscardKong(latestDiscardTile)) {
            pongManager.PongUI(latestDiscardTile);
            kongManager.KongUI(new List<Tile>() { latestDiscardTile });
            return;
        }

        missedDiscardManager.UpdateMissedDiscard(gameManager.discardPlayer, latestDiscardTile);

        // Inform Master Client that the local player can't Pong/Kong
        EventsManager.EventCanPongKong(false);
    }
}


