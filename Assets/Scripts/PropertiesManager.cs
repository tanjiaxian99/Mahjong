using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class PropertiesManager {

    /// <summary>
    /// The discarder's actor number, the tile discarded, and the discard position
    /// </summary>
    public static readonly string DiscardTilePropKey = "dt";

    public static void UpdateDiscardTile(Tuple<int, Tile, float> tuple) {
        Hashtable ht = new Hashtable();
        ht.Add(DiscardTilePropKey, tuple);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }
}
