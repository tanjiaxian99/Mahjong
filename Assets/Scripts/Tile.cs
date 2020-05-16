using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class for all tiles. Rank sequence is as follows:
/// Character, Dot, Bamboo, Season and Flower: the number on the tile;
/// Wind: East, South, West, North;
/// Dragon: Red, White, Green;
/// Animal: Cat, Rat, Rooster, Centipede.
/// </summary>
public class Tile : IEquatable<Tile> {
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

    /// <summary>
    /// The Id of each tile is a 2-digit number: the first digit represents the suit while the second represents the rank.
    /// </summary>
    public byte Id {
        get {
            return (byte) ((int)suit * 10 + (int)rank);
        } set {
            suit = (Suit) (value / 10);
            rank = (Rank) (value % 10);
        }
    }


    /// <summary>
    /// Default constructor. Calls Initialize, which is shared with the secondary constructor.
    /// </summary>
    public Tile(Suit suit, Rank rank) {
        this.Initialize(suit, rank);
    }


    /// <summary>
    /// Secondary constructor. Calls Initialize, which is shared with the default constructor.
    /// </summary>
    public Tile(string name) {
        string[] part = name.Split('_');
        Enum.TryParse(part[0], out Suit suit);
        Enum.TryParse(part[1], out Rank rank);
        this.Initialize(suit, rank);
    }


    /// <summary>
    /// Initialize a new Tile object. Called by both constructors.
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="rank"></param>
    public void Initialize(Suit suit, Rank rank) {
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


    public bool IsBonus() {
        return suit == Suit.Season || suit == Suit.Flower || suit == Suit.Animal;
    }


    /// <summary>
    /// Returns a list of chow combinations for the tile, contingent on the player's hand.
    /// </summary>
    public List<object[]> ChowCombinations(List<Tile> hand) {
        List<object[]> combinations = new List<object[]>();
        object[] combo;

        // Only Character, Dot and Bamboo suits can chow
        if (this.suit != Suit.Character || this.suit != Suit.Dot || this.suit != Suit.Bamboo) {
            return combinations;
        }

        // Chow can only happen between tiles of the same suit
        List<Tile> sameSuit = new List<Tile>();
        foreach (Tile tile in hand) {
            if (tile.suit == this.suit) {
                sameSuit.Add(tile);
            }
        }

        // Can't chow if there is only one tile from the same suit
        if (sameSuit.Count <= 1) {
            return combinations;
        }

        Tile tileMinusTwo = new Tile(this.suit, this.rank - 2);
        Tile tileMinusOne = new Tile(this.suit, this.rank - 1);
        Tile tilePlusOne = new Tile(this.suit, this.rank + 1);
        Tile tilePlusTwo = new Tile(this.suit, this.rank + 2);

        // The tile forms a sequence as the first tile
        if (hand.Contains(tilePlusOne) && hand.Contains(tilePlusTwo)) {
            combo = new object[] { this, tilePlusOne, tilePlusTwo, "First"};
            combinations.Add(combo);
        }

        // The tile forms a sequence as the middle tile
        if (hand.Contains(tileMinusOne) && hand.Contains(tilePlusOne)) {
            combo = new object[] { tileMinusOne, this, tilePlusOne, "Second" };
            combinations.Add(combo);
        }

        // The tile forms a sequence as the last tile
        if (hand.Contains(tileMinusTwo) && hand.Contains(tileMinusOne)) {
            combo = new object[] { tileMinusTwo, tileMinusOne, this, "third" };
            combinations.Add(combo);
        }

        return combinations;
    }


    /// <summary>
    /// Returns true if the discard tile can be ponged, contingent on the player's hand.
    /// </summary>
    public bool CanPong(List<Tile> hand) {
        int equalCount = 0;

        foreach (Tile tile in hand) {
            if (this.Equals(tile)) {
                equalCount++;
            }
        }

        return equalCount == 2;
    }


    /// <summary>
    /// Returns true if the drawn/discard tile can be konged
    /// </summary>
    public bool CanKong(List<Tile> tileList) {
        int equalCount = 0;

        foreach (Tile tile in tileList) {
            if (this.Equals(tile)) {
                equalCount++;
            }
        }

        return equalCount == 3;
    }


    public override string ToString() {
        return suit + "_" + rank;
    }


    /// <summary>
    /// If 2 tiles have the same suit and rank, they are equal, regardless of reference.
    /// </summary>
    public bool Equals(Tile tile) {
        return suit == tile.suit &&
               rank == tile.rank;
    }
    

    public override int GetHashCode() {
        int hashCode = 39855015;
        hashCode = hashCode * -1521134295 + suit.GetHashCode();
        hashCode = hashCode * -1521134295 + rank.GetHashCode();
        return hashCode;
    }

    
}
