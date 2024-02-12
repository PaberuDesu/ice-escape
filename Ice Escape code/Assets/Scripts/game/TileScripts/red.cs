using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class red : Character
{
    private const int turn = 0;
    private void Update() {
        if (GameLogic.turn == turn) {
            gameLogic.NobodyTurn();
            ClearCells();
            Vector2 direction = FindPath();
            if (direction == Vector2.zero) {
                gameLogic.ChangeTurn();
                Debug.Log("No way error");
                return;
            }
            int X = (int) direction.x;
            int Y = (int) direction.y;
            Move(X,Y);
        }
    }

    public void ControlTheDoors() {
        int AccessLevel = CharacterMinAccessLevel();
        if (AccessLevel == 0 && GridManager.DoorsTile.isOpened) {
            GridManager.DoorsTile.Close();
            return;
        }
        if (AccessLevel > 0 && !GridManager.DoorsTile.isOpened) {
            GridManager.DoorsTile.Open();
            return;
        }
    }

    private Vector2 FindPath(){
        bool _isBlueReachable = false;
        Tile StartTile = GridManager.Blue;
        _isBlueReachable = StartTile._canRedStepOn;
        Queue<Tile> queue = new Queue<Tile>();
        if(_isBlueReachable){
            queue.Enqueue(StartTile);
            StartTile.visited = true;
            while (queue.Count > 0){
                Queue<Tile> QueueSaved = queue;
                queue = new Queue<Tile>();
                foreach (Tile currentTile in QueueSaved){
                    foreach (Tile neighbour in currentTile.GetNeighbours()){
                        if (!neighbour.visited){
                            queue.Enqueue(neighbour);
                            neighbour.visited = true;
                            if (neighbour is red){
                                return new Vector2(currentTile.x - x, currentTile.y - y);
                            }
                        }
                    }
                }
            }
        }
        if (GridManager.grid[x+1,y]._canRedStepOn) return Vector2.right;
        if (GridManager.grid[x-1,y]._canRedStepOn) return Vector2.left;
        if (GridManager.grid[x,y+1]._canRedStepOn) return Vector2.up;
        if (GridManager.grid[x,y-1]._canRedStepOn) return Vector2.down;
        return Vector2.zero;
    }

    public void ClearCells(){
        for (int x = 1; x < 11; x++){
            for (int y = 1; y < 11; y++){
                GridManager.grid[x,y].visited = false;
            }
        }
    }

//checks for level stars achieve below

    public bool isNextToPermanent() {
        if (_isItPermanentIce(GridManager.grid[x+1,y])) return true;
        if (_isItPermanentIce(GridManager.grid[x-1,y])) return true;
        if (_isItPermanentIce(GridManager.grid[x,y+1])) return true;
        if (_isItPermanentIce(GridManager.grid[x,y-1])) return true;
        return false;
    }

    public bool isNextToWall() {
        return x == 0 || y == 0  || x == GridManager.length - 1 || y == GridManager.length - 1;
    }

    private bool _isItPermanentIce(Tile tile) {
        return (tile is Ice && !tile._isSelfDestroyable);
    }
}