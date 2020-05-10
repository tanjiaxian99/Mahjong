using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks {
    #region Private Fields

    private int score;
    public enum Wind {
        EAST,
        NORTH,
        WEST,
        SOUTH
    }

    public Wind PlayerWind { get; set; }

    public List<Tile> hand { get; set; } = new List<Tile>() ;

    public List<Tile> bonusTiles { get; set; } = new List<Tile>();

    public List<Tile> comboTiles { get; set; } = new List<Tile>();

    public List<Tile> openTiles { get; set; } = new List<Tile>();

    public bool myTurn = false;

    #endregion

    #region MonoBehaviour Callbacks

    void Awake() {
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Update the tiles which are face up on the GameTable. It consists of bonus tiles and combo tiles.
    /// </summary>
    public void UpdateOpenTiles() {
        openTiles.Clear();
        foreach (Tile tile in bonusTiles) {
            openTiles.Add(tile);
        }
        
        foreach (Tile tile in comboTiles) {
            openTiles.Add
        }
    }
    
    #endregion
}
