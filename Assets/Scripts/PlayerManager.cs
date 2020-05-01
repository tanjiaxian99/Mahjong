using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks {
    #region Private Fields

    private int score;
    public enum Wind {
        EAST,
        SOUTH,
        WEST,
        NORTH
    }
    private Wind playerWind { get; set; }

    #endregion

    #region MonoBehaviour Callbacks

    void Awake() {

    }
    #endregion

    #region Private Methods

    
    #endregion
}
