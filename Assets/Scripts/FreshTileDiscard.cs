using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshTileDiscard {

    public static bool IsFreshTile(List<Tile> discardTiles, List<Tile> allPlayersOpenTiles, Tile discardTile) {
        if (discardTiles.Contains(discardTile)) {
            return false;
        }

        for (int i = 0; i < allPlayersOpenTiles.Count; i++) {
            if (allPlayersOpenTiles[i].suit == Tile.Suit.Wind || allPlayersOpenTiles[i].suit == Tile.Suit.Dragon || 
                allPlayersOpenTiles[i].suit == Tile.Suit.Season || allPlayersOpenTiles[i].suit == Tile.Suit.Flower || 
                allPlayersOpenTiles[i].suit == Tile.Suit.Animal) {
                continue;
            }

            // Chow case
            if (allPlayersOpenTiles[i + 1].rank == allPlayersOpenTiles[i].rank + 1 && allPlayersOpenTiles[i + 2].rank == allPlayersOpenTiles[i].rank + 2) {
                i += 2;
                continue;
            }

            // Pong case
            if (allPlayersOpenTiles[i + 1] == allPlayersOpenTiles[i] && allPlayersOpenTiles[i + 2] == allPlayersOpenTiles[i]) {
                if (discardTile == allPlayersOpenTiles[i]) {
                    return false;
                }
            }

            // If the previous case was Kong, nothing happens.
        }

        return true;
    }
}
