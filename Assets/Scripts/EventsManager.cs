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
    const byte EvPongKongUpdate = 11;

    public static void EventPongKong(bool canPongKong) {
        PhotonNetwork.RaiseEvent(EvPongKongUpdate, canPongKong, new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }
}
