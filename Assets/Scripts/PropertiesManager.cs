using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class PropertiesManager {

    #region PropKeys

    /// <summary>
    /// The discarder's actor number, the tile discarded, and the discard position
    /// </summary>
    public static readonly string DiscardTilePropKey = "dt";

    /// <summary>
    /// The player that has to pay for all players
    /// </summary>
    public static readonly string PayAllDiscardPropKey = "pa";

    #endregion

    #region Update Properties Methods

    public static void UpdateDiscardTile(Tuple<int, Tile, float> tuple) {
        Hashtable ht = new Hashtable();
        ht.Add(DiscardTilePropKey, tuple);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    public static void UpdatePayAllDiscard(Player discardPlayer) {
        Hashtable ht = new Hashtable();
        ht.Add(PayAllDiscardPropKey, discardPlayer);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    #endregion
}
