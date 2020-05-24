using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class FanCalculator {
    private int fanTotal;
    private WinCombos winCombos = new WinCombos();
    private Dictionary<Tile, PlayerManager.Wind> bonusTileToWindDict = new Dictionary<Tile, PlayerManager.Wind>();
    private Dictionary<Tile, int> nineGatesDict;

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


    public void CalculateFan(List<Tile> hand, List<Tile> bonusTiles, List<List<Tile>> comboTiles, Tile discardTile, PlayerManager.Wind playerWind, PlayerManager.Wind tableWind) {
        fanTotal = 0;

        // Combining hand and comboTiles
        List<Tile> combinedHand = new List<Tile>(hand);
        for (int i = 0; i < comboTiles.Count; i++) {

            // GetRange is needed to drop the last tile for Kong combos
            foreach (Tile comboTile in comboTiles[i].GetRange(0, 3)) {
                combinedHand.Add(comboTile);
            }
        }
        combinedHand = combinedHand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();


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

        // TODO: Winning on Replacement Tile
        // TODO: Robbing the Kong
        // TODO: Winning on the Last Available Tile

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
    private void FanInHand (List<string> comboList, List<Tile> combinedHand, List<Tile> originalHand, List<Tile> bonusTiles, Tile discardTile) {
        HashSet<string> comboListNoDuplicate = new HashSet<string>(comboList);
        List<string> winningCombos = new List<string>();

        // Triplets Hand check
        winningCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));


        // TODO: Hidden Treasure check


        // Half Flush & Full Flush check
        winningCombos.Add(this.FlushHandCheck(combinedHand));

        // Only check Nine Gates if there is a Full Flush
        if (winningCombos.Contains("Full Flush")) {
            winningCombos.Add(this.NineGatesCheck(combinedHand));
        }

        // Mixed and Pure Terminals check
        if (winningCombos.Contains("Triplets")) {
            winningCombos.Add(this.TerminalsHandCheck(combinedHand));
        }

        // All Honour check
        if (winningCombos.Contains("Triplets")) {
            winningCombos.Add(this.AllHonourCheck(combinedHand));
        }

        // Sequence Hand check. 
        string sequenceHandCheck = this.SequenceHandCheck(comboListNoDuplicate, combinedHand, originalHand, discardTile);
        if (sequenceHandCheck.Equals("Sequence")) {
            if (bonusTiles.Count > 0) {
                winningCombos.Add("Lesser Sequence");
            } else {
                winningCombos.Add("Sequence");
            }
        }

        // Pure Green Suit check
        winningCombos.Add(this.PureGreenSuitCheck(combinedHand));

        // Three Lesser Scholars and Three Great Scholars check
        winningCombos.Add(this.ThreeScholarsCheck(combinedHand));

    }

    #region Winning Hands

    /// <summary>
    /// Determine if the hand is a Triplets Hand
    /// </summary>
    private string TripletsHandCheck(HashSet<string> comboListNoDuplicate) {
        if (comboListNoDuplicate.Count == 2 && comboListNoDuplicate.Contains("Pong")) {
            return "Triplets";
        }

        return null;
    }


    /// <summary>
    /// Determine if the hand is either a Half Flush or Full Flush Hand
    /// </summary>
    private string FlushHandCheck(List<Tile> combinedHand) {
        Tile.Suit? predominantSuit = Tile.Suit.Character;
        for (int i = 0; i < combinedHand.Count; i++) {
            if (i == 0 && (combinedHand[i].suit == Tile.Suit.Character || combinedHand[i].suit == Tile.Suit.Dot || combinedHand[i].suit == Tile.Suit.Bamboo)) {
                predominantSuit = combinedHand[i].suit;
                continue;
            }

            if (combinedHand[i].suit != predominantSuit && (combinedHand[i].suit == Tile.Suit.Character || combinedHand[i].suit == Tile.Suit.Dot || combinedHand[i].suit == Tile.Suit.Bamboo)) {
                break;
            }

            // Once an honour tile (Wind and Dragon tiles) has been encountered, every tile behind the current tile will be an honour tile
            if (combinedHand[i].suit != predominantSuit && (combinedHand[i].suit == Tile.Suit.Wind || combinedHand[i].suit == Tile.Suit.Dragon)) {
                return "Half Flush";
            }

            // Anything but full flush would have broke out of the loop
            if (i == combinedHand.Count - 1) {
                return "Full Flush";
            }
        }

        return null;
    }


    /// <summary>
    /// Determine if the hand is a Nine Gates Hand
    /// </summary>
    private string NineGatesCheck(List<Tile> combinedHand) {
        nineGatesDict = new Dictionary<Tile, int>();
        this.InstantiateNineGatesDict();

        foreach (Tile tile in combinedHand) {
            nineGatesDict[tile]--;
        }

        int extraTileCount = 0;
        foreach (Tile tile in nineGatesDict.Keys) {
            if (tile.suit != combinedHand[0].suit) {
                break;
            }

            // A Nine Gates hand should only have one extra tile aside from 1112345678999
            if (nineGatesDict[tile] != 0) {
                extraTileCount++;
            }

            if (extraTileCount > 1) {
                return null;
            }
        }

        return "Nine Gates";
    }


    /// <summary>
    /// Determine if the hand is a Mixed Terminals or Pure Terminals Hand
    /// </summary>
    private string TerminalsHandCheck(List<Tile> combinedHand) {

        foreach (Tile tile in combinedHand) {
            if (tile.suit == Tile.Suit.Character || tile.suit == Tile.Suit.Dot || tile.suit == Tile.Suit.Bamboo) {
                if (!(tile.rank == Tile.Rank.One || tile.rank == Tile.Rank.Nine)) {
                    return null;
                }
            }

            if (tile.suit == Tile.Suit.Wind || tile.suit == Tile.Suit.Dragon) {
                return "Mixed Terminals";
            }
        }

        return "Pure Terminals";
    }


    /// <summary>
    /// Determine if the hand is an All Honour Hand
    /// </summary>
    private string AllHonourCheck(List<Tile> combinedHand) {
        if (combinedHand[0].suit == Tile.Suit.Wind || combinedHand[0].suit == Tile.Suit.Dragon) {
            return "All Honour";
        }

        return null;
    }


    /// <summary>
    /// Determine if the hand is a Sequence Hand
    /// </summary>
    private string SequenceHandCheck(HashSet<string> comboListNoDuplicate, List<Tile> combinedHand, List<Tile> originalHand, Tile discardTile) {
        // A Sequence Hand is not possible if player has 4 combos on the table.
        if (originalHand.Count == 2) {
            return null;
        }

        if (!comboListNoDuplicate.Contains("Chow")) {
            return null;
        }

        // discardTile is equal to null only when the tile is Self-Picked. In which case, the player can draw any tile to have a Sequence
        // Hand, as long as all the combos are Chow.
        if (discardTile == null) {
            return "Sequence";
        }

        if (!(discardTile.suit == Tile.Suit.Character || discardTile.suit == Tile.Suit.Dot || discardTile.suit == Tile.Suit.Bamboo)) {
            return null;
        }

        Tile tileMinusTwo = new Tile(discardTile.suit, discardTile.rank - 2);
        Tile tileMinusOne = new Tile(discardTile.suit, discardTile.rank - 1);
        Tile tilePlusOne = new Tile(discardTile.suit, discardTile.rank + 1);
        Tile tilePlusTwo = new Tile(discardTile.suit, discardTile.rank + 2);

        // Works for 6789 (6 or 9) and 45678 (3, 6 or 9)
        if (combinedHand.Contains(tilePlusOne) && combinedHand.Contains(tilePlusTwo)) {
            return "Sequence";
        }

        if (combinedHand.Contains(tileMinusTwo) && combinedHand.Contains(tileMinusOne)) {
            return "Sequence";
        }

        return null;
    }


    /// <summary>
    /// Determine if the hand is a Pure Green Suit Hand
    /// </summary>
    private string PureGreenSuitCheck(List<Tile> combinedHand) {
        foreach (Tile tile in combinedHand) {
            if (!(tile.suit == Tile.Suit.Bamboo || tile.suit != Tile.Suit.Dragon)) {
                return null;
            }

            if (tile.suit == Tile.Suit.Bamboo) {
                if (!(tile.rank == Tile.Rank.Two || tile.rank == Tile.Rank.Three || tile.rank == Tile.Rank.Four || tile.rank == Tile.Rank.Six || tile.rank == Tile.Rank.Eight)) {
                    return null;
                }
            }

            if (tile.suit == Tile.Suit.Dragon && tile.rank != Tile.Rank.Three) {
                return null;
            }
        }

        return "Pure Green Suit";
    }


    /// <summary>
    /// Determine if the hand is a Three Lesser Scholars or Three Great Scholars Hand
    /// </summary>
    private string ThreeScholarsCheck(List<Tile> combinedHand) {
        int scholarsCount = 0;

        foreach (Tile tile in combinedHand) {
            if (tile.suit == Tile.Suit.Dragon) {
                scholarsCount += 1;
            }
        }

        if (scholarsCount == 8) {
            return "Three Lesser Scholars";
        }

        if (scholarsCount == 9) {
            return "Three Great Scholars";
        }

        return null;
    }



    #endregion

    /// <summary>
    /// Instantiate the NineGatesDict only if the hand is checked for Nine Gates
    /// </summary>
    private void InstantiateNineGatesDict() {
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.One), 3);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine), 3);

        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One), 3);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Two), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Three), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Four), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Five), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Six), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Seven), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Eight), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Nine), 3);

        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One), 3);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Two), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Three), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Four), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Five), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Six), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Seven), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight), 1);
        nineGatesDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine), 3);
    }

    /// <summary>
    /// Calculates the number of Fan in Honour Tiles (Dragon and Wind tiles) combos
    /// </summary>
    private void FanInHonourTiles (List<Tile> hand) {

    }
}
