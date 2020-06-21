using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PropertiesManager : MonoBehaviourPunCallbacks {

    #region MonoBehaviour References

    [SerializeField]
    private GameObject scriptManager;

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private Payment payment;

    private WinManager winManager;

    #endregion

    #region PropKeys

    /// <summary>
    /// Dictionary containing actor numbers and wind assignments
    /// </summary>
    public static readonly string WindAllocationPropKey = "wa";

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
    /// The latest tile which was Konged
    /// </summary>
    public static readonly string SpecialTilePropKey = "kt";

    /// <summary>
    /// Number of tiles in the player's hand
    /// </summary>
    public static readonly string HandTilesCountPropKey = "ht";

    /// <summary>
    /// The local player's hand tiles
    /// </summary>
    public static readonly string OpenHandPropKey = "oh";

    /// <summary>
    /// The local player's open tiles
    /// </summary>
    public static readonly string OpenTilesPropKey = "ot";

    /// <summary>
    /// The next player to play
    /// </summary>
    public static readonly string NextPlayerPropKey = "np";

    /// <summary>
    /// The player that has to pay for all players
    /// </summary>
    public static readonly string PayAllPlayerPropKey = "pa";

    #endregion

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
        payment = scriptManager.GetComponent<Payment>();
        winManager = scriptManager.GetComponent<WinManager>();
    }

    #region Set Properties Methods

    public static void SetWindAllocation(Dictionary<int, int> windsAllocation) {
        Hashtable ht = new Hashtable();
        ht.Add(WindAllocationPropKey, windsAllocation);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }
    
    public static void SetPlayerWind(PlayerManager.Wind wind) {
        Hashtable ht = new Hashtable();
        ht.Add(PlayerWindPropKey, wind);
        PhotonNetwork.SetPlayerCustomProperties(ht);
    }

    public static void SetPlayOrder(Player[] playOrder) {
        Hashtable ht = new Hashtable();
        ht.Add(PlayOrderPropkey, playOrder);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public static void SetWallTileList(List<Tile> tiles) {
        Hashtable ht = new Hashtable();
        ht.Add(WallTileListPropKey, tiles);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public static void SetDiscardTile(Tuple<int, Tile, float> tuple) {
        Hashtable ht = new Hashtable();
        ht.Add(DiscardTilePropKey, tuple);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public static void SetSpecialTile(Tuple<int, Tile, float> tuple) {
        Hashtable ht = new Hashtable();
        ht.Add(SpecialTilePropKey, tuple);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public static void SetHandTilesCount(int handTilesCount) {
        Hashtable ht = new Hashtable();
        ht.Add(HandTilesCountPropKey, handTilesCount);
        PhotonNetwork.SetPlayerCustomProperties(ht);
    }

    public static void SetOpenHand(List<Tile> hand) {
        Hashtable ht = new Hashtable();
        ht.Add(OpenHandPropKey, hand);
        PhotonNetwork.SetPlayerCustomProperties(ht);
    }

    public static void SetOpenTiles(List<Tile> openTiles) {
        Hashtable ht = new Hashtable();
        ht.Add(OpenTilesPropKey, openTiles);
        PhotonNetwork.SetPlayerCustomProperties(ht);
    }

    public static void SetNextPlayer(Player player) {
        Hashtable ht = new Hashtable();
        ht.Add(NextPlayerPropKey, player);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public static void SetPayAllPlayer(Player discardPlayer) {
        Hashtable ht = new Hashtable();
        ht.Add(PayAllPlayerPropKey, discardPlayer);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    } 
    
    #endregion

    #region Retrieve Properties Methods

    public static Dictionary<int, int> GetWindAllocation() {
        return (Dictionary<int, int>)PhotonNetwork.CurrentRoom.CustomProperties[WindAllocationPropKey];
    }

    public static Player[] GetPlayOrder() {
        return (Player[])PhotonNetwork.CurrentRoom.CustomProperties[PlayOrderPropkey];
    }

    public static List<Tile> GetWallTileList() {
        return (List<Tile>)PhotonNetwork.CurrentRoom.CustomProperties[WallTileListPropKey];
    }

    public static Tuple<int, Tile, float> GetDiscardTile() {
        return (Tuple<int, Tile, float>)PhotonNetwork.CurrentRoom.CustomProperties[DiscardTilePropKey];
    }

    public static int GetHandTilesCount(Player player) {
        return (int)player.CustomProperties[HandTilesCountPropKey];
    }

    public static List<Tile> GetOpenHand(Player player) {
        return (List<Tile>)player.CustomProperties[OpenHandPropKey];
    }

    public static List<Tile> GetOpenTiles(Player player) {
        return (List<Tile>)player.CustomProperties[OpenTilesPropKey];
    }

    public static Player GetNextPlayer() {
        return (Player)PhotonNetwork.CurrentRoom.CustomProperties[NextPlayerPropKey];
    }

    public static Player GetPayAllPlayer() {
        return (Player)PhotonNetwork.CurrentRoom.CustomProperties[PayAllPlayerPropKey];
    }


    #endregion

    #region Properties Update

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
        if (propertiesThatChanged.ContainsKey(WindAllocationPropKey)) {
            DictManager.Instance.windsAllocation = GetWindAllocation();
            gameManager.windsDict = GetWindAllocation();
            PlayerManager.Wind wind = (PlayerManager.Wind)gameManager.windsDict[PhotonNetwork.LocalPlayer.ActorNumber];

            // Update local player's custom properties
            SetPlayerWind(wind);
            // Update local player's playerManager
            playerManager.seatWind = wind;

            // Initialize Instant Payment dictionary
            this.payment.InitializeInstantPaymentDict(PhotonNetwork.PlayerList.ToList());

        } else if (propertiesThatChanged.ContainsKey(DiscardTilePropKey)) {
            Tuple<int, Tile, float> discardTileInfo = (Tuple<int, Tile, float>)PhotonNetwork.CurrentRoom.CustomProperties[DiscardTilePropKey];

            // Item1 = -1 when the latest discard tile is to be removed, due to Chow, Pong, Kong or Win
            // Item1 = -2 when a player has drawn a tile and both discardPlayer and latestDiscardTile can be reset to null;
            if (discardTileInfo.Item1 == -1) {
                // Remove the last discard tile
                GameObject lastDiscardTile = GameObject.FindGameObjectWithTag("Discard");
                Destroy(lastDiscardTile);
                gameManager.discardTiles.RemoveAt(gameManager.discardTiles.Count - 1);
                return;

            } else if (discardTileInfo.Item1 == -2) {
                gameManager.discardPlayer = null;
                gameManager.latestDiscardTile = null;
                return;
            }

            gameManager.discardPlayer = PhotonNetwork.CurrentRoom.GetPlayer(discardTileInfo.Item1);
            gameManager.latestDiscardTile = discardTileInfo.Item2;
            float hPos = discardTileInfo.Item3;

            gameManager.isFreshTile = FreshTileDiscard.IsFreshTile(gameManager.discardTiles, gameManager.AllPlayersOpenTiles(), gameManager.latestDiscardTile);
            gameManager.discardTiles.Add(gameManager.latestDiscardTile);

            // Only instantiate the tile if a remote player threw it
            if (gameManager.discardPlayer == PhotonNetwork.LocalPlayer) {
                return;
            }

            RemotePlayer.InstantiateRemoteDiscardTile(gameManager, gameManager.discardPlayer, gameManager.latestDiscardTile, hPos);

            // Check to see if the player can win based on the discard tile
            if (winManager.CanWin()) {
                winManager.WinUI();
                return;
            } else {
                // Inform the Master Client that the player can't win
                EventsManager.EventWinUpdate(false);
            }

        } else if (propertiesThatChanged.ContainsKey(SpecialTilePropKey)) {
            Tuple<int, Tile, float> discardTileInfo = (Tuple<int, Tile, float>)PhotonNetwork.CurrentRoom.CustomProperties[SpecialTilePropKey];

            if (discardTileInfo == null) {
                gameManager.kongPlayer = null;
                gameManager.latestKongTile = null;
                return;
            }

            if (discardTileInfo.Item3 == 1) {
                gameManager.bonusPlayer = PhotonNetwork.CurrentRoom.GetPlayer(discardTileInfo.Item1);
                gameManager.latestBonusTile = discardTileInfo.Item2;

                if (gameManager.bonusPlayer == PhotonNetwork.LocalPlayer) {
                    return;
                }

                if (winManager.CanWin("Bonus")) {
                    winManager.WinUI();
                }
                return;

            } else if (discardTileInfo.Item3 == 2) {
                gameManager.kongPlayer = PhotonNetwork.CurrentRoom.GetPlayer(discardTileInfo.Item1);
                gameManager.latestKongTile = discardTileInfo.Item2;

                if (gameManager.kongPlayer == PhotonNetwork.LocalPlayer) {
                    return;
                }

                if (winManager.CanWin("Kong")) {
                    winManager.WinUI();
                }
                return;

            } else if (discardTileInfo.Item3 == 3) {
                gameManager.kongPlayer = PhotonNetwork.CurrentRoom.GetPlayer(discardTileInfo.Item1);
                gameManager.latestKongTile = discardTileInfo.Item2;

                if (gameManager.kongPlayer == PhotonNetwork.LocalPlayer) {
                    return;
                }

                if (winManager.CanWin("Kong")) {
                    if (playerManager.winningCombos.Contains("Thirteen Wonders")) {
                        winManager.WinUI();
                    }
                }
                return;
            }

            if (PhotonNetwork.IsMasterClient) {
                SetSpecialTile(null);
            }

        } else if (propertiesThatChanged.ContainsKey(WallTileListPropKey)) {
            gameManager.numberOfTilesLeft = GetWallTileList().Count;

            //// DEBUG
            //numberOfTilesLeft = 50;

            if (gameManager.numberOfTilesLeft == 15) {
                EndRound.EndGame(null, 0, null, tilesManager);
                EventsManager.EventEndRound();
            }

        } else if (propertiesThatChanged.ContainsKey(PayAllPlayerPropKey)) {
            Player player = GetPayAllPlayer();
            if (player == PhotonNetwork.LocalPlayer) {
                playerManager.payForAll = "Local";
            } else {
                playerManager.payForAll = "Remote";
            }
        }
    }


    /// <summary>
    /// Called when a remote player's hand or open tiles changes
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
        if (changedProps.ContainsKey(HandTilesCountPropKey) && targetPlayer != PhotonNetwork.LocalPlayer) {
            RemotePlayer.InstantiateRemoteHand(gameManager, targetPlayer);

        } else if (changedProps.ContainsKey(OpenHandPropKey) && targetPlayer != PhotonNetwork.LocalPlayer) {
            RemotePlayer.InstantiateRemoteOpenHand(gameManager, targetPlayer);

        } else if (changedProps.ContainsKey(OpenTilesPropKey) && targetPlayer != PhotonNetwork.LocalPlayer) {
            RemotePlayer.InstantiateRemoteOpenTiles(gameManager, payment, targetPlayer);
        }
    }

    #endregion
}
