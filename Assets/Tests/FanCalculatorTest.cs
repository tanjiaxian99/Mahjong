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
        
        #region Instantiate HandsToCheck Dictionary
        public void TestInitialize() {
            fanCalculator = new FanCalculator(handsToCheck);
            handsToCheck = new Dictionary<string, int>();

            handsToCheck.Add("Fan Limit", 50);

            handsToCheck.Add("Heavenly Hand", 10);
            handsToCheck.Add("Earthly Hand", 10);
            handsToCheck.Add("Humanly Hand", 10);

            handsToCheck.Add("Bonus Tile Match Seat Wind", 1);
            handsToCheck.Add("Animal", 1);
            handsToCheck.Add("Complete Season Group", 2);
            handsToCheck.Add("Complete Flower Group", 2);
            handsToCheck.Add("Robbing the Eigth", 10);
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
        }

        #endregion


        [Test]
        public void ZeroFanHand() {
            
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
