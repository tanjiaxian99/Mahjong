using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public static class EventsManager {

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


    public static void EventPongKong(bool canPongKong) {
        PhotonNetwork.RaiseEvent(EvPongKongUpdate, canPongKong, new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }


    public static void EventPlayerWin(Dictionary<int, string[]> winInfo) {
        PhotonNetwork.RaiseEvent(EvPlayerWin, winInfo, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
    }


    public static void EventWinUpdate(bool canWin) {
        PhotonNetwork.RaiseEvent(EvWinUpdate, canWin, new RaiseEventOptions() { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
    }
}
