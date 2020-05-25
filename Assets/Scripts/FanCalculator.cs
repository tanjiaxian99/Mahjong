using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor.WindowsStandalone;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Calculates the number of Fan the player has
/// </summary>
public class FanCalculator {
    private int fanTotal;
    private int fanLimit = 5;
    private WinCombos winCombos = new WinCombos();

    private Dictionary<Tile, PlayerManager.Wind> bonusTileToWindDict = new Dictionary<Tile, PlayerManager.Wind>();
    private Dictionary<Tile, int> nineGatesDict;
    private Dictionary<string, int> handsToCheck;
    private Dictionary<Tile, int> honourTilesCount;
    private Dictionary<PlayerManager.Wind, Tile> windToTileDict = new Dictionary<PlayerManager.Wind, Tile>();

    private List<string> winningCombos;
    private List<List<string>> listOfWinningCombos;

    /// <param name="handsToCheck">A dictionary containing each hand and the number of Fan it should have</param>
    public FanCalculator(Dictionary<string, int> handsToCheck) {
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.One), PlayerManager.Wind.EAST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Two), PlayerManager.Wind.NORTH);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Three), PlayerManager.Wind.WEST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Four), PlayerManager.Wind.SOUTH);

        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.One), PlayerManager.Wind.EAST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Two), PlayerManager.Wind.NORTH);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Three), PlayerManager.Wind.WEST);
        bonusTileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Four), PlayerManager.Wind.SOUTH);

        windToTileDict.Add(PlayerManager.Wind.EAST, new Tile(Tile.Suit.Wind, Tile.Rank.One));
        windToTileDict.Add(PlayerManager.Wind.SOUTH, new Tile(Tile.Suit.Wind, Tile.Rank.Two));
        windToTileDict.Add(PlayerManager.Wind.WEST, new Tile(Tile.Suit.Wind, Tile.Rank.Three));
        windToTileDict.Add(PlayerManager.Wind.NORTH, new Tile(Tile.Suit.Wind, Tile.Rank.Four));

        this.handsToCheck = handsToCheck;
    }


    /// <summary>
    /// Calculates and returns the number of Fan the player's tiles contain
    /// </summary>
    public int CalculateFan(PlayerManager playerManager, Tile discardTile, PlayerManager.Wind discardPlayerWind, PlayerManager.Wind prevailingWind, int numberOfTilesLeft, int turn, List<Tile> allPlayersOpenTiles) {
        fanTotal = 0;

        this.TabulateCombos(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);


        return 0;
    }


    /// <summary>
    /// Tabulates all the combos the player has
    /// </summary>
    /// <param name="discardTile">The latest discard tile. Null if the tile is self-picked</param>
    private void TabulateCombos(PlayerManager playerManager, Tile discardTile, PlayerManager.Wind discardPlayerWind, PlayerManager.Wind prevailingWind, int numberOfTilesLeft, int turn, List<Tile> allPlayersOpenTiles) {
        List<Tile> hand = playerManager.hand;
        List<Tile> bonusTiles = playerManager.bonusTiles;
        List<List<Tile>> comboTiles = playerManager.comboTiles;
        PlayerManager.Wind playerWind = playerManager.playerWind;
        int numberOfReplacementTiles = playerManager.numberOfReplacementTiles;
        int numberOfKong = playerManager.numberOfKong;

        

        // Combining hand and comboTiles
        List<Tile> combinedHand = new List<Tile>(hand);
        for (int i = 0; i < comboTiles.Count; i++) {

            // GetRange is needed to drop the last tile for Kong combos
            foreach (Tile comboTile in comboTiles[i].GetRange(0, 3)) {
                combinedHand.Add(comboTile);
            }
        }

        if (combinedHand.Count == 13 && discardTile != null) {
            combinedHand.Add(discardTile);
        }
        combinedHand = combinedHand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();


        // Retrieve list of solution(s)
        List<List<string>> listOfCombos = winCombos.CheckWin(combinedHand);
        if (listOfCombos == null) {
            return;
        }

        if (listOfCombos.Count == 0) {
            this.ThirteenWondersCheck(combinedHand);
            return;
        }

        for (int i = 0; i < listOfCombos.Count; i++) {
            winningCombos = new List<string>();

            this.FirstRoundHands(allPlayersOpenTiles, playerWind, discardPlayerWind, discardTile, turn);
            this.FanInBonusTiles(bonusTiles, playerWind, allPlayersOpenTiles);
            this.FanInHonourTiles(combinedHand, playerWind, prevailingWind);
            this.FanInHand(listOfCombos[i], combinedHand, hand, bonusTiles, comboTiles, playerWind, prevailingWind, discardTile);
            this.WinningOnReplacementTile(numberOfReplacementTiles, numberOfKong);
            this.RobbingTheKong(discardTile);
            this.WinningOnTheLastAvailableTile(numberOfReplacementTiles, numberOfTilesLeft);

            this.listOfWinningCombos.Add(winningCombos);
        }
    }

    #region Fan Checks

    /// <summary>
    /// Container for Heavenly, Earthly and Humanly Hands
    /// </summary>
    private void FirstRoundHands(List<Tile> allPlayersOpenTiles, PlayerManager.Wind playerWind, PlayerManager.Wind discardPlayerWind, Tile discardTile, int turn) {
        if (handsToCheck["Heavenly Hand"] > 0) {
            winningCombos.Add(HeavenlyHandCheck(playerWind, turn));
        }

        if (handsToCheck["Earthly Hand"] > 0) {
            winningCombos.Add(EarthlyHandCheck(discardPlayerWind, discardTile, turn));
        }

        if (handsToCheck["Humanly Hand"] > 0) {
            winningCombos.Add(HumanlyHandCheck(allPlayersOpenTiles, playerWind, discardPlayerWind, discardTile, turn));
        }

    }


    /// <summary>
    /// Calculates the number of Fan in bonus tiles
    /// </summary>
    private void FanInBonusTiles(List<Tile> bonusTiles, PlayerManager.Wind playerWind, List<Tile> allPlayersOpenTiles) {
        int numberOfSeasonTiles = 0;
        int numberOfFlowerTiles = 0;

        foreach (Tile bonusTile in bonusTiles) {
            if (bonusTileToWindDict.ContainsKey(bonusTile)) {
                if (bonusTileToWindDict[bonusTile] == playerWind) {
                    winningCombos.Add("Bonus Tile Match Seat Wind");
                }

                if (bonusTile.suit == Tile.Suit.Season) {
                    numberOfSeasonTiles += 1;

                } else {
                    numberOfFlowerTiles += 1;
                }

            } else {
                winningCombos.Add("Animal");
            }
        }

        // Complete Season Group
        if (numberOfSeasonTiles == 4) {
            winningCombos.Add("Complete Season Group");
        }

        // Complete Flower Group
        if (numberOfFlowerTiles == 4) {
            winningCombos.Add("Complete Flower Group");
        }

        // Robbing the Eighth
        if (numberOfSeasonTiles + numberOfFlowerTiles == 7) {
            foreach (Tile tile in allPlayersOpenTiles) {
                if (tile.suit == Tile.Suit.Season || tile.suit == Tile.Suit.Flower) {
                    winningCombos.Add("Robbing the Eighth");
                }
            }
        }

        // All Flowers and Seasons
        if (numberOfSeasonTiles + numberOfFlowerTiles == 8) {
            winningCombos.Add("All Flowers and Seasons");
        }
        
    }


    /// <summary>
    /// Calculate the number of Fan in the player's hand due to honour tiles
    /// </summary>
    private void FanInHonourTiles(List<Tile> combinedhand, PlayerManager.Wind playerWind, PlayerManager.Wind prevailingWind) {
        this.InstantiateHonourTilesCount();

        foreach (Tile tile in combinedhand) {
            if (honourTilesCount.ContainsKey(tile)) {
                honourTilesCount[tile]++;
            }
        }

        foreach (Tile tile in honourTilesCount.Keys) {
            if (honourTilesCount[tile] == 3) {

                if (tile.suit == Tile.Suit.Wind && bonusTileToWindDict[tile] == playerWind) {
                    winningCombos.Add("Player Wind Set");
                }

                if (tile.suit == Tile.Suit.Wind && bonusTileToWindDict[tile] == prevailingWind) {
                    winningCombos.Add("Prevailing Wind Set");
                }

                if (tile.suit == Tile.Suit.Dragon) {
                    winningCombos.Add(tile.ToString());
                }
            }
        }
    }


    /// <summary>
    /// Calculates the number of Fan in the hand type (e.g. Half Flush, Sequence Hand, etc.). Excludes Fan from Honour Tiles (Dragon and Wind tiles).
    /// </summary>
    private void FanInHand(List<string> listOfCombos, List<Tile> combinedHand, List<Tile> originalHand, List<Tile> bonusTiles, List<List<Tile>> comboTiles, PlayerManager.Wind playerWind, PlayerManager.Wind prevailingWind, Tile discardTile) {
        HashSet<string> comboListNoDuplicate = new HashSet<string>(listOfCombos);

        // Fully Concealed Hand check
        if (handsToCheck["Fully Concealed"] > 0) {
            winningCombos.Add(this.FullyConcealedHandCheck(comboTiles, discardTile));
        }
        
        // Triplets Hand check
        if (handsToCheck["Triplets"] > 0) {
            winningCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
        }

        // Half Flush & Full Flush check
        if (handsToCheck["Flush"] > 0) {
            winningCombos.Add(this.FlushHandCheck(combinedHand));
        }

        // Sequence Hand check. 
        if (handsToCheck["Sequence Hand"] > 0) {
            string sequenceHandCheck = this.SequenceHandCheck(comboListNoDuplicate, combinedHand, originalHand, playerWind, prevailingWind, discardTile);
            if (sequenceHandCheck.Equals("Sequence")) {
                if (bonusTiles.Count > 0) {
                    winningCombos.Add("Lesser Sequence");
                } else {
                    winningCombos.Add("Sequence");
                }
            }
        }

        // Mixed and Pure Terminals check. Prerequisite: Triplets
        if (handsToCheck["Terminals"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (handsToCheck["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (winCombos.Contains("Triplets")) {
                winningCombos.Add(this.TerminalsHandCheck(combinedHand));
            }
        }

        // All Honour check. Prerequisite: Triplets
        if (handsToCheck["All Honour"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (handsToCheck["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (winCombos.Contains("Triplets")) {
                winningCombos.Add(this.AllHonourCheck(combinedHand));
            }
        }

        // Hidden Treasure check. Prerequisite: Triplets, Fully Concealed Hand
        if (handsToCheck["Hidden Treasure"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (handsToCheck["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (handsToCheck["Fully Concealed Hand"] == 0) {
                winCombos.Add(this.FullyConcealedHandCheck(comboTiles, discardTile));
            }

            if (winCombos.Contains("Triplets") && winCombos.Contains("Fully Concealed Hand")) {
                winningCombos.Add("Hidden Treasure");
            }
        }

        // Full Flush Triplets check. Prerequisite: Full Flush, Triplets
        if (handsToCheck["Full Flush Triplets"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (handsToCheck["Full Flush"] == 0) {
                winCombos.Add(this.FlushHandCheck(combinedHand));
            }

            if (handsToCheck["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (winCombos.Contains("Full Flush") && winCombos.Contains("Triplets")) {
                winningCombos.Add("Full Flush Triplets");
            }
        }

        // Full Flush Sequence Hand check. Prerequisite: Full Flush, Sequence Hand
        if (handsToCheck["Full Flush Sequence Hand"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (handsToCheck["Full Flush"] == 0) {
                winCombos.Add(this.FlushHandCheck(combinedHand));
            }

            if (handsToCheck["Sequence Hand"] == 0) {
                winCombos.Add(this.SequenceHandCheck(comboListNoDuplicate, combinedHand, originalHand, playerWind, prevailingWind, discardTile));
            }

            if (winCombos.Contains("Full Flush") && winCombos.Contains("Sequence Hand")) {
                winningCombos.Add("Full Flush Sequence Hand");
            }
        }

        // Nine Gates Check. Prerequisite: Full Flush
        if (handsToCheck["Nine Gates"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (handsToCheck["Full Flush"] == 0) {
                winCombos.Add(this.FlushHandCheck(combinedHand));
            }

            if (winCombos.Contains("Full Flush")) {
                winningCombos.Add(this.NineGatesCheck(combinedHand));
            }
        }

        // Four Lesser Blessings and Four Great Blessings check. Prerequisite: All Honour
        if (handsToCheck["Four Blessings"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (handsToCheck["All Honour"] == 0) {
                winCombos.Add(this.AllHonourCheck(combinedHand));
            }

            if (winCombos.Contains("Triplets")) {
                winningCombos.Add(this.FourBlessingsCheck(combinedHand));
            }
        }

        // Pure Green Suit check
        if (handsToCheck["Pure Green Suit"] > 0) {
            winningCombos.Add(this.PureGreenSuitCheck(combinedHand));
        }

        // Three Lesser Scholars and Three Great Scholars check. 
        if (handsToCheck["Three Scholars"] > 0) {
            winningCombos.Add(this.ThreeScholarsCheck(combinedHand));
        }

        // Eighteen Arhats check
        if (handsToCheck["Eighteen Arhats"] > 0) {
            winningCombos.Add(this.EighteenArhatsCheck(comboTiles));
        }
    }


    /// <summary>
    /// Container for Replacement Tile Win checks
    /// </summary>
    private void WinningOnReplacementTile(int numberOfReplacementTiles, int numberOfKong) {
        if (handsToCheck["Winning on Replacement Tile For Flower"] > 0) {
            winningCombos.Add(WinningOnReplacementTileForFlower(numberOfReplacementTiles));
        }

        if (handsToCheck["Winning on Replacement Tile for Kong"] > 0) {
            winningCombos.Add(WinningOnReplacementTileForKong(numberOfKong));
        }

        if (handsToCheck["Kong on Kong"] > 0) {
            winningCombos.Add(KongOnKong(numberOfKong));
        }
    }


    /// <summary>
    /// Determine if the player robbed the Kong
    /// </summary>
    private void RobbingTheKong(Tile discardTile) {
        if (discardTile.kongType > 0) {
            winningCombos.Add("Robbing the Kong");
        }
    }


    /// <summary>
    /// Determine if the player won on the last available tile
    /// </summary>
    private string WinningOnTheLastAvailableTile(int numberOfReplacementTiles, int numberOfTilesLeft) {
        if (numberOfReplacementTiles == 0 && numberOfTilesLeft == 15) {
            return "Winning on Last Available Tile";
        }

        return null;
    }

    #endregion

    #region Winning Hands

    /// <summary>
    /// Determine if the hand is a Fully Concealed Hand
    /// </summary>
    private string FullyConcealedHandCheck(List<List<Tile>> comboTiles, Tile discardTile) {
        int comboTilesCount = 0;
        foreach (List<Tile> combo in comboTiles) {
            comboTilesCount += combo.Count;
        }

        if (comboTilesCount == 0 && discardTile == null) {
            return "Fully Concealed";
        }
        return null;
    }


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
    /// Determine if the hand is a Sequence Hand
    /// </summary>
    private string SequenceHandCheck(HashSet<string> comboListNoDuplicate, List<Tile> combinedHand, List<Tile> originalHand, PlayerManager.Wind playerWind, PlayerManager.Wind prevailingWind, Tile discardTile) {
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

        List<Tile> forbiddenTiles = new List<Tile>() {
            windToTileDict[playerWind],
            windToTileDict[prevailingWind],
            new Tile(Tile.Suit.Dragon, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three)
        };

        foreach (Tile tile in forbiddenTiles) {
            if (combinedHand.Contains(tile)) {
                return null;
            }
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
    /// Determine if the hand is a Four Lesser Blessings or Four Great Blessings Hand
    /// </summary>
    private string FourBlessingsCheck(List<Tile> combinedHand) {
        int windTilesCount = 0;
        foreach (Tile tile in combinedHand) {
            if (tile.suit == Tile.Suit.Wind) {
                windTilesCount++;
            }
        }

        if (windTilesCount == 11) {
            return "Four Lesser Blessings";
        }

        if (windTilesCount == 12) {
            return "Four Great Blessings";
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


    /// <summary>
    /// Determine if the hand is a Thirteen Wonders Hand
    /// </summary>
    private string ThirteenWondersCheck(List<Tile> combinedHand) {
        List<Tile> thirteenWondersTiles = new List<Tile>() { 
            new Tile(Tile.Suit.Character, Tile.Rank.One),
            new Tile(Tile.Suit.Character, Tile.Rank.Nine),
            new Tile(Tile.Suit.Dot, Tile.Rank.One),
            new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.Two),
            new Tile(Tile.Suit.Wind, Tile.Rank.Three),
            new Tile(Tile.Suit.Wind, Tile.Rank.Four),
            new Tile(Tile.Suit.Dragon, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
        };

        HashSet<Tile> noDuplicateHand = new HashSet<Tile>(combinedHand);
        foreach (Tile tile in noDuplicateHand) {
            if (!thirteenWondersTiles.Contains(tile)) {
                return null;
            }
        }

        return "Thirteen Wonders";
    }


    /// <summary>
    /// Determine if the hand is an Eighteen Arhats Hand
    /// </summary>
    private string EighteenArhatsCheck(List<List<Tile>> comboTiles) {
        int comboTilesCount = 0;

        foreach (List<Tile> combo in comboTiles) {
            comboTilesCount += combo.Count;
        }

        if (comboTilesCount == 18) {
            return "Eighteen Arhats";
        }

        return null;
    }

    #endregion

    #region Winning on Replacement Tile

    /// <summary>
    /// Determine if the player won with a replacement tile from flower
    /// </summary>
    private string WinningOnReplacementTileForFlower(int numberOfReplacementTiles) {
        if (numberOfReplacementTiles > 0) {
            return string.Format("Winning on Replacement Tile for Flower {0}", numberOfReplacementTiles);
        }
        return null;
    }


    /// <summary>
    /// Determine if the player won with a replacement tile from Kong
    /// </summary>
    private string WinningOnReplacementTileForKong(int numberOfKong) {
        if (numberOfKong == 1) {
            return "Winning on Replacement Tile for Kong";
        }
        return null;
    }


    /// <summary>
    /// Determine if the player won with a Kong on Kong
    /// </summary>
    private string KongOnKong(int numberOfKong) {
        if (numberOfKong == 2) {
            return "Kong on Kong";
        }
        return null;
    }

    #endregion

    #region FirstRoundHands

    /// <summary>
    /// Determine if the hand is a Heavenly Hand
    /// </summary>
    /// <returns></returns>
    private string HeavenlyHandCheck(PlayerManager.Wind playerWind, int turn) {
        if (playerWind == PlayerManager.Wind.EAST && turn == 1) {
            return "Heavenly Hand";
        }
        return null;
    }


    /// <summary>
    /// Determine if the hand is a Earthly Hand
    /// </summary>
    private string EarthlyHandCheck(PlayerManager.Wind discardPlayerWind, Tile discardTile, int turn) {
        if (turn == 1 && discardPlayerWind == PlayerManager.Wind.EAST && discardTile != null) {
            return "Earthly Hand";
        }

        //// TODO: "Non-dealer wins upon drawing the first card in the game" So Chow/Pong/Kong are allowed before this happens?
        //if (turn == 1 && playerWind != PlayerManager.Wind.EAST && discardTile == null) {
        //    return "Earthly Hand";
        //}
        
        return null;
    }


    /// <summary>
    /// Determine if the hand is a Humanly Hand
    /// </summary>
    private string HumanlyHandCheck(List<Tile> allPlayersOpenTiles, PlayerManager.Wind playerWind, PlayerManager.Wind discardPlayerWind, Tile discardTile, int turn) {
        if (turn == 1 && playerWind != PlayerManager.Wind.EAST && discardTile != null) {
            int numberOfComboTiles = 0;
            int numberOfConcealedKong = 0;
            

            foreach (Tile tile in allPlayersOpenTiles) {
                if (tile.suit == Tile.Suit.Season || tile.suit == Tile.Suit.Flower || tile.suit == Tile.Suit.Animal) {
                    continue;
                }
                numberOfComboTiles++;

                if (tile.kongType == 2) {
                    numberOfConcealedKong++;
                }

            }

            if (numberOfConcealedKong * 4 != numberOfComboTiles) {
                return null;
            }

            return "Humanly Hand";
        }
        return null;
    }

    #endregion

    #region Dictionary Instantiation

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
    /// Instantiate the Honour Tiles Count Dict
    /// </summary>
    private void InstantiateHonourTilesCount() {
        honourTilesCount = new Dictionary<Tile, int>();

        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four), 0);

        honourTilesCount.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.One), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Two), 0);
        honourTilesCount.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Three), 0);
    }

    #endregion
}
