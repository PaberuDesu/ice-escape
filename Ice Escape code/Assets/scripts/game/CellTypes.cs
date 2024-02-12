using UnityEngine;
using System.Collections.Generic;

public class cellTypes: MonoBehaviour
{
    [SerializeField] private Tile blueCell;
    [SerializeField] private Tile redCell;
    [SerializeField] private Tile star;
    [SerializeField] private Tile wall;
    [SerializeField] private Tile box;
    [SerializeField] private Tile furnace;
    [SerializeField] private Tile doubleDoor;
    [SerializeField] private Tile ice;
    [SerializeField] private Tile blue;
    [SerializeField] private Tile red;

    private void Awake() {
        CellTypes.blueCell = blueCell;
        CellTypes.redCell = redCell;
        CellTypes.star = star;
        CellTypes.wall = wall;
        CellTypes.box = box;
        CellTypes.furnace = furnace;
        CellTypes.doubleDoor = doubleDoor;
        CellTypes.ice = ice;
        CellTypes.blue = blue;
        CellTypes.red = red;
        CellTypes.Tiles = new Dictionary<char, Tile>() {
            {'v', blueCell},
            {'n', redCell},
            {'s', star},
            {'w', wall},
            {'c', box},
            {'f', furnace},
            {'d', doubleDoor},
            {'b', blue},
            {'r', red}
        };
    }
}

public static class CellTypes
{
    public static Tile blueCell;
    public static Tile redCell;
    public static Tile star;
    public static Tile wall;
    public static Tile box;
    public static Tile furnace;
    public static Tile doubleDoor;
    public static Tile ice;
    public static Tile blue;
    public static Tile red;

    public static Dictionary<char, Tile> Tiles = new Dictionary<char, Tile>() {
        {'v', blueCell},
        {'n', redCell},
        {'s', star},
        {'w', wall},
        {'c', box},
        {'f', furnace},
        {'d', doubleDoor},
        {'b', blue},
        {'r', red}
    };

    public static Tile character(bool isBlue) {
        return isBlue ? blue : red;
    }

    public const char EmptyLocator = '0';
    public const char BlueCellLocator = 'v';
    public const char RedCellLocator = 'n';
    public const char StarLocator = 's';
    public const char WallLocator = 'w';
    public const char BoxLocator = 'c';
    public const char FurnaceLocator = 'f';
    public const char DoorLocator = 'd';
    public const char IceLocator = 'i';
    public const char PermanentIceLocator = 'p';
    public const char BlueLocator = 'b';
    public const char RedLocator = 'r';
}