using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public static class EventsManager {

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
    /// The Pong Kong Update event message byte. Used internally by MasterClient to track the number of players who are unable to Pong/Kong
    /// the latest discard tile.
    /// </summary>
    public const byte EvPongKongUpdate = 11;

    /// <summary>
    /// The Player Win event message byte. Used internally by the local player when a remote player has won.
    /// </summary>
    public const byte EvPlayerWin = 12;

    /// <summary>
    /// The Win Update event message byte. Used internally by MasterClient to track the number of players who are unable to Pong/Kong
    /// </summary>
    public const byte EvWinUpdate = 13;

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


    public static void EventPongKong(bool canPongKong) {
        PhotonNetwork.RaiseEvent(EvPongKongUpdate, canPongKong, new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }


    public static void EventPlayerWin(Dictionary<int, string[]> winInfo) {
        PhotonNetwork.RaiseEvent(EvPlayerWin, winInfo, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
    }


    public static void EventWinUpdate(bool canWin) {
        PhotonNetwork.RaiseEvent(EvWinUpdate, canWin, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
    }

    #endregion 
}
