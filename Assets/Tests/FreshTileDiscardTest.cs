using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {

    public class FreshTileDiscardTest {
        List<Tile> discardTiles;
        List<Tile> allPlayersOpenTiles;
        Tile discardTile;

        [Test]
        public void TileInDiscardPool() {
            discardTiles = new List<Tile>() { 
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Bamboo, Tile.Rank.Three)};

            allPlayersOpenTiles = new List<Tile>() { };
            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Four);

            bool expected = FreshTileDiscard.IsFreshTile(discardTiles, allPlayersOpenTiles, discardTile);
            bool actual = false;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TileInChowCombo() {
            discardTiles = new List<Tile>() { };

            allPlayersOpenTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),};

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Five);

            bool expected = FreshTileDiscard.IsFreshTile(discardTiles, allPlayersOpenTiles, discardTile);
            bool actual = true;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TileInPongCombo() {
            discardTiles = new List<Tile>() { };

            allPlayersOpenTiles = new List<Tile>() {
                new Tile(Tile.Suit.Flower, Tile.Rank.One),
                new Tile(Tile.Suit.Season, Tile.Rank.Three),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.One),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Four),
                new Tile(Tile.Suit.Character, Tile.Rank.Five),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Six),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),
                new Tile(Tile.Suit.Character, Tile.Rank.Nine),};

            discardTile = new Tile(Tile.Suit.Character, Tile.Rank.Six);

            bool expected = FreshTileDiscard.IsFreshTile(discardTiles, allPlayersOpenTiles, discardTile);
            bool actual = false;
            Assert.AreEqual(expected, actual);
        }
    }
}
