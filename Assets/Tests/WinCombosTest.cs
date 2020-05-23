﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TestTools;


namespace Tests {

    public class WinCombosTest {
        WinCombos testWin = new WinCombos();

        [Test]
        public void TestNineGatesOne() {
            List<Tile> hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Chow", "Eye", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);
            
            Assert.AreEqual(expected, actual, "Failed NineGatesOne Test");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestSuiteWinComboTypesWithEnumeratorPasses() {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
