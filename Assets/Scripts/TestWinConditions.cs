using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWinConditions {
    // Start is called before the first frame update
    void Start() {
        List<Tile> hand = new List<Tile>();
        for (int i = 0; i < 3; i++) {
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
            hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
        }
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
        hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.Eight));

        WinConditions testWin = new WinConditions();
        testWin.CheckWin(hand);
    }

    // Update is called once per frame
    void Update() {

    }
}
