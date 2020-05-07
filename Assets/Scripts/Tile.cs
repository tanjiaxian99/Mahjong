using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class for all tiles. Rank sequence is as follows:
/// Character, Dot, Bamboo, Season and Flower: the number on the tile;
/// Wind: East, North, West, South;
/// Dragon: Red, White, Green;
/// Animal: Cat, Rat, Rooster, Centipede.
/// </summary>
public class Tile {
    public Suit suit { get; set; }
    public Rank rank { get; set; }

    public enum Suit {
        Character,
        Dot,
        Bamboo,
        Wind,
        Dragon,
        Season,
        Flower,
        Animal
    }

    public enum Rank {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
    }

    public byte Id {
        get {
            return (byte) ((int)suit * 10 + (int)rank);
        } set {
            suit = (Suit) (value / 10);
            rank = (Rank) (value % 10);
        }
    }

    // Raises error if rank does not belong in suit
    public Tile(Suit suit, Rank rank) {
        if (suit < Suit.Character || suit > Suit.Animal) {
            Debug.LogError("The tile has an invalid suit");
            return;
        }

        if (rank < Rank.One || rank > Rank.Nine) {
            Debug.LogError("The tile's rank is less than one or greater than nine");
            return;
        }

        if (suit == Suit.Wind && rank > Rank.Four) {
            Debug.LogError("The Wind Tile has a greater rank than four");
            return;
        }

        if (suit == Suit.Dragon && rank > Rank.Three) {
            Debug.LogError("The Dragon Tile has rank has a greater rank than three");
            return;
        }

        if (suit == Suit.Season && rank > Rank.Four) {
            Debug.LogError("The Season Tile has a greater rank than four");
            return;
        }

        if (suit == Suit.Flower && rank > Rank.Four) {
            Debug.LogError("The Flower Tile has a greater rank than four");
            return;
        }

        if (suit == Suit.Animal && rank > Rank.Four) {
            Debug.LogError("The Animal Tile has a greater rank than four");
            return;
        }

        this.suit = suit;
        this.rank = rank;
    }

    public override string ToString() {
        return suit + " " + rank;
    }
}
