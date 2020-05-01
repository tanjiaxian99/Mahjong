using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks {
    #region Private Fields

    private int score;
    private enum Wind {
        EAST,
        SOUTH,
        WEST,
        NORTH
    }

    #endregion
}
