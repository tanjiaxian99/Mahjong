using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChowManager : MonoBehaviour {

    #region Serialize Field

    [SerializeField]
    private GameObject ChowComboZero;

    [SerializeField]
    private GameObject ChowComboOne;

    [SerializeField]
    private GameObject ChowComboTwo;

    [SerializeField]
    private GameObject scriptManager;

    #endregion

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
    }


    /// <summary>
    /// Called when the player can chow
    /// </summary>
    public void ChowUI(List<object[]> chowCombos) {
        for (int i = 0; i < chowCombos.Count; i++) {

            // TODO: Might be better to implement a dictionary
            GameObject chowComboGameObject;
            if (i == 0) {
                chowComboGameObject = ChowComboZero;
            } else if (i == 1) {
                chowComboGameObject = ChowComboOne;
            } else {
                chowComboGameObject = ChowComboTwo;
            }

            Transform spritesPanel = chowComboGameObject.transform.GetChild(0);

            // Instantiate the tile sprites
            for (int j = 0; j < 3; j++) {
                object[] tileAndStringArray = chowCombos[i];
                Transform imageTransform = spritesPanel.GetChild(j);
                Image image = imageTransform.GetComponent<Image>();
                image.sprite = DictManager.Instance.spritesDict[(Tile)tileAndStringArray[j]];

                // The drawn/discarded tile is painted yellow. The other tiles are updated with white.
                if (j == 0 && ((string)tileAndStringArray[3]).Equals("First") || j == 1 && ((string)tileAndStringArray[3]).Equals("Second") || j == 2 && ((string)tileAndStringArray[3]).Equals("Third")) {
                    image.color = new Color(1f, 1f, 0f);
                } else {
                    image.color = new Color(1f, 1f, 1f);
                }

            }
            chowComboGameObject.SetActive(true);
        }

        // Disable the ability to interact with hand tiles
        playerManager.canTouchHandTiles = false;
    }


    /// <summary>
    /// Called when "Ok" is clicked for Chow Combo
    /// </summary>
    public void OnChowOk() {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        GameObject chowComboGameObject = button.transform.parent.parent.gameObject;
        object[] tileAndStringArray;

        ChowComboZero.SetActive(false);
        ChowComboOne.SetActive(false);
        ChowComboTwo.SetActive(false);

        // The UI panels are named "Chow Combo 0", "Chow Combo 1" and "Chow Combo 2", which corresponds directly to the index of the 
        // chowCombo list. This was set up in ChowUI.
        int index = (int)Char.GetNumericValue(chowComboGameObject.name[11]);
        tileAndStringArray = gameManager.chowTiles[index];

        Tile otherTile;
        Tile[] handTile = new Tile[2];

        // TODO: A better way of doing this?
        if (((string)tileAndStringArray[3]).Equals("First")) {
            otherTile = (Tile)tileAndStringArray[0];
            handTile[0] = (Tile)tileAndStringArray[1];
            handTile[1] = (Tile)tileAndStringArray[2];

        } else if (((string)tileAndStringArray[3]).Equals("Second")) {
            otherTile = (Tile)tileAndStringArray[1];
            handTile[0] = (Tile)tileAndStringArray[0];
            handTile[1] = (Tile)tileAndStringArray[2];

        } else if (((string)tileAndStringArray[3]).Equals("Third")) {
            otherTile = (Tile)tileAndStringArray[2];
            handTile[0] = (Tile)tileAndStringArray[0];
            handTile[1] = (Tile)tileAndStringArray[1];
        }


        // Update discard tile properties to indicate to all players to remove the latest discard tile
        PropertiesManager.UpdateDiscardTile(new Tuple<int, Tile, float>(-1, new Tile(0, 0), 0));

        // Update both the player's hand and the combo tiles list
        foreach (Tile tile in handTile) {
            tilesManager.hand.Remove(tile);
        }

        List<Tile> pongTiles = new List<Tile>();
        for (int i = 0; i < 3; i++) {
            pongTiles.Add((Tile)tileAndStringArray[i]);
        }
        tilesManager.comboTiles.Add(pongTiles);

        gameManager.InstantiateLocalHand();
        gameManager.InstantiateLocalOpenTiles();

        // Return the ability to interact with hand tiles
        playerManager.canTouchHandTiles = true;
    }


    /// <summary>
    /// Called when "Skip" button is clicked for Chow Combo
    /// </summary>
    public void OnChowSkip() {
        ChowComboZero.SetActive(false);
        ChowComboOne.SetActive(false);
        ChowComboTwo.SetActive(false);

        tilesManager.hand.Add(gameManager.DrawTile());
        gameManager.latestDiscardTile = null;
        gameManager.discardPlayer = null;

        gameManager.ConvertLocalBonusTiles();
        gameManager.InstantiateLocalHand();
        gameManager.InstantiateLocalOpenTiles();

        // Check to see if the player can win based on the drawn tile
        if (gameManager.CanWin()) {
            gameManager.WinUI();
            return;
        }

        // Check if the player can Kong the drawn tile
        if (tilesManager.ExposedKongTiles().Count != 0 || tilesManager.ConcealedKongTiles().Count != 0) {
            gameManager.KongUI(tilesManager.ExposedKongTiles().Concat(tilesManager.ConcealedKongTiles()).ToList());
            return;
        }

        // Return the ability to interact with hand tiles
        playerManager.canTouchHandTiles = true;
    }
    
}
