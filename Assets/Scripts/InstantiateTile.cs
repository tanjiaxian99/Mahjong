using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTile : MonoBehaviour {
    [SerializeField]
    private GameObject tile;

    public GameObject gameTable;
    
    // Start is called before the first frame update
    void Start() {
        Camera camera = Camera.main;
        float tableHeight = 2f * camera.orthographicSize;
        float tableWidth = tableHeight * camera.aspect;

        // Scale the GameTable along z direction
        gameTable.transform.localScale = new Vector3(tableWidth, 1, tableHeight);

        // Instantiate a tile
        float xSep = 0.83f;
        float xPos = -xSep * 6;
        for (int i = 0; i < 13; i++) {
            // Distance between each tile is 0.82
            Instantiate(tile, new Vector3(xPos, 1f, -4.4f), Quaternion.Euler(270f, 180f, 0f));
            xPos += xSep;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
