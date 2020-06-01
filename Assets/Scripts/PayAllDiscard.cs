using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class PayAllDiscard {
    #region Private Fields

    private Dictionary<string, int> handsToCheck;
    private Dictionary<Tile, PlayerManager.Wind> bonusTileToWindDict;
    private Dictionary<PlayerManager.Wind, List<Tile>> windToTileDict;

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

        if (handsToCheck["Point Limit Pay All"] == 0) {
            return highRiskTiles;
        }

        Dictionary<Tile, int> openTilesDict = new Dictionary<Tile, int>();

        openTilesDict.Add(windToTileDict[playerWind][0], 0);
        openTilesDict.Add(windToTileDict[playerWind][1], 0);
        openTilesDict.Add(windToTileDict[playerWind][2], 0);
        openTilesDict.Add(windToTileDict[prevailingWind][2], 0);
        openTilesDict.Add(dragonOne, 0);
        openTilesDict.Add(dragonTwo, 0);
        openTilesDict.Add(dragonThree, 0);
        openTilesDict.Add(animalOne, 0);
        openTilesDict.Add(animalTwo, 0);
        openTilesDict.Add(animalThree, 0);
        openTilesDict.Add(animalFour, 0);

        int seasonTilesCount = 0;
        int flowerTilesCount = 0;
        int animalTilesCount = 0;

        // Bonus Tile Match Seat Wind: Season & Flower. Player Wind Combo: Wind. Prevailing Wind Combo: Wind
        foreach (Tile tile in openTiles) {
            if (openTilesDict.Keys.Contains(tile)) {

                if (tile.suit == Tile.Suit.Season || tile.suit == Tile.Suit.Flower) {
                    openTilesDict[tile] += handsToCheck["Bonus Tile Match Seat Wind"];
                }

                if (tile.suit == Tile.Suit.Wind) {
                    if (tile == windToTileDict[playerWind][2]) {
                        openTilesDict[tile] += handsToCheck["Player Wind Combo"];
                    }

                    if (tile == windToTileDict[prevailingWind][2]) {
                        openTilesDict[tile] += handsToCheck["Prevailing Wind Combo"];
                    }
                }

                if (tile.suit == Tile.Suit.Animal) {
                    openTilesDict[tile] += handsToCheck["Animal"];
                }

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

        // Calculate total fan in open tiles
        int totalFan = 0;
        foreach (int fan in openTilesDict.Values) {
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
        if (totalFan >= handsToCheck["Fan Limit"] - 2 && totalFan <= handsToCheck["Fan Limit"]) {

        }

        return highRiskTiles;
    }
}