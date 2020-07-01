using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public static class InitializeRound {

    /// <summary>
    /// Called at the start of every game (when PunTurnManager.Turn == 0) by MasterClient
    /// </summary>
    public static IEnumerator InitializeGame(GameManager gameManager, int numberOfPlayers, string type = "New Game") {
        if (type == "New Game") {
            // At this point, Start hasn't been called yet. Wait a frame before proceeding with the Coroutine
            yield return null;
            AssignSeatWind(numberOfPlayers);
            // DEBUG
            PropertiesManager.SetPrevailingWind(PlayerManager.Wind.EAST);
            DeterminePlayOrder(numberOfPlayers);
            ScreenViewAdjustment();
            SetInitialPoints();

        } else if (type == "New Round") {
            Debug.Log("Initializing New Round");
            yield return new WaitForSeconds(0.5f);
            NewSeatWind();
        }

        GenerateTiles();
        PropertiesManager.SetTurnNumber(0);
        PropertiesManager.SetTouchTiles(true);
        // Delay for WallTileListPropKey and PlayerWindPropKey related custom properties to update
        yield return new WaitForSeconds(1.5f);
        DistributeTiles();
        HiddenPayouts();
        yield return new WaitForSeconds(0.5f);
        PropertiesManager.StartTurn();
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.StartCoroutine(InitialInitialization());
        // Ensures InitialLocalInstantiation is called before OnPlayerTurn
        yield return new WaitForSeconds(0.8f);
        StartGame();
    }


    /// <summary>
    /// Called by MasterClient to assign a wind to each player
    /// </summary>
    public static void AssignSeatWind(int numberOfPlayers) {
        DictManager.Instance.windsAllocation = new Dictionary<int, int>();

        List<PlayerManager.Wind> winds = ((PlayerManager.Wind[])Enum.GetValues(typeof(PlayerManager.Wind))).ToList();
        PlayerManager.Wind playerWind;

        foreach (Player player in PhotonNetwork.PlayerList) {
            int randomIndex = 0;
            if (numberOfPlayers > 1) {
                randomIndex = GameManager.RandomNumber(winds.Count());
            }
            
            playerWind = winds[randomIndex];
            winds.Remove(winds[randomIndex]);
            DictManager.Instance.windsAllocation.Add(player.ActorNumber, (int)playerWind);
        }
        PropertiesManager.SetWindAllocation(DictManager.Instance.windsAllocation);
        Debug.LogFormat("The 4 winds have been assigned to each player");
    } 


    /// <summary>
    /// Update the room's custom properties with the play order. Play order starts from East Wind and ends at South.
    /// </summary>
    public static void DeterminePlayOrder(int numberOfPlayers) {
        // TODO: An array containing the nicknames might be sufficient.
        Player[] playOrder = new Player[numberOfPlayers];

        foreach (int actorNumber in DictManager.Instance.windsAllocation.Keys) {
            // The values of windsDict are PlayerManager.Wind types, which are ordered from East:0 to North:4
            // The integer value of windsDict themselves is the order of play
            int index = DictManager.Instance.windsAllocation[actorNumber];
            playOrder[index] = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        }
        PropertiesManager.SetInitialPlayerOrder(playOrder);
        PropertiesManager.SetPlayOrder(playOrder);
    }


    /// <summary>
    /// Inform all players to adjust their camera and gameTable
    /// </summary>
    public static void ScreenViewAdjustment() {
        EventsManager.EventScreenViewAdjustment();
    }


    /// <summary>
    /// Inform all players to initialize their points
    /// </summary>
    public static void SetInitialPoints() {
        EventsManager.EventInitialPoints();
    }


    /// <summary>
    /// Called by MasterClient to assign a seat wind to each player based on the predetermined play order.
    /// </summary>
    /// <param name="numberOfPlayers"></param>
    public static void NewSeatWind() {
        DictManager.Instance.windsAllocation = new Dictionary<int, int>();

        Player[] playOrder = PropertiesManager.GetPlayOrder();
        for (int i = 0; i < playOrder.Length; i++) {
            DictManager.Instance.windsAllocation.Add(playOrder[i].ActorNumber, i);
        }

        PropertiesManager.SetWindAllocation(DictManager.Instance.windsAllocation);
        Debug.LogFormat("The 4 winds have been assigned to each player");
    }


    /// <summary>
    /// Create 4 copies of each tile, giving 148 tiles
    /// </summary>
    public static void GenerateTiles() {
        List<Tile> tiles = new List<Tile>();

        foreach (Tile.Suit suit in Enum.GetValues(typeof(Tile.Suit))) {

            switch (suit) {
                // Generate the tiles for Character, Dot and Bamboo suits
                case Tile.Suit.Character:
                case Tile.Suit.Dot:
                case Tile.Suit.Bamboo:
                    foreach (Tile.Rank rank in Enum.GetValues(typeof(Tile.Rank))) {
                        for (int i = 0; i < 4; i++) {
                            tiles.Add(new Tile(suit, rank));
                        }
                    }
                    break;

                // Generate the tiles for Wind suit
                case Tile.Suit.Wind:
                    foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(4)) {
                        for (int i = 0; i < 4; i++) {
                            tiles.Add(new Tile(suit, rank));
                        }
                    }
                    break;

                // Generate the tiles for Dragon suit
                case Tile.Suit.Dragon:
                    foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(3)) {
                        for (int i = 0; i < 4; i++) {
                            tiles.Add(new Tile(suit, rank));
                        }
                    }
                    break;

                // Generate the tiles for Season, Flower and Animal suits. Only generates 1 tile for each rank.
                case Tile.Suit.Season:
                case Tile.Suit.Flower:
                case Tile.Suit.Animal:
                    foreach (Tile.Rank rank in ((Tile.Rank[])Enum.GetValues(typeof(Tile.Rank))).Take(4)) {
                        tiles.Add(new Tile(suit, rank));
                    }
                    break;
            }
        }

        if (tiles.Count != 148) {
            Debug.LogErrorFormat("{0} tiles have been created instead of 148", tiles.Count);
        }

        // DEBUG
        tiles = new List<Tile>() {
            new Tile(Tile.Suit.Character, Tile.Rank.One),
            new Tile(Tile.Suit.Character, Tile.Rank.Three),
            new Tile(Tile.Suit.Character, Tile.Rank.Three),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
            new Tile(Tile.Suit.Character, Tile.Rank.One),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
            new Tile(Tile.Suit.Character, Tile.Rank.One),
            new Tile(Tile.Suit.Dragon, Tile.Rank.Three),
            new Tile(Tile.Suit.Flower, Tile.Rank.Four),

            new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Seven),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.One),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Two),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Three),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Four),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Five),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Six),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Seven),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Eight),
            new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine),
        };

        // Add to Room Custom Properties
        PropertiesManager.SetWallTileList(tiles);
    }


    /// <summary>
    /// Distribute tiles to each player depending on the wind seat. The player with the East Wind receives 14 tiles
    /// while the rest receives 13
    /// </summary>
    public static void DistributeTiles() {
        List<Tile> tiles = PropertiesManager.GetWallTileList();

        // DEBUG
        foreach (Player player in PhotonNetwork.PlayerList) {
            if ((PlayerManager.Wind)DictManager.Instance.windsAllocation[player.ActorNumber] == PlayerManager.Wind.EAST) {
                List<Tile> playerTiles = new List<Tile>();

                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Three));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Three));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Three));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));

                EventsManager.EventDistributeTiles(player, playerTiles);

            } else if ((PlayerManager.Wind)DictManager.Instance.windsAllocation[player.ActorNumber] == PlayerManager.Wind.SOUTH) {
                List<Tile> playerTiles = new List<Tile>();

                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Three));

                EventsManager.EventDistributeTiles(player, playerTiles);

            } else if ((PlayerManager.Wind)DictManager.Instance.windsAllocation[player.ActorNumber] == PlayerManager.Wind.WEST) {
                List<Tile> playerTiles = new List<Tile>();

                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Three));

                EventsManager.EventDistributeTiles(player, playerTiles);

            } else if ((PlayerManager.Wind)DictManager.Instance.windsAllocation[player.ActorNumber] == PlayerManager.Wind.NORTH) {
                List<Tile> playerTiles = new List<Tile>();

                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Four));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Seven));
                playerTiles.Add(new Tile(Tile.Suit.Character, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Seven));
                playerTiles.Add(new Tile(Tile.Suit.Dot, Tile.Rank.Nine));
                playerTiles.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.One));
                playerTiles.Add(new Tile(Tile.Suit.Dragon, Tile.Rank.Two));
                playerTiles.Add(new Tile(Tile.Suit.Bamboo, Tile.Rank.Five));

                EventsManager.EventDistributeTiles(player, playerTiles);
            }
        }


        //foreach (Player player in PhotonNetwork.PlayerList) {
        //    List<Tile> playerTiles = new List<Tile>();

        //    for (int i = 0; i < 14; i++) {
        //        // Choose a tile randomly from the complete tiles list and add it to the player's tiles
        //        int randomIndex = GameManager.RandomNumber(tiles.Count());
        //        playerTiles.Add(tiles[randomIndex]);
        //        tiles.Remove(tiles[randomIndex]);

        //        // Don't give the 14th tile if the player is not the East Wind
        //        if (i == 12 && (PlayerManager.Wind)DictManager.Instance.windsAllocation[player.ActorNumber] != PlayerManager.Wind.EAST) {
        //            break;
        //        }
        //    }

        //    EventsManager.EventDistributeTiles(player, playerTiles);
        //}

        PropertiesManager.SetWallTileList(tiles);
        Debug.Log("The tiles from the wall have been distributed");
    }


    /// <summary>
    /// Inform all players to check for Hidden Payouts.
    /// </summary>
    public static void HiddenPayouts() {
        EventsManager.EventHiddenPayouts();
    }


    /// <summary>
    /// Convert the bonus tiles to normal tiles and instantiate the local player's hand and open tiles. Done in playOrder sequence.
    /// </summary>
    public static IEnumerator InitialInitialization() {
        foreach (Player player in PropertiesManager.GetPlayOrder()) {
            yield return new WaitForSeconds(0.2f);
            EventsManager.EventPlayerInitialization(player);
        }
    }


    /// <summary>
    /// Start the game with the East Wind
    /// </summary>
    public static void StartGame() {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }
        Player[] playOrder = PropertiesManager.GetPlayOrder();

        Player firstPlayer = playOrder[0];
        EventsManager.EventPlayerTurn(firstPlayer);
    }    
}
