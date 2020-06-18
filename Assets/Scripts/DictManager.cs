using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictManager : MonoBehaviour {

    #region Tile Sprites

    [SerializeField]
    private Sprite Character_One_Sprite;

    [SerializeField]
    private Sprite Character_Two_Sprite;

    [SerializeField]
    private Sprite Character_Three_Sprite;

    [SerializeField]
    private Sprite Character_Four_Sprite;

    [SerializeField]
    private Sprite Character_Five_Sprite;

    [SerializeField]
    private Sprite Character_Six_Sprite;

    [SerializeField]
    private Sprite Character_Seven_Sprite;

    [SerializeField]
    private Sprite Character_Eight_Sprite;

    [SerializeField]
    private Sprite Character_Nine_Sprite;

    [SerializeField]
    private Sprite Dot_One_Sprite;

    [SerializeField]
    private Sprite Dot_Two_Sprite;

    [SerializeField]
    private Sprite Dot_Three_Sprite;

    [SerializeField]
    private Sprite Dot_Four_Sprite;

    [SerializeField]
    private Sprite Dot_Five_Sprite;

    [SerializeField]
    private Sprite Dot_Six_Sprite;

    [SerializeField]
    private Sprite Dot_Seven_Sprite;

    [SerializeField]
    private Sprite Dot_Eight_Sprite;

    [SerializeField]
    private Sprite Dot_Nine_Sprite;

    [SerializeField]
    private Sprite Bamboo_One_Sprite;

    [SerializeField]
    private Sprite Bamboo_Two_Sprite;

    [SerializeField]
    private Sprite Bamboo_Three_Sprite;

    [SerializeField]
    private Sprite Bamboo_Four_Sprite;

    [SerializeField]
    private Sprite Bamboo_Five_Sprite;

    [SerializeField]
    private Sprite Bamboo_Six_Sprite;

    [SerializeField]
    private Sprite Bamboo_Seven_Sprite;

    [SerializeField]
    private Sprite Bamboo_Eight_Sprite;

    [SerializeField]
    private Sprite Bamboo_Nine_Sprite;

    [SerializeField]
    private Sprite Wind_One_Sprite;

    [SerializeField]
    private Sprite Wind_Two_Sprite;

    [SerializeField]
    private Sprite Wind_Three_Sprite;

    [SerializeField]
    private Sprite Wind_Four_Sprite;

    [SerializeField]
    private Sprite Dragon_One_Sprite;

    [SerializeField]
    private Sprite Dragon_Two_Sprite;

    [SerializeField]
    private Sprite Dragon_Three_Sprite;

    [SerializeField]
    private Sprite Season_One_Sprite;

    [SerializeField]
    private Sprite Season_Two_Sprite;

    [SerializeField]
    private Sprite Season_Three_Sprite;

    [SerializeField]
    private Sprite Season_Four_Sprite;

    [SerializeField]
    private Sprite Flower_One_Sprite;

    [SerializeField]
    private Sprite Flower_Two_Sprite;

    [SerializeField]
    private Sprite Flower_Three_Sprite;

    [SerializeField]
    private Sprite Flower_Four_Sprite;

    [SerializeField]
    private Sprite Animal_One_Sprite;

    [SerializeField]
    private Sprite Animal_Two_Sprite;

    [SerializeField]
    private Sprite Animal_Three_Sprite;

    [SerializeField]
    private Sprite Animal_Four_Sprite;

    #endregion

    /// <summary>
    /// Dictionary containing Tile objects and their respective sprite
    /// </summary>
    public Dictionary<Tile, Sprite> spritesDict;

    /// <summary>
    /// Dictionary containing Tile objects and their respective winds
    /// </summary>
    public Dictionary<Tile, PlayerManager.Wind> tileToWindDict;

    private static DictManager _instance;

    public static DictManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start() {
        this.InitializeSpritesDict();
        this.InitializeTileToWindDict();
    }

    /// <summary>
    /// Initialize the spritesDict with Tile objects and their respective sprites
    /// </summary>
    private void InitializeSpritesDict() {
        spritesDict = new Dictionary<Tile, Sprite>();

        spritesDict.Add(new Tile("Character_One"), Character_One_Sprite);
        spritesDict.Add(new Tile("Character_Two"), Character_Two_Sprite);
        spritesDict.Add(new Tile("Character_Three"), Character_Three_Sprite);
        spritesDict.Add(new Tile("Character_Four"), Character_Four_Sprite);
        spritesDict.Add(new Tile("Character_Five"), Character_Five_Sprite);
        spritesDict.Add(new Tile("Character_Six"), Character_Six_Sprite);
        spritesDict.Add(new Tile("Character_Seven"), Character_Seven_Sprite);
        spritesDict.Add(new Tile("Character_Eight"), Character_Eight_Sprite);
        spritesDict.Add(new Tile("Character_Nine"), Character_Nine_Sprite);

        spritesDict.Add(new Tile("Dot_One"), Dot_One_Sprite);
        spritesDict.Add(new Tile("Dot_Two"), Dot_Two_Sprite);
        spritesDict.Add(new Tile("Dot_Three"), Dot_Three_Sprite);
        spritesDict.Add(new Tile("Dot_Four"), Dot_Four_Sprite);
        spritesDict.Add(new Tile("Dot_Five"), Dot_Five_Sprite);
        spritesDict.Add(new Tile("Dot_Six"), Dot_Six_Sprite);
        spritesDict.Add(new Tile("Dot_Seven"), Dot_Seven_Sprite);
        spritesDict.Add(new Tile("Dot_Eight"), Dot_Eight_Sprite);
        spritesDict.Add(new Tile("Dot_Nine"), Dot_Nine_Sprite);

        spritesDict.Add(new Tile("Bamboo_One"), Bamboo_One_Sprite);
        spritesDict.Add(new Tile("Bamboo_Two"), Bamboo_Two_Sprite);
        spritesDict.Add(new Tile("Bamboo_Three"), Bamboo_Three_Sprite);
        spritesDict.Add(new Tile("Bamboo_Four"), Bamboo_Four_Sprite);
        spritesDict.Add(new Tile("Bamboo_Five"), Bamboo_Five_Sprite);
        spritesDict.Add(new Tile("Bamboo_Six"), Bamboo_Six_Sprite);
        spritesDict.Add(new Tile("Bamboo_Seven"), Bamboo_Seven_Sprite);
        spritesDict.Add(new Tile("Bamboo_Eight"), Bamboo_Eight_Sprite);
        spritesDict.Add(new Tile("Bamboo_Nine"), Bamboo_Nine_Sprite);

        spritesDict.Add(new Tile("Wind_One"), Wind_One_Sprite);
        spritesDict.Add(new Tile("Wind_Two"), Wind_Two_Sprite);
        spritesDict.Add(new Tile("Wind_Three"), Wind_Three_Sprite);
        spritesDict.Add(new Tile("Wind_Four"), Wind_Four_Sprite);

        spritesDict.Add(new Tile("Dragon_One"), Dragon_One_Sprite);
        spritesDict.Add(new Tile("Dragon_Two"), Dragon_Two_Sprite);
        spritesDict.Add(new Tile("Dragon_Three"), Dragon_Three_Sprite);

        spritesDict.Add(new Tile("Season_One"), Season_One_Sprite);
        spritesDict.Add(new Tile("Season_Two"), Season_Two_Sprite);
        spritesDict.Add(new Tile("Season_Three"), Season_Three_Sprite);
        spritesDict.Add(new Tile("Season_Four"), Season_Four_Sprite);

        spritesDict.Add(new Tile("Flower_One"), Flower_One_Sprite);
        spritesDict.Add(new Tile("Flower_Two"), Flower_Two_Sprite);
        spritesDict.Add(new Tile("Flower_Three"), Flower_Three_Sprite);
        spritesDict.Add(new Tile("Flower_Four"), Flower_Four_Sprite);

        spritesDict.Add(new Tile("Animal_One"), Animal_One_Sprite);
        spritesDict.Add(new Tile("Animal_Two"), Animal_Two_Sprite);
        spritesDict.Add(new Tile("Animal_Three"), Animal_Three_Sprite);
        spritesDict.Add(new Tile("Animal_Four"), Animal_Four_Sprite);
    }


    /// <summary>
    /// Initialize the tileToWindDict with Tile objects and their respective winds
    /// </summary>
    private void InitializeTileToWindDict() {
        tileToWindDict = new Dictionary<Tile, PlayerManager.Wind>();

        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.One), PlayerManager.Wind.EAST);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Two), PlayerManager.Wind.SOUTH);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Three), PlayerManager.Wind.WEST);
        tileToWindDict.Add(new Tile(Tile.Suit.Wind, Tile.Rank.Four), PlayerManager.Wind.NORTH);

        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.One), PlayerManager.Wind.EAST);
        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Two), PlayerManager.Wind.SOUTH);
        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Three), PlayerManager.Wind.WEST);
        tileToWindDict.Add(new Tile(Tile.Suit.Season, Tile.Rank.Four), PlayerManager.Wind.NORTH);

        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.One), PlayerManager.Wind.EAST);
        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Two), PlayerManager.Wind.SOUTH);
        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Three), PlayerManager.Wind.WEST);
        tileToWindDict.Add(new Tile(Tile.Suit.Flower, Tile.Rank.Four), PlayerManager.Wind.NORTH);
    }
}
