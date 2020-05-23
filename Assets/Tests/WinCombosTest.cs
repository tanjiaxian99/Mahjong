using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Tests {

    public class WinCombosTest {

        [Test]
        public void TestNineGates() {
            List<Tile> hand = new List<Tile>();
            WinCombos testWin = new WinCombos();

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
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
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
