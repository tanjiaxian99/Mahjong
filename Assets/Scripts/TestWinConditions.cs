using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWinConditions : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        List<Tile> hand = new List<Tile>();
        WinCombos testWin = new WinCombos();

        //// Nine Gates
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Five));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Six));

        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
        //hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
        
        //testWin.CheckWin(hand);
        //hand.Clear();

        // 2 ways of winning
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));

        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));

        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));

        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));

        testWin.CheckWin(hand);
        hand.Clear();
    }

    // Update is called once per frame
    void Update() {

    }
}
