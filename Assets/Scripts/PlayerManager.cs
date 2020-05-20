using Photon.Pun;
using System.Collections.Generic;

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

    public List<List<Tile>> comboTiles { get; set; } = new List<List<Tile>>();

    public List<Tile> openTiles { get; set; } = new List<Tile>();

    public bool myTurn = false;

    public bool canTouchHandTiles = false;

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
        
        foreach (List<Tile> tilesList in comboTiles) {
            foreach (Tile tile in tilesList) {
                openTiles.Add(tile);
            }
        }
    }

    #endregion

}


