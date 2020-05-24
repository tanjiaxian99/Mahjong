using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanCalculator {
    private int fanTotal;
    private WinCombos winCombos = new WinCombos();
    private Dictionary<Tile, PlayerManager.Wind> bonusTileToWindDict = new Dictionary<Tile, PlayerManager.Wind>();

    public FanCalculator() {
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.One), PlayerManager.Wind.EAST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Two), PlayerManager.Wind.NORTH);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Three), PlayerManager.Wind.WEST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Four), PlayerManager.Wind.SOUTH);

        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.One), PlayerManager.Wind.EAST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Two), PlayerManager.Wind.NORTH);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Three), PlayerManager.Wind.WEST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Four), PlayerManager.Wind.SOUTH);
    }


    public void CalculateFan(List<Tile> hand, List<Tile> bonusTiles, List<List<Tile>> comboTiles, Tile drawnTile, PlayerManager.Wind playerWind, PlayerManager.Wind tableWind) {
        fanTotal = 0;
        
        // Combining hand and comboTiles
        List<Tile> combinedHand = new List<Tile>(hand);
        for (int i = 0; i < comboTiles.Count; i++) {

            // GetRange is needed to drop the last tile for Kong combos
            foreach (Tile comboTile in comboTiles[i].GetRange(0, 3)) {
                combinedHand.Add(comboTile);
            }
        }


        // Retrieve list of solution(s)
        List<List<string>> listOfCombos = winCombos.CheckWin(combinedHand);
        if (listOfCombos.Count == 0) {
            return;
        }


        if (listOfCombos.Count == 1) {

        }


        if (listOfCombos.Count == 2) {

        }


        // Calculates number of Fan in bonus tiles
        this.FanInBonusTiles(bonusTiles, playerWind);
    }


    /// <summary>
    /// Calculates the number of Fan in bonus tiles
    /// </summary>
    private void FanInBonusTiles(List<Tile> bonusTiles, PlayerManager.Wind playerWind) {
        int numberOfSeasonTiles = 0;
        int numberOfFlowerTiles = 0;

        foreach (Tile bonusTile in bonusTiles) {
            if (bonusTileToWindDict.ContainsKey(bonusTile)) {
                if (bonusTileToWindDict[bonusTile] == playerWind) {
                    fanTotal += 1;
                }

                if (bonusTile.suit == Tile.Suit.Season) {
                    numberOfSeasonTiles += 1;
                } else {
                    numberOfFlowerTiles += 1;
                }

            } else {
                // Add 1 Fan for each Animal tile
                fanTotal += 1;
            }
        }

        // TODO: 2 complete flower groups
        if (numberOfSeasonTiles == 4) {
            fanTotal += 1;
        }

        if (numberOfFlowerTiles == 4) {
            fanTotal += 1;
        }
    }


    /// <summary>
    /// Calculates the number of Fan in the hand type (e.g. Half Flush, Sequence Hand, etc.). Excludes Fan from Honour Tiles (Dragon and Wind tiles).
    /// </summary>
    private void FanInHandCombos (List<string> comboList, List<Tile> hand) {
        HashSet<string> comboListNoDuplicate = new HashSet<string>(comboList);

        // Triplets Hand
        if (comboListNoDuplicate.Count == 2 && comboListNoDuplicate.Contains("Pong")) {
            fanTotal += 2;
        }


        // TODO: Hidden Treasure


        // Half Flush & Full Flush
        int fanToAdd = 0;
        Tile.Suit? predominantSuit = Tile.Suit.Character;
        for (int i = 0; i < hand.Count; i++) {
            if (i == 0 && (hand[i].suit == Tile.Suit.Character || hand[i].suit == Tile.Suit.Dot || hand[i].suit == Tile.Suit.Bamboo)) {
                predominantSuit = hand[i].suit;
                continue;
            }

            if (hand[i].suit != predominantSuit && (hand[i].suit == Tile.Suit.Character || hand[i].suit == Tile.Suit.Dot || hand[i].suit == Tile.Suit.Bamboo)) {
                break;
            }

            // Once an honour tile (Wind and Dragon tiles) has been encountered, every tile behind the current tile will be an honour tile
            if (hand[i].suit != predominantSuit && (hand[i].suit == Tile.Suit.Wind || hand[i].suit == Tile.Suit.Dragon)) {
                fanToAdd = 2;
                break;
            }

            // Anything but full flush would have broke out of the loop
            if (i == hand.Count - 1) {
                fanToAdd = 4;
            }
        }
        fanTotal += fanToAdd;
    }


    /// <summary>
    /// Calculates the number of Fan in Honour Tiles (Dragon and Wind tiles) combos
    /// </summary>
    private void FanInHonourTiles (List<Tile> hand) {

    }
}
