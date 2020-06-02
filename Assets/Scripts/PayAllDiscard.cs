using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class PayAllDiscard {
    #region Private Fields

    private Dictionary<string, int> handsToCheck;
    private Dictionary<Tile, PlayerManager.Wind> bonusTileToWindDict;
    private Dictionary<PlayerManager.Wind, List<Tile>> windToTileDict;
    private Dictionary<Tile.Suit?, List<Tile>> fullFlushDict;

    private Tile dragonOne;
    private Tile dragonTwo;
    private Tile dragonThree;

    private Tile windOne;
    private Tile windTwo;
    private Tile windThree;
    private Tile windFour;

    private Tile animalOne;
    private Tile animalTwo;
    private Tile animalThree;
    private Tile animalFour;

    #endregion

    public PayAllDiscard(Dictionary<string, int> handsToCheck) {
        this.handsToCheck = handsToCheck;
        this.bonusTileToWindDict = new Dictionary<Tile, PlayerManager.Wind>();
        this.windToTileDict = new Dictionary<PlayerManager.Wind, List<Tile>>();

        this.InitializeWindToTileDict();
        this.InitializeFullFlushDict();
        this.InitializeTiles();
    }

    /// <summary>
    /// Determine the high risk discard in a Dragon Tile Set Scenario
    /// </summary>
    public List<Tile> DragonTileSet(List<Tile> openTiles) {
        List<Tile> highRiskTiles = new List<Tile>();

        if (handsToCheck["Dragon Tile Set Pay All"] == 0) {
            return highRiskTiles;
        } else if (handsToCheck["Three Lesser Scholars"] == 0 && handsToCheck["Three Great Scholars"] == 0) {
            return highRiskTiles;
        }

        if (!openTiles.Contains(dragonOne) && openTiles.Contains(dragonTwo) && openTiles.Contains(dragonThree)) {
            highRiskTiles.Add(dragonOne);
        } else if (openTiles.Contains(dragonOne) && !openTiles.Contains(dragonTwo) && openTiles.Contains(dragonThree)) {
            highRiskTiles.Add(dragonTwo);
        } else if (openTiles.Contains(dragonOne) && openTiles.Contains(dragonTwo) && !openTiles.Contains(dragonThree)) {
            highRiskTiles.Add(dragonThree);
        }

        return highRiskTiles;
    }


    /// <summary>
    /// Determine the high risk discard in a Wind Tile Set Scenario
    /// </summary>
    public List<Tile> WindTileSet(List<Tile> openTiles) {
        List<Tile> highRiskTiles = new List<Tile>();

        if (handsToCheck["Wind Tile Set Pay All"] == 0) {
            return highRiskTiles;
        } 

        if (handsToCheck["Four Lesser Blessings"] > 0 || handsToCheck["Four Great Blessings"] > 0) {
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

        if (handsToCheck["All Honour"] > 0) {
            highRiskTiles.Add(dragonOne);
            highRiskTiles.Add(dragonTwo);
            highRiskTiles.Add(dragonThree);
        }
        
        return highRiskTiles;
    }


    /// <summary>
    /// Determine the high risk discards in a Point Limit Scenario
    /// </summary>
    public List<Tile> PointLimit(List<Tile> openTiles, PlayerManager.Wind playerWind, PlayerManager.Wind prevailingWind) {
        List<Tile> highRiskTiles = new List<Tile>();
        int totalFan = 0;

        if (handsToCheck["Point Limit Pay All"] == 0) {
            return highRiskTiles;
        }

        Dictionary<Tile, int> highRiskDict = new Dictionary<Tile, int>();
        highRiskDict.Add(windToTileDict[playerWind][2], 0);
        highRiskDict.Add(windToTileDict[prevailingWind][2], 0);
        highRiskDict.Add(dragonOne, 0);
        highRiskDict.Add(dragonTwo, 0);
        highRiskDict.Add(dragonThree, 0);

        int seasonTilesCount = 0;
        int flowerTilesCount = 0;
        int animalTilesCount = 0;

        // Bonus Tile Match Seat Wind: Season & Flower. Player Wind Combo: Wind. Prevailing Wind Combo: Wind
        foreach (Tile tile in openTiles) {

            if (tile.suit == Tile.Suit.Season || tile.suit == Tile.Suit.Flower) {
                totalFan += handsToCheck["Bonus Tile Match Seat Wind"];
            }

            if (tile.suit == Tile.Suit.Wind) {
                if (tile == windToTileDict[playerWind][2]) {
                    highRiskDict[tile] += handsToCheck["Player Wind Combo"];
                }

                if (tile == windToTileDict[prevailingWind][2]) {
                    highRiskDict[tile] += handsToCheck["Prevailing Wind Combo"];
                }
            }

            if (tile.suit == Tile.Suit.Dragon) {
                highRiskDict[tile] += handsToCheck["Dragon"];
            }

            if (tile.suit == Tile.Suit.Animal) {
                totalFan += handsToCheck["Animal"];
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

        foreach (int fan in highRiskDict.Values) {
            totalFan += fan;
        }

        if (seasonTilesCount == 4) {
            totalFan += handsToCheck["Complete Season Group"];
            totalFan -= handsToCheck["Bonus Tile Match Seat Wind"];
        }

        if (flowerTilesCount == 4) {
            totalFan += handsToCheck["Complete Flower Group"];
            totalFan -= handsToCheck["Bonus Tile Match Seat Wind"];
        }

        if (animalTilesCount == 4) {
            totalFan += handsToCheck["Complete Animal Group"];
            totalFan -= handsToCheck["Animal"] * 4;
        }

        // If fan limit is 5, there are high risk discards only when there are at least 3 fan.
        if (totalFan == handsToCheck["Fan Limit"] - 2) {
            if (playerWind == prevailingWind && highRiskDict[windToTileDict[playerWind][2]] == 0) {
                highRiskTiles.Add(windToTileDict[playerWind][2]);
                return highRiskTiles;
            }

        } else if (totalFan >= handsToCheck["Fan Limit"] - 1) {
            foreach (Tile tile in highRiskDict.Keys) {
                if (highRiskDict[tile] == 0) {
                    highRiskTiles.Add(tile);
                }
            }
        }

        return highRiskTiles;
    }


    /// <summary>
    /// Determine the high risk discards in Full Flush Scenario
    /// </summary>
    public List<Tile> FullFlush(List<Tile> openTiles) {
        List<Tile> highRiskTiles = new List<Tile>();
        Tile.Suit? referenceSuit = openTiles[openTiles.Count - 1].suit;
        int referenceSuitCount = 0;

        if (referenceSuit == Tile.Suit.Season || referenceSuit == Tile.Suit.Flower || referenceSuit == Tile.Suit.Animal) {
            return highRiskTiles;
        }

        foreach (Tile tile in openTiles) {
            if (tile.suit != referenceSuit) {
                if (tile.suit != Tile.Suit.Season && tile.suit != Tile.Suit.Flower && tile.suit != Tile.Suit.Animal) {
                    return highRiskTiles;
                }

                if (tile.suit == Tile.Suit.Wind && referenceSuit == Tile.Suit.Dragon) {
                    referenceSuitCount++;
                }

                if (tile.suit == Tile.Suit.Dragon && referenceSuit == Tile.Suit.Wind) {
                    referenceSuitCount++;
                }

            } else {
                referenceSuitCount++;
            }
        }

        if (referenceSuitCount < 9) {
            return highRiskTiles;
        }

        return fullFlushDict[referenceSuit];

    }

    #region Initializer Functions

    /// <summary>
    /// Initialize the Wind to Tile Dictionary
    /// </summary>
    private void InitializeWindToTileDict() {
        this.windToTileDict = new Dictionary<PlayerManager.Wind, List<Tile>>();
        this.windToTileDict.Add(PlayerManager.Wind.EAST, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.One),
            new Tile(Tile.Suit.Flower, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.One) });
        this.windToTileDict.Add(PlayerManager.Wind.SOUTH, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.Two),
            new Tile(Tile.Suit.Flower, Tile.Rank.Two),
            new Tile(Tile.Suit.Wind, Tile.Rank.Two)});
        this.windToTileDict.Add(PlayerManager.Wind.WEST, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.Three),
            new Tile(Tile.Suit.Flower, Tile.Rank.Three),
            new Tile(Tile.Suit.Wind, Tile.Rank.Three)});
        this.windToTileDict.Add(PlayerManager.Wind.NORTH, new List<Tile>() {
            new Tile(Tile.Suit.Season, Tile.Rank.Four),
            new Tile(Tile.Suit.Flower, Tile.Rank.Four),
            new Tile(Tile.Suit.Wind, Tile.Rank.Four)});
    }


    /// <summary>
    /// Initializes the Full Flush Dictionary
    /// </summary>
    private void InitializeFullFlushDict() {
        this.fullFlushDict = new Dictionary<Tile.Suit?, List<Tile>>();
        fullFlushDict.Add(Tile.Suit.Character, new List<Tile>() {
            new Tile(Tile.Suit.Character, Tile.Rank.One),
            new Tile(Tile.Suit.Character, Tile.Rank.Two),
            new Tile(Tile.Suit.Character, Tile.Rank.Three),
            new Tile(Tile.Suit.Character, Tile.Rank.Four),
            new Tile(Tile.Suit.Character, Tile.Rank.Five),
            new Tile(Tile.Suit.Character, Tile.Rank.Six),
            new Tile(Tile.Suit.Character, Tile.Rank.Seven),
            new Tile(Tile.Suit.Character, Tile.Rank.Eight),
            new Tile(Tile.Suit.Character, Tile.Rank.Nine)
        });

        fullFlushDict.Add(Tile.Suit.Dot, new List<Tile>() {
            new Tile(Tile.Suit.Dot, Tile.Rank.One),
            new Tile(Tile.Suit.Dot, Tile.Rank.Two),
            new Tile(Tile.Suit.Dot, Tile.Rank.Three),
            new Tile(Tile.Suit.Dot, Tile.Rank.Four),
            new Tile(Tile.Suit.Dot, Tile.Rank.Five),
            new Tile(Tile.Suit.Dot, Tile.Rank.Six),
            new Tile(Tile.Suit.Dot, Tile.Rank.Seven),
            new Tile(Tile.Suit.Dot, Tile.Rank.Eight),
            new Tile(Tile.Suit.Dot, Tile.Rank.Nine)
        });

        fullFlushDict.Add(Tile.Suit.Bamboo, new List<Tile>() {
            new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Seven),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine)
        });

        fullFlushDict.Add(Tile.Suit.Wind, new List<Tile>() {
            new Tile(Tile.Suit.Wind, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.Two),
            new Tile(Tile.Suit.Wind, Tile.Rank.Three),
            new Tile(Tile.Suit.Wind, Tile.Rank.Four),
            new Tile(Tile.Suit.Dragon, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
        });

        fullFlushDict.Add(Tile.Suit.Dragon, new List<Tile>() {
            new Tile(Tile.Suit.Wind, Tile.Rank.One),
            new Tile(Tile.Suit.Wind, Tile.Rank.Two),
            new Tile(Tile.Suit.Wind, Tile.Rank.Three),
            new Tile(Tile.Suit.Wind, Tile.Rank.Four),
            new Tile(Tile.Suit.Dragon, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
        });
    }


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

        this.animalOne = new Tile(Tile.Suit.Animal, Tile.Rank.One);
        this.animalTwo = new Tile(Tile.Suit.Animal, Tile.Rank.Two);
        this.animalThree = new Tile(Tile.Suit.Animal, Tile.Rank.Three);
        this.animalFour = new Tile(Tile.Suit.Animal, Tile.Rank.Four);
    }
    #endregion

}