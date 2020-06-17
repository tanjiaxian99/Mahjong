using Photon.Pun;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks {

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

    public int points = 200;

    public string payForAll = "";

    public Tile sacredDiscard;

    public int fanTotal;

    public List<string> winningCombos;
    
}


