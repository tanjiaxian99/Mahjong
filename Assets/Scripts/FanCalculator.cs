﻿using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Calculates the number of Fan the player has
/// </summary>
public class FanCalculator : MonoBehaviour {
    [SerializeField]
    private GameObject scriptManager;

    private Dictionary<string, int> settingsDict;
    private int fanTotal;
    private int fanLimit;
    private WinCombos winCombos;
  
    private List<string> winningCombos;
    private List<List<string>> listOfWinningCombos;
    private List<int> fanTotalList;

    private void Start() {
        Settings settings = scriptManager.GetComponent<Settings>();
        settingsDict = settings.settingsDict;
        winCombos = new WinCombos();
    }

    
    /// <summary>
    /// For Unit Testing
    /// </summary>
    public FanCalculator(Dictionary<string, int> settingsDict) {
        this.settingsDict = settingsDict;
        winCombos = new WinCombos();
    }


    /// <summary>
    /// Calculates and returns the number of Fan the player's tiles contain
    /// </summary>
    public (int, List<string>) CalculateFan(PlayerManager playerManager, TilesManager tileManager, Tile discardTile, PlayerManager.Wind? discardPlayerWind, PlayerManager.Wind prevailingWind, int numberOfTilesLeft, int turn, List<Tile> allPlayersOpenTiles) {
        fanLimit = settingsDict["Fan Limit"];
        fanTotalList = new List<int>();
        listOfWinningCombos = new List<List<string>>();

        this.TabulateCombos(playerManager, tileManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
        
        for (int i = 0; i < listOfWinningCombos.Count; i++) {
            listOfWinningCombos[i].RemoveAll(x => x == null);
        }

        foreach (List<string> winningCombos in listOfWinningCombos) {
            fanTotal = 0;

            #region Fan in First Round

            if (winningCombos.Contains("Heavenly Hand")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Earthly Hand")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Humanly Hand")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            #endregion

            #region Fan in Bonus Tiles

            if (winningCombos.Contains("Bonus Tile Match Seat Wind")) {
                foreach (string winCombo in winningCombos) {
                    if (winCombo == "Bonus Tile Match Seat Wind") {
                        fanTotal += settingsDict["Bonus Tile Match Seat Wind"];
                    }
                }

            }

            if (winningCombos.Contains("Animal")) {
                foreach (string winCombo in winningCombos) {
                    if (winCombo == "Animal") {
                        fanTotal += settingsDict["Animal"];
                    }
                }
            }

            if (winningCombos.Contains("Complete Animal Group")) {
                fanTotal += settingsDict["Complete Animal Group"];
            }

            if (winningCombos.Contains("Complete Season Group")) {
                fanTotal += settingsDict["Complete Season Group"];
            }

            if (winningCombos.Contains("Complete Flower Group")) {
                fanTotal += settingsDict["Complete Flower Group"];
            }

            if (winningCombos.Contains("Robbing the Eighth")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("All Flowers and Seasons")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            #endregion

            #region Fan in Honour Tiles

            if (winningCombos.Contains("Seat Wind Combo")) {
                fanTotal += settingsDict["Seat Wind Combo"];
            }

            if (winningCombos.Contains("Prevailing Wind Combo")) {
                fanTotal += settingsDict["Prevailing Wind Combo"];
            }

            foreach (string winCombo in winningCombos) {
                if (winCombo.Contains("Dragon")) {
                    fanTotal += settingsDict["Dragon"];
                }
            }

            #endregion

            #region Fan in Hand

            if (winningCombos.Contains("Fully Concealed")) {
                fanTotal += settingsDict["Fully Concealed"];
            }

            if (winningCombos.Contains("Triplets")) {
                fanTotal += settingsDict["Triplets"];
            }

            if (winningCombos.Contains("Half Flush")) {
                fanTotal += settingsDict["Half Flush"];
            }

            if (winningCombos.Contains("Full Flush")) {
                fanTotal += settingsDict["Full Flush"];
            }

            if (winningCombos.Contains("Lesser Sequence")) {
                fanTotal += settingsDict["Lesser Sequence"];
            }

            if (winningCombos.Contains("Full Sequence")) {
                fanTotal += settingsDict["Full Sequence"];
            }

            if (winningCombos.Contains("Mixed Terminals")) {
                fanTotal += settingsDict["Mixed Terminals"];
            }

            if (winningCombos.Contains("Pure Terminals")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("All Honour")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Hidden Treasure")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Full Flush Triplets")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Full Flush Full Sequence")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Full Flush Lesser Sequence")) {
                fanTotal += settingsDict["Full Flush Lesser Sequence"];
            }

            if (winningCombos.Contains("Nine Gates")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Four Lesser Blessings")) {
                fanTotal += settingsDict["Four Lesser Blessings"];
            }

            if (winningCombos.Contains("Four Great Blessings")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Pure Green Suit")) {
                fanTotal += settingsDict["Pure Green Suit"];
            }

            if (winningCombos.Contains("Three Lesser Scholars")) {
                fanTotal += settingsDict["Three Lesser Scholars"];
            }

            if (winningCombos.Contains("Three Great Scholars")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Eighteen Arhats")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            if (winningCombos.Contains("Thirteen Wonders")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            #endregion

            #region Fan in Replacement Tile

            if (winningCombos.Contains("Replacement Tile for Flower")) {
                foreach (string winCombo in winningCombos) {
                    if (winCombo == "Replacement Tile for Flower") {
                        fanTotal += settingsDict["Replacement Tile for Flower"];
                    }
                }
            }

            if (winningCombos.Contains("Replacement Tile for Kong")) {
                fanTotal += settingsDict["Replacement Tile for Kong"];
            }

            if (winningCombos.Contains("Kong on Kong")) {
                fanTotal = fanLimit;
                fanTotalList.Add(fanTotal);
                continue;
            }

            #endregion

            if (winningCombos.Contains("Robbing the Kong")) {
                fanTotal += settingsDict["Robbing the Kong"];
            }

            if (winningCombos.Contains("Winning on Last Available Tile")) {
                fanTotal += settingsDict["Winning on Last Available Tile"];
            }

            if (fanTotal > fanLimit) {
                fanTotal = fanLimit;
            }

            fanTotalList.Add(fanTotal);
        }

        if (listOfWinningCombos.Count == 0) {
            return (0, null);
        }

        if (listOfWinningCombos.Count == 1) {
            return (fanTotalList[0], listOfWinningCombos[0]);
        }

        if (fanTotalList[0] > fanTotalList[1]) {
            return (fanTotalList[0], listOfWinningCombos[0]);
        } else {
            return (fanTotalList[1], listOfWinningCombos[1]);
        }

    }


    /// <summary>
    /// Tabulates all the combos the player has
    /// </summary>
    /// <param name="discardTile">The latest discard tile. Null if the tile is self-picked</param>
    private void TabulateCombos(PlayerManager playerManager, TilesManager tileManager, Tile discardTile, PlayerManager.Wind? discardPlayerWind, PlayerManager.Wind prevailingWind, int numberOfTilesLeft, int turn, List<Tile> allPlayersOpenTiles) {
        List<Tile> hand = tileManager.hand;
        List<Tile> bonusTiles = tileManager.bonusTiles;
        List<List<Tile>> comboTiles = tileManager.comboTiles;
        PlayerManager.Wind playerWind = playerManager.seatWind;
        int numberOfReplacementTiles = playerManager.numberOfReplacementTiles;
        int numberOfKong = playerManager.numberOfKong;
        bool hasDrawnTile = playerManager.hasDrawnTile;

        List<Tile> combinedHand = new List<Tile>(hand);

        if ((combinedHand.Count + 1) % 3 != 0 && discardTile != null) {
            combinedHand.Add(discardTile);
        }
        combinedHand = combinedHand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();

        // Retrieve list of solution(s), excluding that of combo tiles
        List<List<string>> listOfCombos = winCombos.CheckWin(combinedHand);

        // Add combos from combo tile to listOfCombos
        List<string> combosInComboTiles = new List<string>();
        foreach (List<Tile> tiles in comboTiles) {
            combosInComboTiles.Add(TilesManager.ComboType(tiles));
        }

        foreach (List<string> combos in listOfCombos) {
            combos.AddRange(combosInComboTiles);
        }

        // Combining hand and comboTiles
        for (int i = 0; i < comboTiles.Count; i++) {

            // GetRange is needed to drop the last tile for Kong combos
            foreach (Tile comboTile in comboTiles[i].GetRange(0, 3)) {
                combinedHand.Add(comboTile);
            }
        }
        combinedHand = combinedHand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();

        if (listOfCombos == null) {
            return;
        }

        if (listOfCombos.Count == 0) {
            winningCombos = new List<string>();

            this.FanInFirstRound(allPlayersOpenTiles, playerWind, discardPlayerWind, discardTile, hasDrawnTile, turn);
            this.FanInBonusTiles(bonusTiles, playerWind, allPlayersOpenTiles);
            this.RobbingTheKong(discardTile);
            winningCombos.Add(this.ThirteenWondersCheck(combinedHand));

            if (settingsDict["Four Great Blessings"] > 0) {
                List<string> winCombos = new List<string>(winningCombos);

                winningCombos.Add(this.FourBlessingsCheck(combinedHand));

                if (winningCombos.Contains("Four Great Blessings")) {
                    winningCombos.Remove("Seat Wind Combo");
                    winningCombos.Remove("Prevailing Wind Combo");
                }
            }

            if (settingsDict["Three Great Scholars"] > 0) {
                winningCombos.Add(this.ThreeScholarsCheck(combinedHand));

                if (winningCombos.Contains("Three Great Scholars")) {
                    winningCombos.Remove("Dragon_One");
                    winningCombos.Remove("Dragon_Two");
                    winningCombos.Remove("Dragon_Three");
                }
            }

            if (!winningCombos.Contains("Thirteen Wonders") && !winningCombos.Contains("Four Great Blessings") && 
                !winningCombos.Contains("Three Great Scholars") && !winningCombos.Contains("Robbing the Eighth") &&
                !winningCombos.Contains("All Flowers and Seasons")) {
                winningCombos.Clear();
            }

            this.listOfWinningCombos.Add(winningCombos);
            return;
        }

        for (int i = 0; i < listOfCombos.Count; i++) {
            winningCombos = new List<string>();

            this.FanInFirstRound(allPlayersOpenTiles, playerWind, discardPlayerWind, discardTile, hasDrawnTile, turn);
            this.FanInBonusTiles(bonusTiles, playerWind, allPlayersOpenTiles);
            this.FanInHonourTiles(combinedHand, playerWind, prevailingWind);
            this.FanInHand(listOfCombos[i], combinedHand, hand, bonusTiles, comboTiles, playerWind, prevailingWind, discardTile);
            this.WinningOnReplacementTile(numberOfReplacementTiles, numberOfKong);
            this.RobbingTheKong(discardTile);
            this.WinningOnTheLastAvailableTile(numberOfReplacementTiles, numberOfKong, numberOfTilesLeft);

            this.listOfWinningCombos.Add(winningCombos);
        }
    }

    #region Fan Checks

    /// <summary>
    /// Container for Heavenly, Earthly and Humanly Hands
    /// </summary>
    private void FanInFirstRound(List<Tile> allPlayersOpenTiles, PlayerManager.Wind playerWind, PlayerManager.Wind? discardPlayerWind, Tile discardTile, bool hasDrawnTile, int turn) {
        if (settingsDict["Heavenly Hand"] > 0) {
            winningCombos.Add(HeavenlyHandCheck(playerWind, discardTile, hasDrawnTile, turn));

            if (winningCombos.Contains("Heavenly Hand")) {
                return;
            }
        }

        if (settingsDict["Earthly Hand"] > 0) {
            winningCombos.Add(EarthlyHandCheck(discardPlayerWind, discardTile, turn));

            if (winningCombos.Contains("Earthly Hand")) {
                return;
            }
        }

        if (settingsDict["Humanly Hand"] > 0) {
            winningCombos.Add(HumanlyHandCheck(allPlayersOpenTiles, discardTile, hasDrawnTile, turn));
        }

    }


    /// <summary>
    /// Calculates the number of Fan in bonus tiles
    /// </summary>
    private void FanInBonusTiles(List<Tile> bonusTiles, PlayerManager.Wind playerWind, List<Tile> allPlayersOpenTiles) {
        int numberOfSeasonTiles = 0;
        int numberOfFlowerTiles = 0;
        int numberOfAnimalTiles = 0;
        
        foreach (Tile bonusTile in bonusTiles) {
            if (DictManager.Instance.tileToWindDict.ContainsKey(bonusTile)) {

                if (settingsDict["Bonus Tile Match Seat Wind"] > 0) {
                    if (DictManager.Instance.tileToWindDict[bonusTile] == playerWind) {
                        winningCombos.Add("Bonus Tile Match Seat Wind");
                    }

                    if (bonusTile.suit == Tile.Suit.Season) {
                        numberOfSeasonTiles++;

                    } else {
                        numberOfFlowerTiles++;
                    }
                }

            } else {
                if (settingsDict["Animal"] > 0) {
                    winningCombos.Add("Animal");
                    numberOfAnimalTiles++;
                }
            }
        }

        // Complete Animal Group
        if (settingsDict["Complete Animal Group"] > 0) {
            if (numberOfAnimalTiles == 4) {
                winningCombos.Add("Complete Animal Group");
                winningCombos.RemoveAll(x => x == "Animal");
            }
        }

        // Complete Season Group
        if (settingsDict["Complete Season Group"] > 0) {
            if (numberOfSeasonTiles == 4) {
                winningCombos.Add("Complete Season Group");
                winningCombos.Remove("Bonus Tile Match Seat Wind");
            }
        }

        // Complete Flower Group
        if (settingsDict["Complete Flower Group"] > 0) {
            if (numberOfFlowerTiles == 4) {
                winningCombos.Add("Complete Flower Group");
                winningCombos.Remove("Bonus Tile Match Seat Wind");
            }
        }            

        // Robbing the Eighth
        if (settingsDict["Robbing the Eighth"] > 0) {
            if (numberOfSeasonTiles + numberOfFlowerTiles == 7) {
                foreach (Tile tile in allPlayersOpenTiles) {
                    if (bonusTiles.Contains(tile)) {
                        continue;
                    }
                    if (tile.suit == Tile.Suit.Season || tile.suit == Tile.Suit.Flower) {
                        winningCombos.Add("Robbing the Eighth");
                        winningCombos.Remove("Bonus Tile Match Seat Wind");
                        winningCombos.Remove("Complete Season Group");
                        winningCombos.Remove("Complete Flower Group");
                        return;
                    }
                }
            }
        }

        // All Flowers and Seasons
        if (settingsDict["All Flowers and Seasons"] > 0) {
            if (numberOfSeasonTiles + numberOfFlowerTiles == 8) {
                winningCombos.Add("All Flowers and Seasons");
                winningCombos.Remove("Complete Season Group");
                winningCombos.Remove("Complete Flower Group");
            }
        }
    }


    /// <summary>
    /// Calculate the number of Fan in the player's hand due to honour tiles
    /// </summary>
    private void FanInHonourTiles(List<Tile> combinedhand, PlayerManager.Wind playerWind, PlayerManager.Wind prevailingWind) {
        Dictionary<Tile, int> honourTilesCount = DictManager.Instance.HonourTilesCountDict();

        foreach (Tile tile in combinedhand) {
            if (honourTilesCount.ContainsKey(tile)) {
                honourTilesCount[tile]++;
            }
        }

        foreach (Tile tile in honourTilesCount.Keys) {
            if (honourTilesCount[tile] == 3) {

                if (settingsDict["Seat Wind Combo"] > 0) {
                    if (tile.suit == Tile.Suit.Wind && DictManager.Instance.tileToWindDict[tile] == playerWind) {
                        winningCombos.Add("Seat Wind Combo");
                    }
                }

                if (settingsDict["Prevailing Wind Combo"] > 0) {
                    if (tile.suit == Tile.Suit.Wind && DictManager.Instance.tileToWindDict[tile] == prevailingWind) {
                        winningCombos.Add("Prevailing Wind Combo");
                    }
                }

                if (settingsDict["Dragon"] > 0) {
                    if (tile.suit == Tile.Suit.Dragon) {
                        winningCombos.Add(tile.ToString());
                    }
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
        if (settingsDict["Fully Concealed"] > 0) {
            winningCombos.Add(this.FullyConcealedHandCheck(comboTiles, discardTile));
        }

        // Triplets Hand check
        if (settingsDict["Triplets"] > 0) {
            winningCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
        }

        // Half Flush & Full Flush check
        if (settingsDict["Half Flush"] > 0 || settingsDict["Full Flush"] > 0) {
            winningCombos.Add(this.FlushHandCheck(combinedHand));

            if (winningCombos.Contains("Half Flush")) {
                if (settingsDict["Half Flush"] == 0) {
                    winningCombos.Remove("Half Flush");
                }
            }

            if (winningCombos.Contains("Full Flush")) {
                if (settingsDict["Full Flush"] == 0) {
                    winningCombos.Remove("Full Flush");
                }
            }
        }

        // Sequence Hand check.
        if (settingsDict["Full Sequence"] > 0 || settingsDict["Lesser Sequence"] > 0) {
            string sequenceHandCheck = this.SequenceHandCheck(comboListNoDuplicate, combinedHand, originalHand, playerWind, prevailingWind, discardTile);

            if (sequenceHandCheck != null) {

                if (sequenceHandCheck.Equals("Sequence")) {
                    if (bonusTiles.Count > 0) {
                        if (settingsDict["Lesser Sequence"] > 0) {
                            winningCombos.Add("Lesser Sequence");
                        }

                    } else {
                        if (settingsDict["Lesser Sequence"] > 0) {
                            winningCombos.Add("Full Sequence");
                        }
                    }
                }
            }
        }

        // Mixed and Pure Terminals check. Prerequisite: Triplets
        if (settingsDict["Mixed Terminals"] > 0 || settingsDict["Pure Terminals"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (settingsDict["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (winCombos.Contains("Triplets")) {
                winningCombos.Add(this.TerminalsHandCheck(combinedHand));
            }

            if (winningCombos.Contains("Mixed Terminals")) {
                winningCombos.Remove("Triplets");

                if (settingsDict["Mixed Terminals"] == 0) {
                    winningCombos.Remove("Mixed Terminals");
                    winningCombos.Add("Triplets");
                }
            }

            if (winningCombos.Contains("Pure Terminals")) {
                winningCombos.Remove("Triplets");

                if (settingsDict["Pure Terminals"] == 0) {
                    winningCombos.Remove("Pure Terminals");
                    winningCombos.Add("Triplets");
                }
            }
        }

        // All Honour check. Prerequisite: Triplets
        if (settingsDict["All Honour"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (settingsDict["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (winCombos.Contains("Triplets")) {
                winningCombos.Add(this.AllHonourCheck(combinedHand));
            }

            if (winningCombos.Contains("All Honour")) {
                winningCombos.Remove("Triplets");
                winningCombos.Remove("Seat Wind Combo");
                winningCombos.Remove("Prevailing Wind Combo");
                winningCombos.Remove("Dragon_One");
                winningCombos.Remove("Dragon_Two");
                winningCombos.Remove("Dragon_Three");
            }
        }

        // Hidden Treasure check. Prerequisite: Triplets, Fully Concealed Hand
        if (settingsDict["Hidden Treasure"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (settingsDict["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (settingsDict["Fully Concealed"] == 0) {
                winCombos.Add(this.FullyConcealedHandCheck(comboTiles, discardTile));
            }

            if (winCombos.Contains("Triplets") && winCombos.Contains("Fully Concealed")) {
                winningCombos.Add("Hidden Treasure");
                winningCombos.Remove("Triplets");
                winningCombos.Remove("Fully Concealed");
            }
        }

        // Full Flush Triplets check. Prerequisite: Full Flush, Triplets
        if (settingsDict["Full Flush Triplets"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (settingsDict["Full Flush"] == 0) {
                winCombos.Add(this.FlushHandCheck(combinedHand));
            }

            if (settingsDict["Triplets"] == 0) {
                winCombos.Add(this.TripletsHandCheck(comboListNoDuplicate));
            }

            if (winCombos.Contains("Full Flush") && winCombos.Contains("Triplets")) {
                winningCombos.Add("Full Flush Triplets");
                winningCombos.Remove("Full Flush");
                winningCombos.Remove("Triplets");
            }
        }

        // Full Flush Full/Lesser Sequence Hand check. Prerequisite: Full Flush, Full Sequence/Lesser Sequence
        if (settingsDict["Full Flush Full Sequence"] > 0 || settingsDict["Full Flush Lesser Sequence"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (settingsDict["Full Flush"] == 0) {
                winCombos.Add(this.FlushHandCheck(combinedHand));
            }

            if (settingsDict["Full Sequence"] == 0 && settingsDict["Lesser Sequence"] == 0) {
                winCombos.Add(this.SequenceHandCheck(comboListNoDuplicate, combinedHand, originalHand, playerWind, prevailingWind, discardTile));
            }

            if (settingsDict["Full Flush Full Sequence"] > 0) {
                if (winCombos.Contains("Full Flush") && (winCombos.Contains("Full Sequence"))) {
                    winningCombos.Add("Full Flush Full Sequence");
                    winningCombos.Remove("Full Flush");
                    winningCombos.Remove("Full Sequence");
                }
            }

            if (settingsDict["Full Flush Lesser Sequence"] > 0) {
                if (winCombos.Contains("Full Flush") && winCombos.Contains("Lesser Sequence")) {
                    winningCombos.Add("Full Flush Lesser Sequence");
                    winningCombos.Remove("Full Flush");
                    winningCombos.Remove("Lesser Sequence");
                }
            }
        }

        // Nine Gates Check. Prerequisite: Full Flush
        if (settingsDict["Nine Gates"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (settingsDict["Full Flush"] == 0) {
                winCombos.Add(this.FlushHandCheck(combinedHand));
            }

            if (winCombos.Contains("Full Flush")) {
                winningCombos.Add(this.NineGatesCheck(combinedHand));
            }

            if (winningCombos.Contains("Nine Gates")) {
                winningCombos.Remove("Full Flush");
            }
        }

        // Four Lesser Blessings and Four Great Blessings check. 
        if (settingsDict["Four Lesser Blessings"] > 0 || settingsDict["Four Great Blessings"] > 0) {
            List<string> winCombos = new List<string>(winningCombos);

            if (!winningCombos.Contains("All Honour")) {
                winningCombos.Add(this.FourBlessingsCheck(combinedHand));
            }

            if (winningCombos.Contains("Four Lesser Blessings")) {
                if (settingsDict["Four Lesser Blessings"] == 0) {
                    winningCombos.Remove("Four Lesser Blessings");
                }
            }

            if (winningCombos.Contains("Four Great Blessings")) {
                if (settingsDict["Four Great Blessings"] == 0) {
                    winningCombos.Remove("Four Great Blessings");
                } else {
                    winningCombos.Remove("Seat Wind Combo");
                    winningCombos.Remove("Prevailing Wind Combo");
                }
            }
        }

        // Pure Green Suit check. Prerequisite-ish: Half Flush
        if (settingsDict["Pure Green Suit"] > 0) {
            winningCombos.Add(this.PureGreenSuitCheck(combinedHand));

            if (winningCombos.Contains("Pure Green Suit")) {
                winningCombos.Remove("Half Flush");
            }
        }

        // Three Lesser Scholars and Three Great Scholars check. 
        if (settingsDict["Three Lesser Scholars"] > 0 || settingsDict["Three Great Scholars"] > 0) {
            winningCombos.Add(this.ThreeScholarsCheck(combinedHand));

            if (winningCombos.Contains("Three Lesser Scholars")) {
                if (settingsDict["Three Lesser Scholars"] == 0) {
                    winningCombos.Remove("Three Lesser Scholars");
                } else {
                    winningCombos.Remove("Dragon_One");
                    winningCombos.Remove("Dragon_Two");
                    winningCombos.Remove("Dragon_Three");
                }
            }

            if (winningCombos.Contains("Three Great Scholars")) {
                if (settingsDict["Three Great Scholars"] == 0) {
                    winningCombos.Remove("Three Great Scholars");
                } else {
                    winningCombos.Remove("Dragon_One");
                    winningCombos.Remove("Dragon_Two");
                    winningCombos.Remove("Dragon_Three");
                }
            }
        }

        // Eighteen Arhats check
        if (settingsDict["Eighteen Arhats"] > 0) {
            winningCombos.Add(this.EighteenArhatsCheck(comboTiles));

            if (winningCombos.Contains("Eighteen Arhats")) {
                winningCombos.Remove("Triplets");
            }
        }
    }


    /// <summary>
    /// Container for Replacement Tile Win checks
    /// </summary>
    private void WinningOnReplacementTile(int numberOfReplacementTiles, int numberOfKong) {
        if (settingsDict["Replacement Tile for Flower"] > 0) {
            for (int i = 0; i < numberOfReplacementTiles; i++) {
                winningCombos.Add("Replacement Tile for Flower");
            }
        }

        if (settingsDict["Replacement Tile for Kong"] > 0) {
            winningCombos.Add(WinningOnReplacementTileForKong(numberOfKong));
        }

        if (settingsDict["Kong on Kong"] > 0) {
            winningCombos.Add(KongOnKong(numberOfKong));
        }
    }


    /// <summary>
    /// Determine if the player robbed the Kong
    /// </summary>
    private void RobbingTheKong(Tile discardTile) {
        if (discardTile == null) {
            return;
        }

        if (discardTile.kongType > 1) {
            winningCombos.Add("Robbing the Kong");
        }
    }


    /// <summary>
    /// Determine if the player won on the last available tile
    /// </summary>
    private void WinningOnTheLastAvailableTile(int numberOfReplacementTiles, int numberOfKong, int numberOfTilesLeft) {
        if (numberOfReplacementTiles == 0 && numberOfKong == 0 && numberOfTilesLeft == 15) {
            winningCombos.Add("Winning on Last Available Tile");
        }
    }

    #endregion

    #region FirstRoundHands

    /// <summary>
    /// Determine if the hand is a Heavenly Hand
    /// </summary>
    /// <returns></returns>
    private string HeavenlyHandCheck(PlayerManager.Wind playerWind, Tile discardTile, bool hasDrawnTile, int turn) {
        if (playerWind == PlayerManager.Wind.EAST && turn == 1 && discardTile == null && !hasDrawnTile) {
            return "Heavenly Hand";
        }
        return null;
    }


    /// <summary>
    /// Determine if the hand is a Earthly Hand
    /// </summary>
    private string EarthlyHandCheck(PlayerManager.Wind? discardPlayerWind, Tile discardTile, int turn) {
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
    private string HumanlyHandCheck(List<Tile> allPlayersOpenTiles, Tile discardTile, bool hasDrawnTile, int turn) {
        if (turn == 1 && discardTile != null) {
            int numberOfComboTiles = 0;
            int numberOfConcealedKong = 0;

            foreach (Tile tile in allPlayersOpenTiles) {
                if (tile.suit == Tile.Suit.Season || tile.suit == Tile.Suit.Flower || tile.suit == Tile.Suit.Animal) {
                    continue;
                }
                numberOfComboTiles++;

                if (tile.kongType == 3) {
                    numberOfConcealedKong++;
                }

            }

            if (numberOfConcealedKong * 4 != numberOfComboTiles) {
                return null;
            }

            if (hasDrawnTile) {
                return null;
            }

            return "Humanly Hand";
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
        if (comboListNoDuplicate.Contains("Chow")) {
            return null;
        }

        return "Triplets";
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
            } else if (i == 0 && (combinedHand[i].suit == Tile.Suit.Wind || combinedHand[i].suit == Tile.Suit.Dragon)) {
                break;
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

        if (comboListNoDuplicate.Contains("Pong") || comboListNoDuplicate.Contains("Kong")) {
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
            DictManager.Instance.windToTileDict[playerWind],
            DictManager.Instance.windToTileDict[prevailingWind],
            new Tile(Tile.Suit.Dragon, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three)
        };

        foreach (Tile tile in forbiddenTiles) {
            if (combinedHand.Contains(tile)) {
                return null;
            }
        }

        Tile tileMinusThree = new Tile(discardTile.suit, discardTile.rank - 3);
        Tile tilePlusThree = new Tile(discardTile.suit, discardTile.rank + 3);

        List<Tile> testHand;
        int testHandWinCombosCount = 0;

        if (tileMinusThree.rank != null) {
            testHand = new List<Tile>(combinedHand);
            testHand.Remove(discardTile);
            testHand.Add(tileMinusThree);
            testHand = testHand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();

            List<List<string>> testHandWinningCombos = winCombos.CheckWin(testHand);
            testHandWinCombosCount += testHandWinningCombos.Count;
        }

        if (tilePlusThree.rank != null) {
            testHand = new List<Tile>(combinedHand);
            testHand.Remove(discardTile);
            testHand.Add(tilePlusThree);
            testHand = testHand.OrderBy(x => x.suit).ThenBy(x => x.rank).ToList();

            List<List<string>> testHandWinningCombos = winCombos.CheckWin(testHand);
            testHandWinCombosCount += testHandWinningCombos.Count;
        }

        if (testHandWinCombosCount > 0) {
            return "Sequence";
        }

        return null;
    }


    /// <summary>
    /// Determine if the hand is a Mixed Terminals or Pure Terminals Hand
    /// </summary>
    private string TerminalsHandCheck(List<Tile> combinedHand) {

        if (combinedHand[0].suit == Tile.Suit.Wind || combinedHand[0].suit == Tile.Suit.Dragon) {
            return null;
        }

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
        Dictionary<Tile, int> nineGatesDict = DictManager.Instance.NineGatesDict();

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
            if (tile.suit != Tile.Suit.Bamboo && tile.suit != Tile.Suit.Dragon) {
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
    /// Determine if the hand is an Eighteen Arhats Hand
    /// </summary>
    private string EighteenArhatsCheck(List<List<Tile>> comboTiles) {
        int comboTilesCount = 0;

        foreach (List<Tile> combo in comboTiles) {
            comboTilesCount += combo.Count;
        }

        if (comboTilesCount == 16) {
            return "Eighteen Arhats";
        }

        return null;
    }


    /// <summary>
    /// Determine if the hand is a Thirteen Wonders Hand
    /// </summary>
    private string ThirteenWondersCheck(List<Tile> combinedHand) {
        Dictionary<Tile, int> thirteenWondersDict = DictManager.Instance.ThirteenWondersDict();

        HashSet<Tile> noDuplicateHand = new HashSet<Tile>(combinedHand);
        foreach (Tile tile in noDuplicateHand) {
            if (!thirteenWondersDict.ContainsKey(tile)) {
                return null;
            }

            thirteenWondersDict[tile]++;
        }

        foreach (int count in thirteenWondersDict.Values) {
            if (count != 1) {
                return null;
            }
        }

        return "Thirteen Wonders";
    }
    
    #endregion

    #region Winning on Replacement Tile

    /// <summary>
    /// Determine if the player won with a replacement tile from flower
    /// </summary>
    private string WinningOnReplacementTileForFlower(int numberOfReplacementTiles) { 
        if (numberOfReplacementTiles > 0) {
            return "Replacement Tile for Flower";
        }
        return null;
    }


    /// <summary>
    /// Determine if the player won with a replacement tile from Kong
    /// </summary>
    private string WinningOnReplacementTileForKong(int numberOfKong) {
        if (numberOfKong == 1) {
            return "Replacement Tile for Kong";
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
}
