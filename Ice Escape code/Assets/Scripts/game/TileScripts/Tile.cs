using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool _isPlayerDestroyable;
    public bool _isSelfDestroyable;
    public bool _canBlueStepOn;
    public bool _canRedStepOn;
    public bool isWarm;
    public bool isEmpty;

    public bool isDestroyable{get => _isPlayerDestroyable || _isSelfDestroyable;}

    public Tile lowerTile = null;
    public Animator animator = null;

    public int x,y;
    public bool visited = false;

    public bool _isCharacter;

    public void placeTo(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public bool canCharacterStepOn(bool isBlue) {
        if (this is blue) return GridManager.Blue.lowerTile.canCharacterStepOn(isBlue);
        if (this is red) return GridManager.Red.lowerTile.canCharacterStepOn(isBlue);
        return isBlue ? _canBlueStepOn : _canRedStepOn;
    }

    public List<Tile> GetNeighbours() {
        List<Tile> tiles = new List<Tile>();
        if (x < 10 && (GridManager.grid[x+1,y]._canRedStepOn || GridManager.grid[x+1,y] is red)) tiles.Add(GridManager.grid[x+1,y]);
        if (x > 1 && (GridManager.grid[x-1,y]._canRedStepOn || GridManager.grid[x-1,y] is red)) tiles.Add(GridManager.grid[x-1,y]);
        if (y < 10 && (GridManager.grid[x,y+1]._canRedStepOn || GridManager.grid[x,y+1] is red)) tiles.Add(GridManager.grid[x,y+1]);
        if (y > 1 && (GridManager.grid[x,y-1]._canRedStepOn || GridManager.grid[x,y-1] is red)) tiles.Add(GridManager.grid[x,y-1]);
        return tiles;
    }
}