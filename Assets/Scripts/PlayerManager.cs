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

    #endregion

    #region MonoBehaviour Callbacks

    void Awake() {
    }
    #endregion

    #region Private Methods

    
    #endregion
}
