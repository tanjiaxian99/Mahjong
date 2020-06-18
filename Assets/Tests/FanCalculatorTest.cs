using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal.Builders;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TestTools;


namespace Tests {

    public class FanCalculatorTest {
        WinCombos testWin = new WinCombos();
        Dictionary<string, int> handsToCheck;
        FanCalculator fanCalculator;

        PlayerManager playerManager;
        TilesManager tilesManager;
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
            handsToCheck.Add("Full Flush Full Sequence", 10);
            handsToCheck.Add("Full Flush Lesser Sequence", 5);
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
            tilesManager = new TilesManager();

            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 2;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Heavenly Hand", "Player Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedFan, actualFan);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
        }

        [Test]
        public void EarthlyHand() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.EAST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 2;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Earthly Hand", });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedFan, actualFan);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
        }

        [Test]
        public void HumanlyHand() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 2;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Humanly Hand", });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedFan, actualFan);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
        }

        [Test]
        public void BonusTileMatchSeatWind() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Two)};

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Bonus Tile Match Seat Wind"] + handsToCheck["Bonus Tile Match Seat Wind"], new List<string>() { "Bonus Tile Match Seat Wind", "Bonus Tile Match Seat Wind" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void Animal() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Animal, Tile.Rank.One),
                new Tile(Tile.Suit.Animal, Tile.Rank.Two)};

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Animal"] + handsToCheck["Animal"], new List<string>() { "Animal", "Animal" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void CompleteAnimalGroup() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Animal, Tile.Rank.One),
                new Tile(Tile.Suit.Animal, Tile.Rank.Two),
                new Tile(Tile.Suit.Animal, Tile.Rank.Three),
                new Tile(Tile.Suit.Animal, Tile.Rank.Four)};

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Complete Animal Group"], new List<string>() { "Complete Animal Group" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void CompleteSeasonGroup() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Season, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Season, Tile.Rank.Three),
                new Tile(Tile.Suit.Season, Tile.Rank.Four)};

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Complete Season Group"], new List<string>() { "Complete Season Group" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void CompleteFlowerGroup() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Flower, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Three),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four)};

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Complete Flower Group"], new List<string>() { "Complete Flower Group" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void RobbingTheEighth() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Flower, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Three),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four),
                new Tile(Tile.Suit.Season, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Season, Tile.Rank.Three)};

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Four)};

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Robbing the Eighth" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void AllFlowersAndSeasons() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Flower, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Three),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four),
                new Tile(Tile.Suit.Season, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Season, Tile.Rank.Three),
                new Tile(Tile.Suit.Season, Tile.Rank.Four)};

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "All Flowers and Seasons" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager , discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PlayerWindCombo() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Player Wind Combo"], new List<string>() { "Player Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PrevailingWindCombo() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.EAST;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Prevailing Wind Combo"], new List<string>() { "Prevailing Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PlayerAndPrevailingWindCombo() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.EAST;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Player Wind Combo"] + handsToCheck["Prevailing Wind Combo"], new List<string>() { "Player Wind Combo", "Prevailing Wind Combo" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void Dragon() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.EAST;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Dragon"] + handsToCheck["Dragon"], new List<string>() { "Dragon_One", "Dragon_Two" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);        }

        [Test]
        public void FullyConcealed() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { };
            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fully Concealed"], new List<string>() { "Fully Concealed" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void Triplets() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { 
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two) 
                } 
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Dragon, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Triplets"], new List<string>() { "Triplets" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void HalfFlush() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
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


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Dragon, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Half Flush"], new List<string>() { "Half Flush" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullFlush() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven) };


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Full Flush"], new List<string>() { "Full Flush" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequence() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Full Sequence"], new List<string>() { "Full Sequence" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequence_67899() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                new Tile(Tile.Suit.Dot, Tile.Rank.Three),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                new Tile(Tile.Suit.Dot, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven),
                new Tile(Tile.Suit.Character, Tile.Rank.Eight),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Nine);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Full Sequence"], new List<string>() { "Full Sequence" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequence_34567() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven),
                new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
                new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Three)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Five);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Full Sequence"], new List<string>() { "Full Sequence" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequenceFail_PlayerWind() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequenceFail_PrevailingWind() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.NORTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.EAST;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequenceFail_Dragon() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.EAST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequenceFail_WaitingForEye() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six), 
                new Tile(Tile.Suit.Dot, Tile.Rank.Seven)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Dot, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequenceFail_WaitingForEdgeTile_One() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Dot, Tile.Rank.Seven),
                new Tile(Tile.Suit.Dot, Tile.Rank.Seven)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Three);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequenceFail_WaitingForEdgeTile_Two() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Eight),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Dot, Tile.Rank.Seven),
                new Tile(Tile.Suit.Dot, Tile.Rank.Seven)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullSequenceFail_WaitingForMiddleTile() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Dot, Tile.Rank.Seven),
                new Tile(Tile.Suit.Dot, Tile.Rank.Seven)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Eight);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void LesserSequence() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.Two)};

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Bonus Tile Match Seat Wind"] + handsToCheck["Lesser Sequence"], new List<string>() { "Bonus Tile Match Seat Wind", "Lesser Sequence" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void MixedTerminals() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Nine)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Mixed Terminals"], new List<string>() { "Mixed Terminals" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PureTerminals() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Nine)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Pure Terminals" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void AllHonour() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "All Honour" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void HiddenTreasure() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
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

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {  };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Hidden Treasure" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullFlushTriplets() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine)};

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { 
                new List<Tile>(){
                    new Tile(Tile.Suit.Character, Tile.Rank.Eight),
                    new Tile(Tile.Suit.Character, Tile.Rank.Eight),
                    new Tile(Tile.Suit.Character, Tile.Rank.Eight)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Nine);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Full Flush Triplets" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullFlushFullSequence() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine)};

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Full Flush Full Sequence" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FullFlushLesserSequence() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine)};

            tilesManager.bonusTiles = new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.One)};

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.One),
                    new Tile(Tile.Suit.Character, Tile.Rank.Two),
                    new Tile(Tile.Suit.Character, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Seven);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Full Flush Lesser Sequence"], new List<string>() { "Full Flush Lesser Sequence" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void NineGates() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Seven)};

            tilesManager.bonusTiles = new List<Tile>() { new Tile(Tile.Suit.Season, Tile.Rank.One) };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                    new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                    new Tile(Tile.Suit.Character, Tile.Rank.Nine)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Eight);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Nine Gates" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FourLesserBlessings() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Four),
                new Tile(Tile.Suit.Wind, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                new Tile(Tile.Suit.Dot, Tile.Rank.Three)};

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Player Wind Combo"] + handsToCheck["Four Lesser Blessings"], new List<string>() { "Player Wind Combo", "Four Lesser Blessings" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void FourGreatBlessings() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Four),
                new Tile(Tile.Suit.Wind, Tile.Rank.Four),
                new Tile(Tile.Suit.Wind, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two) };

            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Four Great Blessings" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void PureGreenSuit() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Three) };


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Dragon, Tile.Rank.Three);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Pure Green Suit"], new List<string>() { "Pure Green Suit" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void ThreeLesserScholars() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
                    new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
                    new Tile(Tile.Suit.Dragon, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Three Lesser Scholars"], new List<string>() { "Three Lesser Scholars" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void ThreeGreatScholars() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
                    new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
                    new Tile(Tile.Suit.Dragon, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Three Great Scholars" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void EighteenArhats() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.One)}, 
                new List<Tile>() {
                    new Tile(Tile.Suit.Character, Tile.Rank.Three),
                    new Tile(Tile.Suit.Character, Tile.Rank.Three),
                    new Tile(Tile.Suit.Character, Tile.Rank.Three),
                    new Tile(Tile.Suit.Character, Tile.Rank.Three)},
                new List<Tile>() {
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.Three)},
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                    new Tile(Tile.Suit.Wind, Tile.Rank.Three)}
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Eighteen Arhats" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void ThirteenWonders() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Dot, Tile.Rank.One),
                new Tile(Tile.Suit.Dot, Tile.Rank.Nine),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                new Tile(Tile.Suit.Wind, Tile.Rank.Four),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Three),};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() { };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Thirteen Wonders" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void WinningOnReplacementTileForFlower() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One) }
            };

            playerManager.seatWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 2;
            playerManager.numberOfKong = 0;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Winning on Replacement Tile for Flower"] + handsToCheck["Winning on Replacement Tile for Flower"], new List<string>() { "Winning on Replacement Tile for Flower", "Winning on Replacement Tile for Flower" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void WinningOnReplacementTileForKong() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One)}
            };

            playerManager.seatWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 1;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Winning on Replacement Tile for Kong"], new List<string>() { "Winning on Replacement Tile for Kong"});
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void KongOnKong() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One)},
                new List<Tile>() {
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                    new Tile(Tile.Suit.Bamboo, Tile.Rank.One)}
            };

            playerManager.seatWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 2;

            discardTile = null;
            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Fan Limit"], new List<string>() { "Kong on Kong" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void RobbingTheKong() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One)}
            };

            playerManager.seatWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Four);
            discardTile.kongType = 2;

            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Robbing the Kong"], new List<string>() { "Robbing the Kong" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void WinningOnLastAvailableTile() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One)}
            };

            playerManager.seatWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = null;

            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 15;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Winning on Last Available Tile"], new List<string>() { "Winning on Last Available Tile" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void WinningOnLastAvailableTileFail() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Dot, Tile.Rank.Five),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One)};

            tilesManager.bonusTiles = new List<Tile>() { };
            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One),
                    new Tile(Tile.Suit.Wind, Tile.Rank.One)}
            };

            playerManager.seatWind = PlayerManager.Wind.WEST;
            playerManager.numberOfReplacementTiles = 1;
            playerManager.numberOfKong = 0;

            discardTile = null;

            discardPlayerWind = PlayerManager.Wind.SOUTH;
            prevailingWind = PlayerManager.Wind.SOUTH;
            numberOfTilesLeft = 15;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>();

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Winning on Replacement Tile for Flower"], new List<string>() { "Winning on Replacement Tile for Flower" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void TwoWinningCombos() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One)};


            tilesManager.bonusTiles = new List<Tile>() { };

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (handsToCheck["Full Sequence"], new List<string>() { "Full Sequence" });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

        [Test]
        public void OneFanNonWinning() {
            playerManager = new PlayerManager();
            tilesManager = new TilesManager();
            tilesManager.hand = new List<Tile>() {
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Two),
                new Tile(Tile.Suit.Character, Tile.Rank.Three),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two)};


            tilesManager.bonusTiles = new List<Tile>() { 
                new Tile(Tile.Suit.Animal, Tile.Rank.One)};

            tilesManager.comboTiles = new List<List<Tile>>() {
                new List<Tile>() {
                    new Tile(Tile.Suit.Dot, Tile.Rank.One),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Two),
                    new Tile(Tile.Suit.Dot, Tile.Rank.Three)
                }
            };

            playerManager.seatWind = PlayerManager.Wind.SOUTH;
            playerManager.numberOfReplacementTiles = 0;
            playerManager.numberOfKong = 0;

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.One);
            discardPlayerWind = PlayerManager.Wind.WEST;
            prevailingWind = PlayerManager.Wind.NORTH;
            numberOfTilesLeft = 45;
            turn = 10;
            allPlayersOpenTiles = new List<Tile>() { };

            (int expectedFan, List<string> expectedWinningCombos) = (0, new List<string>() { });
            (int actualFan, List<string> actualWinningCombos) = fanCalculator.CalculateFan(playerManager, tilesManager, discardTile, discardPlayerWind, prevailingWind, numberOfTilesLeft, turn, allPlayersOpenTiles);
            Assert.AreEqual(expectedWinningCombos, actualWinningCombos);
            Assert.AreEqual(expectedFan, actualFan);
        }

    }
}
