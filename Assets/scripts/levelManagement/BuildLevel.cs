using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuildLevel : MonoBehaviour
{
    [SerializeField] private GameObject star;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject Furnace;
    [SerializeField] private GameObject blueCell;
    [SerializeField] private GameObject redCell;
    [SerializeField] private GameObject blue;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject DoubleDoor;

    private const int length = 12;

    private void Awake() {
        List<Vector3> doors = new List<Vector3>();
        FieldData _fieldData = JsonUtility.FromJson<FieldData>(File.ReadAllText(Application.streamingAssetsPath + $"/Levels/level{LoadLevel.LevelNumber}.json"));
        for(int x = 0; x < length; x++){
            for(int y = 0; y < length; y++){
                char Symbol =_fieldData.Field[x][y];
                GameProcess.Cells[x,y] = new Cell(Symbol, x, y);
                GameObject newObject;
                switch(Symbol){
                    case CellTypes.WallLocator:
                        newObject = Instantiate(wall, new Vector3(2*x-11, 1.25f, 2*y-11), Quaternion.identity);
                        newObject.name = $"{Symbol}({x}, {y})";
                        break;
                    case CellTypes.FurnaceLocator:
                        newObject = Instantiate(Furnace, new Vector3(2*x-11, 0.5f, 2*y-11), Quaternion.identity);
                        newObject.name = $"{Symbol}({x}, {y})";
                        break;
                    case CellTypes.BoxLocator:
                        newObject = Instantiate(box, new Vector3(2*x-11, 1.25f, 2*y-11), Quaternion.identity);
                        newObject.name = $"{Symbol}({x}, {y})";
                        break;
                    case CellTypes.BlueCellLocator:
                        newObject = Instantiate(blueCell, new Vector3(2*x-11, 0.5f, 2*y-11), Quaternion.identity);
                        newObject.name = $"{Symbol}({x}, {y})";
                        break;
                    case CellTypes.RedCellLocator:
                        newObject = Instantiate(redCell, new Vector3(2*x-11, 0.5f, 2*y-11), Quaternion.identity);
                        newObject.name = $"{Symbol}({x}, {y})";
                        break;
                    case CellTypes.BlueLocator:
                        newObject = Instantiate(blue, new Vector3(2*x-11, 0.5f, 2*y-11), Quaternion.Euler(0,180,0));
                        Character.Blue = newObject.GetComponent<Blue>();
                        break;
                    case CellTypes.RedLocator:
                        newObject = Instantiate(red, new Vector3(2*x-11, 0.5f, 2*y-11), Quaternion.identity);
                        Character.Red = newObject.GetComponent<Red>();
                        break;
                    case CellTypes.DoorLocator:
                        doors.Add(new Vector3(2*x-11, 1.25f, 2*y-11));
                        break;
                    case CellTypes.StarLocator:
                        newObject = Instantiate(star, new Vector3(2*x-11, 0.5f, 2*y-11), Quaternion.identity);
                        newObject.name = $"{Symbol}({x}, {y})";
                        GameProcess.Cells[x,y].CellContains = CellTypes.EmptyLocator;
                        break;
                }
            }
        }
        Character.DoorLocker = Instantiate(DoubleDoor, (doors[0] + doors[1]) / 2, Quaternion.identity).GetComponent<Animator>();//double door setter
    }
}

[System.Serializable] public class FieldData {
    public List<String> Field;
}