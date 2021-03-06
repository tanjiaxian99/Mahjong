﻿using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class HighRiskTiles : MonoBehaviour {
    #region Private Fields

    [SerializeField]
    private GameObject scriptManager;

    private Dictionary<string, int> settingsDict;

    private List<string> highRiskScenarios;

    private Tile dragonOne;
    private Tile dragonTwo;
    private Tile dragonThree;

    private Tile windOne;
    private Tile windTwo;
    private Tile windThree;
    private Tile windFour;

    #endregion

    private void Start() {
        Settings settings = scriptManager.GetComponent<Settings>();
        settingsDict = settings.settingsDict;

        this.highRiskScenarios = new List<string>();
        this.InitializeTiles();
    }

    /// <summary>
    /// For Unit Testing. 
    /// </summary>
    public HighRiskTiles(Dictionary<string, int> settingsDict) {
        this.settingsDict = settingsDict;

        this.highRiskScenarios = new List<string>();
        this.InitializeTiles();
    }

    /// <summary>
    /// Wrapper method for High Risk Discards
    /// </summary>
    public List<Tile> HighRiskDiscards(List<Tile> openTiles, PlayerManager.Wind playerWind, PlayerManager.Wind prevailingWind) {
        List<Tile> highRiskTiles = new List<Tile>();
        highRiskScenarios = new List<string>();

        highRiskTiles.AddRange(this.DragonTileSet(openTiles));
        //highRiskTiles.AddRange(this.WindTileSet(openTiles));
        highRiskTiles.AddRange(this.PointLimit(openTiles, playerWind, prevailingWind));
        highRiskTiles.AddRange(this.FullFlush(openTiles));
        highRiskTiles.AddRange(this.PureTerminals(openTiles));

        return highRiskTiles;
    }


    /// <summary>
    /// Returns the list of High Risk Scenarios which the discard tile falls under
    /// </summary>
    public List<string> HighRiskScenarios() {
        return highRiskScenarios;
    }


    /// <summary>
    /// Determine the high risk discard in a Dragon Tile Set Scenario
    /// </summary>
    private List<Tile> DragonTileSet(List<Tile> openTiles) {
        List<Tile> highRiskTiles = new List<Tile>();

        if (settingsDict["Dragon Tile Set Pay All"] == 0) {
            return highRiskTiles;
        } else if (settingsDict["Three Lesser Scholars"] == 0 && settingsDict["Three Great Scholars"] == 0) {
            return highRiskTiles;
        }

        if (!openTiles.Contains(dragonOne) && openTiles.Contains(dragonTwo) && openTiles.Contains(dragonThree)) {
            highRiskTiles.Add(dragonOne);
        } else if (openTiles.Contains(dragonOne) && !openTiles.Contains(dragonTwo) && openTiles.Contains(dragonThree)) {
            highRiskTiles.Add(dragonTwo);
        } else if (openTiles.Contains(dragonOne) && openTiles.Contains(dragonTwo) && !openTiles.Contains(dragonThree)) {
            highRiskTiles.Add(dragonThree);
        }

        if (highRiskTiles.Count != 0) {
            highRiskScenarios.Add("Dragon Tile Set");
        }

        return highRiskTiles;
    }


    /// <summary>
    /// Determine the high risk discard in a Wind Tile Set Scenario
    /// </summary>
    private List<Tile> WindTileSet(List<Tile> openTiles) {
        List<Tile> highRiskTiles = new List<Tile>();

        if (settingsDict["Wind Tile Set Pay All"] == 0) {
            return highRiskTiles;
        } 

        if (settingsDict["Four Lesser Blessings"] > 0 || settingsDict["Four Great Blessings"] > 0) {
            if (!openTiles.Contains(windOne) && openTiles.Contains(windTwo) && openTiles.Contains(windThree) && openTiles.Contains(windFour)) {
                highRiskTiles.Add(windOne);
            } else if (openTiles.Contains(windOne) && !openTiles.Contains(windTwo) && openTiles.Contains(windThree) && openTiles.Contains(windFour)) {
                highRiskTiles.Add(windTwo);
            } else if (openTiles.Contains(windOne) && openTiles.Contains(windTwo) && !openTiles.Contains(windThree) && openTiles.Contains(windFour)) {
                highRiskTiles.Add(windThree);
            } else if (openTiles.Contains(windOne) && openTiles.Contains(windTwo) && openTiles.Contains(windThree) && !openTiles.Contains(windFour)) {
                highRiskTiles.Add(windFour);
            } else {
                return highRiskTiles;
            }
        }

        if (settingsDict["All Honour"] > 0) {
            highRiskTiles.Add(dragonOne);
            highRiskTiles.Add(dragonTwo);
            highRiskTiles.Add(dragonThree);
        }
        
        return highRiskTiles;
    }


    /// <summary>
    /// Determine the high risk discards in a Point Limit Scenario
    /// </summary>
    private List<Tile> PointLimit(List<Tile> openTiles, PlayerManager.Wind playerWind, PlayerManager.Wind prevailingWind) {
        List<Tile> highRiskTiles = new List<Tile>();
        int totalFan = 0;

        if (settingsDict["Point Limit Pay All"] == 0) {
            return highRiskTiles;
        }

        Dictionary<Tile, int> highRiskDict = new Dictionary<Tile, int>();
        highRiskDict.Add(dragonOne, 0);
        highRiskDict.Add(dragonTwo, 0);
        highRiskDict.Add(dragonThree, 0);
        highRiskDict.Add(DictManager.Instance.windTo3TilesDict[playerWind][2], 0);
        if (playerWind != prevailingWind) {
            highRiskDict.Add(DictManager.Instance.windTo3TilesDict[prevailingWind][2], 0);
        }

        int seasonTilesCount = 0;
        int flowerTilesCount = 0;
        int animalTilesCount = 0;
        

        // Bonus Tile Match Seat Wind: Season & Flower. Seat Wind Combo: Wind. Prevailing Wind Combo: Wind
        foreach (Tile tile in openTiles) {

            if (tile == DictManager.Instance.windTo3TilesDict[playerWind][0] || tile == DictManager.Instance.windTo3TilesDict[playerWind][1]) {
                totalFan += settingsDict["Bonus Tile Match Seat Wind"];
            }

            if (tile.suit == Tile.Suit.Animal) {
                totalFan += settingsDict["Animal"];
            }

            if (tile.suit == Tile.Suit.Season) {
                seasonTilesCount++;
            }

            if (tile.suit == Tile.Suit.Flower) {
                flowerTilesCount++;
            }

            if (tile.suit == Tile.Suit.Animal) {
                animalTilesCount++;
            }
        }

        if (openTiles.Contains(DictManager.Instance.windTo3TilesDict[playerWind][2])) {
            highRiskDict[DictManager.Instance.windTo3TilesDict[playerWind][2]] += settingsDict["Seat Wind Combo"];
        }

        if (openTiles.Contains(DictManager.Instance.windTo3TilesDict[prevailingWind][2])) {
            highRiskDict[DictManager.Instance.windTo3TilesDict[prevailingWind][2]] += settingsDict["Prevailing Wind Combo"];
        }

        if (openTiles.Contains(dragonOne)) {
            highRiskDict[dragonOne] += settingsDict["Dragon"];
        }

        if (openTiles.Contains(dragonTwo)) {
            highRiskDict[dragonTwo] += settingsDict["Dragon"];
        }

        if (openTiles.Contains(dragonThree)) {
            highRiskDict[dragonThree] += settingsDict["Dragon"];
        }


        foreach (int fan in highRiskDict.Values) {
            totalFan += fan;
        }

        if (seasonTilesCount == 4) {
            totalFan += settingsDict["Complete Season Group"];
            totalFan -= settingsDict["Bonus Tile Match Seat Wind"];
        }

        if (flowerTilesCount == 4) {
            totalFan += settingsDict["Complete Flower Group"];
            totalFan -= settingsDict["Bonus Tile Match Seat Wind"];
        }

        if (animalTilesCount == 4) {
            totalFan += settingsDict["Complete Animal Group"];
            totalFan -= settingsDict["Animal"] * 4;
        }

        // If fan limit is 5, there are high risk discards only when there are at least 3 fan.
        if (totalFan == settingsDict["Fan Limit"] - 2) {
            if (playerWind == prevailingWind && highRiskDict[DictManager.Instance.windTo3TilesDict[playerWind][2]] == 0) {
                highRiskTiles.Add(DictManager.Instance.windTo3TilesDict[playerWind][2]);
                return highRiskTiles;
            }

        } else if (totalFan >= settingsDict["Fan Limit"] - 1) {
            foreach (Tile tile in highRiskDict.Keys) {
                if (highRiskDict[tile] == 0) {
                    highRiskTiles.Add(tile);
                }
            }
        }

        if (highRiskTiles.Count != 0) {
            highRiskScenarios.Add("Point Limit");
        }

        return highRiskTiles;
    }


    /// <summary>
    /// Determine the high risk discards in Full Flush Scenario
    /// </summary>
    private List<Tile> FullFlush(List<Tile> openTiles) {
        List<Tile> highRiskTiles = new List<Tile>();
        if (openTiles.Count == 0) {
            return highRiskTiles;
        }

        Tile.Suit? referenceSuit = openTiles[openTiles.Count - 1].suit;
        int referenceSuitCount = 0;

        if (settingsDict["Full Flush Pay All"] == 0) {
            return highRiskTiles;
        }

        if (referenceSuit == Tile.Suit.Season || referenceSuit == Tile.Suit.Flower || referenceSuit == Tile.Suit.Animal) {
            return highRiskTiles;
        }

        foreach (Tile tile in openTiles) {
            if (tile.suit != referenceSuit) {
                if (tile.suit == Tile.Suit.Wind && referenceSuit == Tile.Suit.Dragon) {
                    referenceSuitCount++;
                    continue;
                }

                if (tile.suit == Tile.Suit.Dragon && referenceSuit == Tile.Suit.Wind) {
                    referenceSuitCount++;
                    continue;
                }

                if (tile.suit != Tile.Suit.Season && tile.suit != Tile.Suit.Flower && tile.suit != Tile.Suit.Animal) {
                    return highRiskTiles;
                }

            } else {
                referenceSuitCount++;
            }
        }

        if (referenceSuitCount < 9) {
            return highRiskTiles;
        }

        highRiskTiles = DictManager.Instance.fullFlushDict[referenceSuit];

        var q = from x in openTiles
                group x by x into g
                let count = g.Count()
                orderby count descending
                select new { Tile = g.Key, Count = count };

        foreach (var tileCountDict in q) {
            if (tileCountDict.Count == 4) {
                highRiskTiles.Remove(tileCountDict.Tile);
            }

            if (tileCountDict.Count == 3 && (tileCountDict.Tile.suit == Tile.Suit.Wind || tileCountDict.Tile.suit == Tile.Suit.Dragon)) {
                highRiskTiles.Remove(tileCountDict.Tile);
            }
        }

        if (highRiskTiles.Count != 0) {
            highRiskScenarios.Add("Full Flush");
        }

        return highRiskTiles;
    }


    /// <summary>
    /// Determine the high risk discards in Pure Terminals Scenario
    /// </summary>
    private List<Tile> PureTerminals(List<Tile> openTiles) {
        List<Tile> highRiskTiles = new List<Tile>();
        Dictionary<Tile, int> pureTerminalsDict = new Dictionary<Tile, int>();
        int terminalsCount = 0;

        if (settingsDict["Pure Terminals Pay All"] == 0) {
            return highRiskTiles;
        }

        pureTerminalsDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.One), 0);
        pureTerminalsDict.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine), 0);
        pureTerminalsDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One), 0);
        pureTerminalsDict.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Nine), 0);
        pureTerminalsDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One), 0);
        pureTerminalsDict.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine), 0);

        List<Tile> terminalTiles = pureTerminalsDict.Keys.ToList();
        foreach (Tile terminalTile in terminalTiles) {
            if (openTiles.Contains(terminalTile)) {
                pureTerminalsDict[terminalTile]++;
                terminalsCount++;
            }
        }

        if (terminalsCount < 3) {
            return highRiskTiles;
        }

        foreach (Tile terminalTile in pureTerminalsDict.Keys) {
            if (pureTerminalsDict[terminalTile] == 0) {
                highRiskTiles.Add(terminalTile);
            }
        }

        if (highRiskTiles.Count != 0) {
            highRiskScenarios.Add("Pure Terminals");
        }

        return highRiskTiles;
    }


    // Eighteen Arhats Scenario


    /// <summary>
    /// Initialize tiles
    /// </summary>
    private void InitializeTiles() {
        this.dragonOne = new Tile(Tile.Suit.Dragon, Tile.Rank.One);
        this.dragonTwo = new Tile(Tile.Suit.Dragon, Tile.Rank.Two);
        this.dragonThree = new Tile(Tile.Suit.Dragon, Tile.Rank.Three);

        this.windOne = new Tile(Tile.Suit.Wind, Tile.Rank.One);
        this.windTwo = new Tile(Tile.Suit.Wind, Tile.Rank.Two);
        this.windThree = new Tile(Tile.Suit.Wind, Tile.Rank.Three);
        this.windFour = new Tile(Tile.Suit.Wind, Tile.Rank.Four);
    }

}