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
    /// Dictionary containing actor numbers and wind assignments
    /// </summary>
    public static readonly string WindAllocationPropKey = "wa";

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
    /// The local player's open tiles
    /// </summary>
    public static readonly string OpenTilesPropKey = "ot";

    /// <summary>
    /// The player that has to pay for all players
    /// </summary>
    public static readonly string PayAllDiscardPropKey = "pa";

    #endregion

    #region Set Properties Methods

    public static void SetWindAllocation(Dictionary<int, int> windsAllocation) {
        Hashtable ht = new Hashtable();
        ht.Add(WindAllocationPropKey, windsAllocation);
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
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

    public static void SetPayAllDiscard(Player discardPlayer) {
        Hashtable ht = new Hashtable();
        ht.Add(PayAllDiscardPropKey, discardPlayer);
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

    public static int GetHandTilesCount(Player player) {
        return (int)player.CustomProperties[HandTilesCountPropKey];
    }

    public static List<Tile> GetOpenTiles(Player player) {
        return (List<Tile>)player.CustomProperties[OpenTilesPropKey];
    }

    #endregion

}
