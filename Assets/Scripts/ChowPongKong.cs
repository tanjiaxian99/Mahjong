using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class ChowPongKong {
    [SerializeField]
    private static GameObject playerPrefab;

    private static PlayerManager playerManager;

    private static List<Tile> hand;

    private static List<List<Tile>> comboTiles;

    static ChowPongKong() {
        playerManager = playerPrefab.GetComponent<PlayerManager>();
        hand = playerManager.hand;
        comboTiles = playerManager.comboTiles;
    }


    /// <summary>
    /// Returns a list of chow combinations for the tile, contingent on the player's hand. Last item in the object array specifies
    /// which tile is the drawn/discard tile.
    /// </summary>
    public static List<object[]> ChowCombinations(Tile discardTile) {
        List<object[]> combinations = new List<object[]>();
        object[] combo;

        // Only Character, Dot and Bamboo suits can chow
        if (discardTile.suit != Tile.Suit.Character && discardTile.suit != Tile.Suit.Dot && discardTile.suit != Tile.Suit.Bamboo) {
            return combinations;
        }

        // Chow can only happen between tiles of the same suit
        List<Tile> sameSuit = new List<Tile>();
        foreach (Tile tile in hand) {
            if (tile.suit == discardTile.suit) {
                sameSuit.Add(tile);
            }
        }

        // Can't chow if there is only one tile from the same suit
        if (sameSuit.Count <= 1) {
            return combinations;
        }

        Tile tileMinusTwo = new Tile(discardTile.suit, discardTile.rank - 2);
        Tile tileMinusOne = new Tile(discardTile.suit, discardTile.rank - 1);
        Tile tilePlusOne = new Tile(discardTile.suit, discardTile.rank + 1);
        Tile tilePlusTwo = new Tile(discardTile.suit, discardTile.rank + 2);

        // The tile forms a sequence as the first tile
        if (hand.Contains(tilePlusOne) && hand.Contains(tilePlusTwo)) {
            combo = new object[] { discardTile, tilePlusOne, tilePlusTwo, "First" };
            combinations.Add(combo);
        }

        // The tile forms a sequence as the middle tile
        if (hand.Contains(tileMinusOne) && hand.Contains(tilePlusOne)) {
            combo = new object[] { tileMinusOne, discardTile, tilePlusOne, "Second" };
            combinations.Add(combo);
        }

        // The tile forms a sequence as the last tile
        if (hand.Contains(tileMinusTwo) && hand.Contains(tileMinusOne)) {
            combo = new object[] { tileMinusTwo, tileMinusOne, discardTile, "Third" };
            combinations.Add(combo);
        }

        return combinations;
    }


    /// <summary>
    /// Helper function. Returns true if the number of the object tile matches numberOfTiles. Used for Pong and Kong.
    /// </summary>
    private static bool SameNumberOfTiles(Tile discardTile, List<Tile> tiles, int numberOfTiles) {
        int equalCount = 0;

        foreach (Tile tile in tiles) {
            if (discardTile.Equals(tile)) {
                equalCount++;
            }
        }

        return equalCount == numberOfTiles;
    }


    /// <summary>
    /// Returns true if the player can Pong
    /// </summary>
    public static bool CanPong(Tile discardTile) {
        return SameNumberOfTiles(discardTile, hand, 2);
    }


    /// <summary>
    /// Returns true if the player can execute a Discard Kong
    /// </summary>
    public static bool CanDiscardKong(Tile discardTile) {
        return SameNumberOfTiles(discardTile, hand, 3);
    }


    /// <summary>
    /// Returns the tiles which the player could execute a Exposed Kong with
    /// </summary>
    public static List<Tile> ExposedKongTiles() {
        List<Tile> exposedKongTiles = new List<Tile>();

        foreach (List<Tile> combo in comboTiles) {
            if (ComboType(combo).Equals("Pong")) {

                if (hand.Contains(combo[0])) {
                    exposedKongTiles.Add(combo[0]);
                }
            }
        }

        return exposedKongTiles;
    }


    /// <summary>
    /// Returns the tiles which the player could execute a Concealed Kong with
    /// </summary>
    public static List<Tile> ConcealedKongTiles() {
        HashSet<Tile> concealedKongTiles = new HashSet<Tile>();

        foreach (Tile handTile in hand) {
            if (SameNumberOfTiles(handTile, hand, 4)) {
                concealedKongTiles.Add(handTile);
            }
        }

        return new List<Tile>(concealedKongTiles);
    }


    /// <summary>
    /// Returns the combo types of Chow, Pong or Kong
    /// </summary>
    public static string ComboType(List<Tile> tiles) {
        if (tiles.Count == 4) {
            return "Kong";
        }

        if (new HashSet<Tile>(tiles).Count == 1) {
            return "Pong";
        }

        return "Chow";
    }
}
