using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridManager : MonoBehaviour
{
    public static Tile[,] grid = new Tile[12,12];
    [SerializeField] private Tile emptyTile;
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private EndGame _end;
    public static EndGame end;
    public static int length = 12;
    public static Transform gridTransform;
    public static red Red;
    public static blue Blue;
    public static Doors DoorsTile;
    public static PointCounter pointCounter;

    private void Awake() {
        grid = new Tile[12,12];
        GameLogic.turn = 1;
        end = _end;
        pointCounter = GameObject.Find("Points").GetComponent<PointCounter>();
        gridTransform = transform;
        List<Vector3> doors = new List<Vector3>();
        FieldData _fieldData = JsonUtility.FromJson<FieldData>(File.ReadAllText(Application.streamingAssetsPath + $"/Levels/level{LoadLevel.LevelNumber}.json"));
        for (int x = 0; x < length; x++) {
            for (int y = 0; y < length; y++) {
                char Symbol =_fieldData.Field[x][y];
                if (Symbol == CellTypes.DoorLocator)
                    doors.Add(new Vector3(x, 0, y));
                else if (CellTypes.Tiles.ContainsKey(Symbol)) {
                    Tile spawnedTile = Instantiate(CellTypes.Tiles[Symbol], new Vector3(x,0,y), Quaternion.identity, this.transform);
                    spawnedTile.placeTo(x,y);
                    if (spawnedTile._isCharacter) {
                        spawnedTile.transform.position += Vector3.down * 0.5f;
                        if (Symbol == CellTypes.BlueLocator)
                            spawnedTile.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
                        ((Character)spawnedTile).gameLogic = gameLogic;
                        if (spawnedTile is blue)
                            Blue = (blue) spawnedTile;
                        else Red = (red) spawnedTile;
                    }
                    spawnedTile.name = $"Cell({x}, {y})";
                    grid[x,y] = spawnedTile;
                }
                else {
                    Tile spawnedTile = Instantiate(emptyTile, new Vector3(x,0,y), Quaternion.identity, this.transform);
                    spawnedTile.name = $"Empty({x}, {y})";
                    spawnedTile.placeTo(x,y);
                    grid[x,y] = spawnedTile;
                }
            }
        }
        DoorsTile = Instantiate((Doors) CellTypes.Tiles[CellTypes.DoorLocator], (doors[0] + doors[1]) / 2, Quaternion.identity);
        foreach(Vector3 door in doors)
            grid[(int) door.x, (int) door.z] = DoorsTile;
    }
}

[System.Serializable] public class FieldData {
    public List<String> Field;
}