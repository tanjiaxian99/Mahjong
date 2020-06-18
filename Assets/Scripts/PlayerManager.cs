using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public enum Wind {
        EAST,
        SOUTH,
        WEST,
        NORTH
    }

    public Wind seatWind { get; set; }

    public bool myTurn = false;

    public bool canTouchHandTiles = false;

    public int numberOfReplacementTiles { get; set; }

    public int numberOfKong { get; set; }

    public Tile sacredDiscard;

    public int points = 200;

    public string payForAll = "";

    public int fanTotal;

    public List<string> winningCombos;
    
}


