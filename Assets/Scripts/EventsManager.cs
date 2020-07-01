using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Runtime.InteropServices.ComTypes;
using System;
using Photon.Pun.UtilityScripts;

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
    /// The Initial Points event message byte. Used internally to set the initial points of the player.
    /// </summary>
    public const byte EvInitialPoints = 5;

    /// <summary>
    /// The Distribute Tiles event message byte. Used internally for saving data in local player's PlayerManager.
    /// </summary>
    public const byte EvDistributeTiles = 6;

    /// <summary>
    /// The Hidden Payouts event message byte. Used internally for checking for Hidden Payouts in local and remote tiles.
    /// </summary>
    public const byte EvHiddenPayouts = 7;

    /// <summary>
    /// The Player Initialization event message byte. Used internally for converting the local player's bonus tiles into normal tiles.
    /// Afterwards, instantiate the local player's hand and open tiles
    /// </summary>
    public const byte EvPlayerInitialization = 8;

    /// <summary>
    /// The Player Turn event message byte. Used internally by MasterClient to update the next player on his turn.
    /// </summary>
    public const byte EvPlayerTurn = 9;

    /// <summary>
    /// The Check Pong Kong event message byte. Used internally to check whether the local player can Pong/Kong.
    /// </summary>
    public const byte EvCheckPongKong = 10;

    /// <summary>
    /// The Can Pong Kong event message byte. Used internally by MasterClient to track the number of players who are unable to Pong/Kong.
    /// the latest discard tile.
    /// </summary>
    public const byte EvCanPongKong = 11;

    /// <summary>
    /// The Player Win event message byte. Used internally by the local player when a remote player has won.
    /// </summary>
    public const byte EvPlayerWin = 12;

    /// <summary>
    /// The Win Update event message byte. Used internally by MasterClient to inform the next player to check for a win.
    /// </summary>
    public const byte EvWinUpdate = 13;

    /// <summary>
    /// The Check Win event mesage byte. Used internally to check if the local player can win.
    /// </summary>
    public const byte EvCheckWin = 14;

    /// <summary>
    /// The End Round event message byte. Used internally by the local player when a remote player ends the round.
    /// </summary>
    public const byte EvEndRound = 15;

    /// <summary>
    /// The Ready For New Round event message byte. Used internally by the local player to signify that he/she is ready to start a new round.
    /// </summary>
    public const byte EvReadyForNewRound = 16;

    /// <summary>
    /// The Start New Round event message byte. Used internally by the local player to start a new round.
    /// </summary>
    public const byte EvStartNewRound = 17;

    /// <summary>
    /// The End Game event message byte. Used internally by all players to end the game.
    /// </summary>
    public const byte EvEndGame = 18;

    #endregion

    /// <summary>
    /// A list used by MasterClient to track whether players can/want to Pong/Kong
    /// </summary>
    private static List<bool> PongKongUpdateList;

    /// <summary>
    /// A list used by MasterClient to track the actor numbers of players who didn't discard the tile
    /// </summary>
    private static List<int> nonDiscardActorNumbers;

    /// <summary>
    /// A list used by MasterClient to track whether players can/want to win
    /// </summary>
    private static int winUpdateCount;

    /// <summary>
    /// A counter used by MasterClient to track the number of players that is ready to start a new round.
    /// </summary>
    private static int startNewRoundCount;

    #region MonoBehaviourPunCallbacks Callbacks

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
        winManager = scriptManager.GetComponent<WinManager>();

        PongKongUpdateList = new List<bool>();
        nonDiscardActorNumbers = new List<int>();
        winUpdateCount = 0;
        startNewRoundCount = 0;
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

    public static void EventInitialPoints() {
        PhotonNetwork.RaiseEvent(EvInitialPoints, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

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

    public static void EventWinUpdate(string winUpdate) {
        PhotonNetwork.RaiseEvent(EvWinUpdate, winUpdate, new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    public static void EventCheckWin(Player player) {
        PhotonNetwork.RaiseEvent(EvCheckWin, null, new RaiseEventOptions() { TargetActors = new int[] { player.ActorNumber } }, SendOptions.SendReliable);
    }

    public static void EventEndRound() {
        PhotonNetwork.RaiseEvent(EvEndRound, null, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
    }

    public static void EventReadyForNewRound() {
        PhotonNetwork.RaiseEvent(EvReadyForNewRound, null, new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    public static void EventStartNewRound() {
        PhotonNetwork.RaiseEvent(EvStartNewRound, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public static void EventEndGame() {
        PhotonNetwork.RaiseEvent(EvEndGame, null, new RaiseEventOptions() { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    #endregion 

    public void OnEvent(EventData photonEvent) {
        byte eventCode = photonEvent.Code;

        switch (eventCode) {
            case EvInitialPoints:
                playerManager.Points = 200;
                break;

            case EvScreenViewAdjustment:
                playerManager.LocalScreenViewAdjustment();
                break;

            case EvDistributeTiles:
                // Update local player's playerManager
                tilesManager.hand = (List<Tile>)photonEvent.CustomData;
                break;

            case EvHiddenPayouts:
                playerManager.LocalHiddenPayouts();
                break;

            case EvPlayerInitialization:
                playerManager.InitialLocalInstantiation();
                break;

            case EvPlayerTurn:
                playerManager.myTurn = true;
                playerManager.canTouchHandTiles = true;
                StartCoroutine(playerManager.OnPlayerTurn());
                break;

            case EvWinUpdate:
                if (!PhotonNetwork.IsMasterClient) {
                    return;
                }
                string winUpdate = (string)photonEvent.CustomData;
                string tileType = PropertiesManager.GetTileType();
                Player discardPlayer = null;

                if (winUpdate == "Start Win Check") {
                    PropertiesManager.SetTouchTiles(false);

                    if (tileType == "Normal") {
                        discardPlayer = gameManager.discardPlayer;
                    } else if (tileType == "Bonus") {
                        discardPlayer = gameManager.bonusPlayer;
                    } else if (tileType == "Exposed Kong") {
                        discardPlayer = gameManager.kongPlayer;
                    } else if (tileType == "Concealed Kong") {
                        discardPlayer = gameManager.kongPlayer;
                    }
                    Player nextPlayer = GameManager.GetNextPlayer(discardPlayer);
                    EventCheckWin(nextPlayer);
                    return;
                }

                winUpdateCount++;
                nonDiscardActorNumbers.Add(photonEvent.Sender);

                if (winUpdateCount < 3) {
                    Player currentPlayer = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
                    Player nextPlayer = GameManager.GetNextPlayer(currentPlayer);
                    EventCheckWin(nextPlayer);
                    return;
                }

                // If none of the players can/wants to win from the discard, players can then check for Pong/Kong
                if (winUpdateCount == 3) {
                    if (tileType == "Normal") {
                        EventCheckPongKong(nonDiscardActorNumbers);

                    } else {
                        PropertiesManager.SetTouchTiles(true);
                    }
                    winUpdateCount = 0;
                    nonDiscardActorNumbers.Clear();
                }
                break;

            case EvCheckWin:
                string type = PropertiesManager.GetTileType();

                if (type == "Normal") {
                    if (winManager.CanWin()) {
                        winManager.WinUI();
                        return;
                    } else {
                        EventWinUpdate("Can't Win");
                    }
                } else if (type == "Bonus") {
                    if (winManager.CanWin("Bonus")) {
                        winManager.WinUI();
                        return;
                    } else {
                        EventWinUpdate("Can't Win");
                    }
                } else if (type == "Exposed Kong") {
                    if (winManager.CanWin("Kong")) {
                        winManager.WinUI();
                        return;
                    } else {
                        EventWinUpdate("Can't Win");
                    }
                } else if (type == "Concealed Kong") {
                    if (winManager.CanWin("Kong")) {
                        if (playerManager.winningCombos.Contains("Thirteen Wonders")) {
                            winManager.WinUI();
                            return;
                        }
                    } 
                    EventWinUpdate("Can't Win");
                }
                break;

            case EvCheckPongKong:
                playerManager.CheckPongKong();
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
                        PropertiesManager.SetTouchTiles(true);
                    }

                    PongKongUpdateList.Clear();
                }
                break;

            case EvPlayerWin:
                Dictionary<int, string[]> winInfo = (Dictionary<int, string[]>)photonEvent.CustomData;
                Player sender = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);
                winManager.RemoteWin(sender, winInfo.Keys.ToList()[0], winInfo.Values.ToList()[0].ToList());
                FinishRound.Instance.EndRound(sender);
                break;

            case EvEndRound:
                FinishRound.Instance.EndRound(null);
                break;

            case EvReadyForNewRound:
                if (!PhotonNetwork.IsMasterClient) {
                    return;
                }
                startNewRoundCount++;
                if (startNewRoundCount == 4) {
                    startNewRoundCount = 0;
                    EventStartNewRound();
                }
                break;

            case EvStartNewRound:
                FinishRound.Instance.StartNewRound();
                break;

            case EvEndGame:
                FinishRound.Instance.EndGame();
                break;
        }
    }

    public static void ResetVariables() {
        PongKongUpdateList.Clear();
        nonDiscardActorNumbers.Clear();
        winUpdateCount = 0;
    }
}
