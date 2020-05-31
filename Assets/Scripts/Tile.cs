using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;


/// <summary>
/// Class for all tiles. Rank sequence is as follows:
/// Character, Dot, Bamboo, Season and Flower: the number on the tile;
/// Wind: East, South, West, North;
/// Dragon: Red, White, Green;
/// Animal: Cat, Rat, Rooster, Centipede.
/// </summary>
public class Tile : IEquatable<Tile> {
    public Suit? suit { get; set; }
    public Rank? rank { get; set; }

    /// <summary>
    /// Discard Kong: 1, Exposed Kong: 2, Concealed Kong: 3
    /// </summary>
    public byte kongType { get; set; } = 0;
    public bool isWinning { get; set; } = false;


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
    /// The Id consists of 2 bytes: the first represents the suit and rank, while the second represents the kongType.
    /// </summary>
    public byte[] Id {
        get {
            return new byte[2] { (byte)((int) suit * 10 + (int) rank), kongType };
        } set {
            suit = (Suit) (value[0] / 10);
            rank = (Rank) (value[0] % 10);
            kongType = value[1];
        }
    }


    /// <summary>
    /// Default constructor. Calls Initialize, which is shared with the secondary constructor.
    /// </summary>
    public Tile(Suit? suit, Rank? rank) {
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
    public void Initialize(Suit? suit, Rank? rank) {
        if (suit < Suit.Character || suit > Suit.Animal) {
            Debug.LogError("The tile has an invalid suit");
            return;
        }

        if (rank < Rank.One || rank > Rank.Nine) {
            //Debug.LogError("The tile's rank is less than one or greater than nine");
            return;
        }

        if (suit == Suit.Wind && rank > Rank.Four) {
            Debug.LogError("The Wind Tile has a greater rank than four");
            return;
        }

        if (suit == Suit.Dragon && rank > Rank.Three) {
            Debug.LogError("The Dragon Tile has a greater rank than three");
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


    /// <summary>
    /// Returns true if the tile is a bonus tile
    /// </summary>
    public bool IsBonus() {
        return suit == Suit.Season || suit == Suit.Flower || suit == Suit.Animal;
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
