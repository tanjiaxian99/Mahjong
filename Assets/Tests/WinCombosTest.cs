﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TestTools;


namespace Tests {

    public class WinCombosTest {
        WinCombos testWin = new WinCombos();
        List<Tile> hand;

        [Test]
        public void OneUniqueSolutionOne() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Chow", "Chow", "Eye" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OneUniqueSolutionTwo() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Eye", "Pong", "Pong", "Pong" }};
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OneUniqueSolutionThree() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Eye", "Pong", "Pong", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TwoDifferentSolutionsOne() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() {
                new List<string>() { "Chow", "Eye", "Pong", "Pong", "Pong" },
                new List<string>() { "Chow", "Chow", "Chow", "Chow", "Eye" }
            };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TwoDifferentSolutionsTwo() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));

            List<List<string>> expected = new List<List<string>>() {
                new List<string>() { "Eye", "Pong", "Pong", "Pong", "Pong" },
                new List<string>() { "Chow", "Chow", "Chow", "Eye", "Pong" }
            };
            testWin.CheckWin(hand);

            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TwoDifferentSolutionsThree() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            List<List<string>> expected = new List<List<string>>() {
                new List<string>() { "Chow", "Eye", "Pong", "Pong", "Pong" },
                new List<string>() { "Chow", "Chow", "Chow", "Chow", "Eye" }
            };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TwoDifferentSolutionsFour() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));

            List<List<string>> expected = new List<List<string>>() {
                new List<string>() { "Eye", "Pong", "Pong", "Pong", "Pong" },
                new List<string>() { "Chow", "Chow", "Chow", "Eye", "Pong" }
            };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TwoSolutionsFail() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>();
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesOne() {
            hand = new List<Tile>();

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

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesTwo() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
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

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Eye", "Pong", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesThree() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

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

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesFour() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

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

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesFive() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Eye", "Pong", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesSix() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Chow", "Eye", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesSeven() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Chow", "Eye", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesEight() {
            hand = new List<Tile>();

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
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Eye", "Pong", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestNineGatesNine() {
            hand = new List<Tile>();

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

            List<List<string>> expected = new List<List<string>>() { new List<string>() { "Chow", "Chow", "Chow", "Eye", "Pong" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SameReferenceTileFail() {
            hand = new List<Tile>();

            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            Tile special = new Tile(Tile.Suit.Character, Tile.Rank.Eight);
            hand.Add(special);
            hand.Add(special);
            hand.Add(special);
            hand.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Five));
            hand.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Three));
            

            List<List<string>> expected = new List<List<string>>() { 
                new List<string>() {"Chow", "Chow", "Chow", "Eye", "Pong"},
                new List<string>() { "Chow", "Chow", "Chow", "Eye", "Eye" } };
            List<List<string>> actual = testWin.CheckWin(hand);

            Assert.AreEqual(expected, actual);
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
