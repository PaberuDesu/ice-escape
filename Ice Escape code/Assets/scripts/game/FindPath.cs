using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour
{
    public int Find(){
        bool _isBlueReachable = false;//means that exists at least one cell from where red could reach blue
        Cell StartCell = GameProcess.Cells[1,1];
        for (int x = 0; x < 12; x++){
            for (int y = 0; y < 12; y++){
                Cell CurrentCell = GameProcess.Cells[x,y];
                if(CurrentCell.CellContains == CellTypes.BlueLocator){
                    StartCell = CurrentCell;
                    _isBlueReachable = Character.Red.CanMoveTo(CurrentCell.x,CurrentCell.y);
                }
            }
        }
        Queue<Cell> queue = new Queue<Cell>();
        if(_isBlueReachable){
            queue.Enqueue(StartCell);
            StartCell.visited = true;
            while (queue.Count > 0){
                Queue<Cell> QueueSaved = queue;
                queue = new Queue<Cell>();
                foreach (Cell currentCell in QueueSaved){
                    foreach (Cell neighbor in currentCell.GetNeighbours()){
                        if (!neighbor.visited){
                            queue.Enqueue(neighbor);
                            neighbor.visited = true;
                            if (neighbor == CellTypes.RedLocator){
                                return neighbor.direction;
                            }
                        }
                    }
                }
            }
        }
        int RedCoordX = (int) Mathf.Floor(Character.Red.transform.position.x / 2 + 6);
        int RedCoordZ = (int) Mathf.Floor(Character.Red.transform.position.z / 2 + 6);
        Cell RedCell = GameProcess.Cells[RedCoordX, RedCoordZ];
        if (Character.Red.CanMoveTo(RedCoordX+1, RedCoordZ)) RedCell.direction = 2;
        else if (Character.Red.CanMoveTo(RedCoordX-1, RedCoordZ)) RedCell.direction = 4;
        else if (Character.Red.CanMoveTo(RedCoordX, RedCoordZ+1)) RedCell.direction = 1;
        else if (Character.Red.CanMoveTo(RedCoordX, RedCoordZ-1)) RedCell.direction = 3;
        return RedCell.direction;
    }

    public void ClearCells(){
        for (int x = 0; x < 12; x++){
            for (int y = 0; y < 12; y++){
                Cell CurrentCell = GameProcess.Cells[x,y];
                CurrentCell.direction = 0;
                CurrentCell.visited = false;
            }
        }
    }
}