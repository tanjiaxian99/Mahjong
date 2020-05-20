using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TestTiles : MonoBehaviour {
    [SerializeField]
    private GameObject tile;

    public GameObject gameTable;

    [SerializeField]
    private GameObject ChowComboOne;

    [SerializeField]
    private GameObject ChowComboTwo;

    [SerializeField]
    private GameObject ChowComboThree;

    [SerializeField]
    private Sprite tileSprite;

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
        this.ShowChow();
        //this.TestChowCombinations();
    }


    // Update is called once per frame
    void Update() {

    }


    public void ShowChow() {
        Transform spritesPanel = ChowComboOne.transform.GetChild(0);
        for (int i = 0; i < 3; i++) {
            Transform imageTransform = spritesPanel.GetChild(i);
            Image image = imageTransform.GetComponent<Image>();
            image.sprite = tileSprite;
            image.color = new Color(1f, 1f, 0f);

            Debug.Log(image.sprite.name);

        }

        ChowComboOne.SetActive(true);

    }


    public void OnOk() {
        ChowComboOne.SetActive(false);
    }


    public void TestChowCombinations() {
        PlayerManager playerManager = new PlayerManager();

        playerManager.hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
        playerManager.hand.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
        playerManager.hand.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One));
        playerManager.hand.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One));
        Tile discardTile = new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine);

        //List<object[]> chowCombos = ChowPongKong.Instance.ChowCombinations(discardTile);

        //Debug.Log(chowCombos.Count);
        //foreach (object[] objArray in chowCombos) {

        //    foreach (object obj in objArray) {
        //        Debug.Log(obj);
        //    }
        //}

        //Tile newTile = new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine);
        //Tile anotherTile = new Tile(Tile.Suit.Animal, Tile.Rank.Eight);
        //Debug.LogError(anotherTile == null);
    }
}

