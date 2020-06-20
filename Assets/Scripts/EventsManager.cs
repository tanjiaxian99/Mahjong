using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EventsManager : MonoBehaviourPunCallbacks, IOnEventCallback {
    #region MonoBehaviour References

    [SerializeField]
    private GameObject scriptManager;

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private WinManager winManager;

    #endregion

    #region Ev Bytes

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
    /// The Hidden Payouts event message byte. Used internally for checking for Hidden Payouts in local and remote tiles.
    /// </summary>
    public const byte EvHiddenPayouts = 6;

    /// <summary>
    /// The Player Initialization event message byte. Used internally for converting the local player's bonus tiles into normal tiles.
    /// Afterwards, instantiate the local player's hand and open tiles
    /// </summary>
    public const byte EvPlayerInitialization = 7;

    /// <summary>
    /// The Player Turn event message byte. Used internally by MasterClient to update the next player on his turn.
    /// </summary>
    public const byte EvPlayerTurn = 8;

    /// <summary>
    /// The Check Pong Kong event message byte. Used internally to check whether the local player can Pong/Kong.
    /// </summary>
    public const byte EvCheckPongKong = 9;

    /// <summary>
    /// The Can Pong Kong event message byte. Used internally by MasterClient to track the number of players who are unable to Pong/Kong
    /// the latest discard tile.
    /// </summary>
    public const byte EvCanPongKong = 10;

    /// <summary>
    /// The Player Win event message byte. Used internally by the local player when a remote player has won.
    /// </summary>
    public const byte EvPlayerWin = 11;

    /// <summary>
    /// The Win Update event message byte. Used internally by MasterClient to track the number of players who are unable to Pong/Kong
    /// </summary>
    public const byte EvWinUpdate = 12;

    #endregion

    /// <summary>
    /// A list used by MasterClient to track whether players can/want to Pong/Kong
    /// </summary>
    public List<bool> PongKongUpdateList;

    /// <summary>
    /// A list used by MasterClient to track the actor numbers of players who didn't discard the tile
    /// </summary>
    public List<int> nonDiscardActorNumbers;

    /// <summary>
    /// A list used by MasterClient to track whether players can/want to win
    /// </summary>
    public List<bool> winUpdateList;

    #region MonoBehaviourPunCallbacks Callbacks

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
        winManager = scriptManager.GetComponent<WinManager>();

        PongKongUpdateList = new List<bool>();
        nonDiscardActorNumbers = new List<int>();
        winUpdateList = new List<bool>();
    }

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

    #endregion

    #region Raise Events

    /// <summary>
    /// Raise an event telling all players to point their camera towards the GameTable and stretch the GameTable
    /// </summary>
    public static void EventScreenViewAdjustment() {
        PhotonNetwork.RaiseEvent(EvScreenViewAdjustment, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }


    public static void EventDistributeTiles(Player player, List<Tile> playerTiles) {
        PhotonNetwork.RaiseEvent(EvDistributeTiles, playerTiles, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
    }


    /// <summary>
    /// Raise an event to inform all players to update their initial open tiles
    /// </summary>
    public static void EventHiddenPayouts() {
        PhotonNetwork.RaiseEvent(EvHiddenPayouts, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }


    public static void EventPlayerInitialization(Player player) {
        PhotonNetwork.RaiseEvent(EvPlayerInitialization, null, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
    }


    public static void EventPlayerTurn(Player player) {
        PhotonNetwork.RaiseEvent(EvPlayerTurn, null, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
    }


    public static void EventCheckPongKong(List<int> nonDiscardActorNumbers) {
        PhotonNetwork.RaiseEvent(EvCheckPongKong, null, new RaiseEventOptions() { TargetActors = nonDiscardActorNumbers.ToArray() }, SendOptions.SendReliable);
    }


    public static void EventCanPongKong(bool canPongKong) {
        PhotonNetwork.RaiseEvent(EvCanPongKong, canPongKong, new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }


    public static void EventPlayerWin(Dictionary<int, string[]> winInfo) {
        PhotonNetwork.RaiseEvent(EvPlayerWin, winInfo, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
    }


    public static void EventWinUpdate(bool canWin) {
        PhotonNetwork.RaiseEvent(EvWinUpdate, canWin, new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    #endregion 

    public void OnEvent(EventData photonEvent) {
        byte eventCode = photonEvent.Code;

        switch (eventCode) {
            case EvScreenViewAdjustment:
                gameManager.LocalScreenViewAdjustment();
                break;

            case EvDistributeTiles:
                // Update local player's playerManager
                tilesManager.hand = (List<Tile>)photonEvent.CustomData;
                break;

            case EvHiddenPayouts:
                gameManager.LocalHiddenPayouts();
                break;

            case EvPlayerInitialization:
                gameManager.InitialLocalInstantiation();
                break;

            case EvPlayerTurn:
                playerManager.myTurn = true;
                playerManager.canTouchHandTiles = true;
                StartCoroutine(gameManager.OnPlayerTurn());
                break;

            case EvCheckPongKong:
                gameManager.CheckPongKong();
                break;

            case EvCanPongKong:
                if (!PhotonNetwork.IsMasterClient) {
                    return;
                }
                bool pongKongUpdate = (bool)photonEvent.CustomData;
                PongKongUpdateList.Add(pongKongUpdate);

                // If none of the players can/wants to Pong/Kong, start the next player's turn.
                bool allFalse = true;
                if (PongKongUpdateList.Count == 3) {
                    foreach (bool update in PongKongUpdateList) {
                        if (update) {
                            allFalse = false;
                        }
                    }

                    if (allFalse) {
                        Player nextPlayer = PropertiesManager.GetNextPlayer();
                        EventPlayerTurn(nextPlayer);
                    }

                    PongKongUpdateList.Clear();
                }
                break;

            case EvPlayerWin:
                Debug.LogError(PhotonNetwork.IsMasterClient);
                Dictionary<int, string[]> winInfo = (Dictionary<int, string[]>)photonEvent.CustomData;
                winInfo.Values.ToList()[0].ToList().ForEach(Debug.LogError);
                Player sender = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
                winManager.RemoteWin(sender, winInfo.Keys.ToList()[0], winInfo.Values.ToList()[0].ToList());
                break;

            case EvWinUpdate:
                if (!PhotonNetwork.IsMasterClient) {
                    return;
                }
                bool winUpdate = (bool)photonEvent.CustomData;
                winUpdateList.Add(winUpdate);
                nonDiscardActorNumbers.Add(photonEvent.Sender);

                // If none of the players can/wants to win from the discard, players can then check for Pong/Kong
                bool allWinFalse = true;
                if (winUpdateList.Count == 3) {
                    foreach (bool update in winUpdateList) {
                        if (update) {
                            allWinFalse = false;
                        }
                    }

                    if (allWinFalse) {
                        EventsManager.EventCheckPongKong(nonDiscardActorNumbers);
                    }
                    winUpdateList.Clear();
                    nonDiscardActorNumbers.Clear();
                }
                break;
        }
    }
}
