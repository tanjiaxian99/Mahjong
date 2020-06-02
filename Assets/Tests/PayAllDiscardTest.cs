using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {

    public class PayAllDiscardTest {
        Dictionary<string, int> handsToCheck;
        PayAllDiscard payAllDiscard;

        List<Tile> openTiles;
        PlayerManager.Wind playerWind;
        PlayerManager.Wind prevailingWind;

        public PayAllDiscardTest() {
            handsToCheck = new Dictionary<string, int>();

            handsToCheck.Add("Fan Limit", 5);

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

            handsToCheck.Add("Dragon Tile Set Pay All", 1);
            handsToCheck.Add("Wind Tile Set Pay All", 1);
            handsToCheck.Add("Point Limit Pay All", 1);
            handsToCheck.Add("Full Flush Pay All", 1);
            handsToCheck.Add("Pure Terminals Pay All", 1);

            payAllDiscard = new PayAllDiscard(handsToCheck);
        }

        [Test]
        public void DragonTileSetTest() {
            openTiles = new List<Tile>() {
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
            };

            playerWind = PlayerManager.Wind.EAST;
            prevailingWind = PlayerManager.Wind.WEST;

            List<Tile> expectedHighRiskTiles = new List<Tile>() { new Tile(Tile.Suit.Dragon, Tile.Rank.Three) };
            List<Tile> actualHighRiskTiles = payAllDiscard.PayAllCheck(openTiles, playerWind, prevailingWind);
            Assert.AreEqual(expectedHighRiskTiles, actualHighRiskTiles);
        }

        [Test]
        public void WindTileSetTest() {
            openTiles = new List<Tile>() {
                new Tile(Tile.Suit.Season, Tile.Rank.Two),
                new Tile(Tile.Suit.Flower, Tile.Rank.Four),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.One),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Two),
                new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                new Tile(Tile.Suit.Wind, Tile.Rank.Three),
                new Tile(Tile.Suit.Wind, Tile.Rank.Three),
            };

            playerWind = PlayerManager.Wind.EAST;
            prevailingWind = PlayerManager.Wind.WEST;

            List<Tile> expectedHighRiskTiles = new List<Tile>() {
                new Tile(Tile.Suit.Wind, Tile.Rank.Four),
                new Tile(Tile.Suit.Dragon, Tile.Rank.One),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Three) };
            List<Tile> actualHighRiskTiles = payAllDiscard.PayAllCheck(openTiles, playerWind, prevailingWind);
            Assert.AreEqual(expectedHighRiskTiles, actualHighRiskTiles);
        }

        [Test]
        public void PointLimitTest_ThreeFan() {
            openTiles = new List<Tile>() {
                new Tile(Tile.Suit.Season, Tile.Rank.One),
                new Tile(Tile.Suit.Flower, Tile.Rank.Three),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Dragon, Tile.Rank.Two),
                new Tile(Tile.Suit.Animal, Tile.Rank.One)
            };

            playerWind = PlayerManager.Wind.EAST;
            prevailingWind = PlayerManager.Wind.EAST;

            List<Tile> expectedHighRiskTiles = new List<Tile>() {
                new Tile(Tile.Suit.Wind, Tile.Rank.One)
            };
            List<Tile> actualHighRiskTiles = payAllDiscard.PayAllCheck(openTiles, playerWind, prevailingWind);
            Assert.AreEqual(expectedHighRiskTiles, actualHighRiskTiles);
        }
    }
}
