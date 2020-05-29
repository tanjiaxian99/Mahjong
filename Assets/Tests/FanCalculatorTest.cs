using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TestTools;


namespace Tests {

    public class FanCalculatorTest {
        WinCombos testWin = new WinCombos();
        Dictionary<string, int> handsToCheck;
        FanCalculator fanCalculator;

        PlayerManager playerManager;
        Tile discardTile;
        PlayerManager.Wind discardPlayerWind;
        PlayerManager.Wind prevailingWind;
        int numberOfTilesLeft;
        int turn;
        List<Tile> allPlayersOpenTiles;

        public FanCalculatorTest() {
            this.TestInitialize();
        }

        public void TestInitialize() {
            handsToCheck = new Dictionary<string, int>();

            handsToCheck.Add("Fan Limit", 50);

            handsToCheck.Add("Heavenly Hand", 10);
            handsToCheck.Add("Earthly Hand", 10);
            handsToCheck.Add("Humanly Hand", 10);

            handsToCheck.Add("Bonus Tile Match Seat Wind", 1);
            handsToCheck.Add("Animal", 1);
            handsToCheck.Add("Complete Animal Group", 5);
            handsToCheck.Add("Complete Season Group", 2);
            handsToCheck.Add("Complete Flower Group", 2);
            handsToCheck.Add("Robbing the Eighth", 10);
            handsToCheck.Add("All Flowers and Seasons", 10);

            handsToCheck.Add("Player Wind Combo", 1);
            handsToCheck.Add("Prevailing Wind Combo", 1);
            handsToCheck.Add("Dragon", 1);

            handsToCheck.Add("Fully Concealed", 1);
            handsToCheck.Add("Triplets", 2);
            handsToCheck.Add("Half Flush", 2);
            handsToCheck.Add("Full Flush", 4);
            handsToCheck.Add("Lesser Sequence", 1);
            handsToCheck.Add("Full Sequence", 4);
            handsToCheck.Add("Mixed Terminals", 4);
            handsToCheck.Add("Pure Terminals", 10);
            handsToCheck.Add("All Honour", 10);
            handsToCheck.Add("Hidden Treasure", 10);
            handsToCheck.Add("Full Flush Triplets", 10);
            handsToCheck.Add("Full Flush Sequence", 10);
            handsToCheck.Add("Nine Gates", 10);
            handsToCheck.Add("Four Lesser Blessings", 2);
            handsToCheck.Add("Four Great Blessings", 10);
            handsToCheck.Add("Pure Green Suit", 4);
            handsToCheck.Add("Three Lesser Scholars", 3);
            handsToCheck.Add("Three Great Scholars", 10);
            handsToCheck.Add("Eighteen Arhats", 10);
            handsToCheck.Add("Thirteen Wonders", 10);

            handsToCheck.Add("Winning on Replacement Tile for Flower", 1);
            handsToCheck.Add("Winning on Replacement Tile for Kong", 1);
            handsToCheck.Add("Kong on Kong", 10);

            handsToCheck.Add("Robbing the Kong", 1);
            handsToCheck.Add("Winning on Last Available Tile", 1);

            fanCalculator = new FanCalculator(handsToCheck);
        }

        [Test]
        public void HeavenlyHand() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { };
            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 1;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Heavenly Hand", "Player Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedFan, actualFan);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
        }

        [Test]
        public void EarthlyHand() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { };
            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.EAST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 1;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Earthly Hand", });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedFan, actualFan);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
        }

        [Test]
        public void HumanlyHand() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { };
            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 1;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Humanly Hand", });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedFan, actualFan);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
        }

        [Test]
        public void BonusTileMatchSeatWind() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { 
                new Tile(Tile.Suit.Season, Tile.Rank.Two), 
                new Tile(Tile.Suit.Flower, Tile.Rank.Two)};

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Bonus Tile Match Seat Wind", "Bonus Tile Match Seat Wind" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void Animal() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Animal, Tile.Rank.One),
                new Tile(Tile.Suit.Animal, Tile.Rank.Two)};

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Animal", "Animal" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void CompleteAnimalGroup() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Animal, Tile.Rank.One),
                new Tile(Tile.Suit.Animal, Tile.Rank.Two),
                new Tile(Tile.Suit.Animal, Tile.Rank.Three),
                new Tile(Tile.Suit.Animal, Tile.Rank.Four)};

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (5, new List<string>() { "Complete Animal Group" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void CompleteSeasonGroup() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Season, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Season, Tile.Rank.Three),
                new Tile(Tile.Suit.Season, Tile.Rank.Four)};

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Complete Season Group" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void CompleteFlowerGroup() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Flower, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Three),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four)};

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Complete Flower Group" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void RobbingTheEighth() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two)};

            playerManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Flower, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Three),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four),
                new Tile(Tile.Suit.Season, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Season, Tile.Rank.Three)};

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Four)};

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Robbing the Eighth" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void AllFlowersAndSeasons() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two)};

            playerManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Flower, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Three),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four),
                new Tile(Tile.Suit.Season, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Season, Tile.Rank.Three),
                new Tile(Tile.Suit.Season, Tile.Rank.Four)};

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "All Flowers and Seasons" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PlayerWindCombo() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { };

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (1, new List<string>() { "Player Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PrevailingWindCombo() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { };

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.EAST;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (1, new List<string>() { "Prevailing Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PlayerAndPrevailingWindCombo() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { };

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.EAST;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Player Wind Combo", "Prevailing Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void Dragon() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),};

            playerManager.bonusTiles = new List<Tile>() { };

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.EAST;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Dragon_One", "Dragon_Two" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
            LogAssert.Expect(LogType.Error, "The Dragon Tile has a greater rank than three");
        }

        [Test]
        public void FullyConcealed() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            playerManager.bonusTiles = new List<Tile>() { };

            playerManager.comboTiles = new List<List<Tile>>() { };
            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (1, new List<string>() { "Fully Concealed" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void Triplets() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One) };


            playerManager.bonusTiles = new List<Tile>() { };

            playerManager.comboTiles = new List<List<Tile>>() { 
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two) 
                } 
            };

            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Dragon, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Triplets" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void HalfFlush() {
            playerManager = new PlayerManager();
            playerManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One) };


            playerManager.bonusTiles = new List<Tile>() { };

            playerManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two)
                }
            };

            playerManager.playerWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Dragon, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (2, new List<string>() { "Half Flush" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }


        //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        //// `yield return null;` to skip a frame.
        //[UnityTest]
        //public IEnumerator TestSuiteWinComboTypesWithEnumeratorPasses() {
        //    // Use the Assert class to test conditions.
        //    // Use yield to skip a frame.
        //    yield return null;
        //}
    }
}
