using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public static class EndRound {

    //[SerializeField]
    //private GameObject scriptManager;

    //private TilesManager tilesManager;

    public static void EndGame(Player winner, int fanTotal, List<string> winningCombos, TilesManager tilesManager) {
        ShowHandTiles(tilesManager);
    }

    /// <summary>
    /// Inform other players of the local player's hand tiles
    /// </summary>
    public static void ShowHandTiles(TilesManager tilesManager) {
        // TODO: Add winning tile to player's hand
        PropertiesManager.SetOpenHand(tilesManager.hand);
    }

    // TODO: Display winning combos / fan

    // TODO: Reset most variables

    // TODO: Recalculate playOrder & prevailing

    // TODO: Call InitializeRound to start the new round

    // TODO: If next round is East/East, end the game
}
