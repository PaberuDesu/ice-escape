using System.Collections.Generic;
using UnityEngine;

public class GameProcess : MonoBehaviour
{
    public static Cell[,] Cells = new Cell[12,12];
}

public class Cell{
    public char CellContains;
    public char UnmovableBlock;
    public int x;
    public int y;
    public bool visited = false;
    public int direction = 0;
    public bool WasFreezed = false;
    public Transform Director;

    public static bool operator==(Cell cell, char state){
        return cell.CellContains == state || cell.UnmovableBlock == state;
    }
    public static bool operator==(char state, Cell cell){
        return cell.CellContains == state || cell.UnmovableBlock == state;
    }
    public static bool operator!=(Cell cell, char state){
        return !(cell.CellContains == state || cell.UnmovableBlock == state);
    }
    public static bool operator!=(char state, Cell cell){
        return !(cell.CellContains == state || cell.UnmovableBlock == state);
    }
    public override bool Equals(object state){
        if (state is Cell cell) return this.CellContains == cell.CellContains || this.UnmovableBlock == cell.UnmovableBlock;
        else return false;
    }
    public override int GetHashCode(){
        return CellContains.GetHashCode();
    }
    public Cell(char Symbol, int x, int y){
        if(_isObstacleSolid(Symbol) || Symbol == CellTypes.FurnaceLocator){
            UnmovableBlock = Symbol;
            CellContains = CellTypes.EmptyLocator;
        }
        else{
            UnmovableBlock = CellTypes.EmptyLocator;
            CellContains = Symbol;
        }
        this.x = x;
        this.y = y;
    }

    public bool _isAbsolutelyEmpty(){return CellContains == CellTypes.EmptyLocator && UnmovableBlock == CellTypes.EmptyLocator;}

    public bool _isObstacleDestroyable(){
        foreach (char obstacle in CellTypes.DestroyableObstacles){
            if(this == obstacle) return true;
        }
        return false;
    }

    public int DestroyObstacle(){
        WasFreezed = false;
        foreach (char obstacle in CellTypes.DestroyableObstacles){
            if(UnmovableBlock == obstacle){
                UnmovableBlock = CellTypes.EmptyLocator;
                Bonuses.destroyableObject = GameObject.Find($"{obstacle}({x}, {y})");
                return 0;
            }
            if(CellContains == obstacle){
                CellContains = CellTypes.EmptyLocator;
                Bonuses.destroyableObject = GameObject.Find($"{obstacle}({x}, {y})");
                return 0;
            }
        }
        return 0;
    }

    public bool _isObstacleSolid(){return UnmovableBlock != CellTypes.EmptyLocator;}

    private bool _isObstacleSolid(char Symbol){
        foreach (char obstacle in CellTypes.SolidObstacles){
            if(Symbol == obstacle) return true;
        }
        return false;
    }

    public List<Cell> GetNeighbours(){
        List<Cell> Neighbours = new List<Cell>();
        Cell newNeighbour;
        int NeighbourX, NeighbourY;
        if(x < 11){
            NeighbourX = x+1;
            NeighbourY = y;
            newNeighbour = GameProcess.Cells[NeighbourX,NeighbourY];
            if(Character.Red.CanMoveTo(NeighbourX,NeighbourY) || newNeighbour.CellContains == CellTypes.RedLocator){
                if(!newNeighbour.visited)
                    newNeighbour.direction = 4;
                Neighbours.Add(newNeighbour);
            }
        }
        if(x > 0){
            NeighbourX = x-1;
            NeighbourY = y;
            newNeighbour = GameProcess.Cells[NeighbourX,NeighbourY];
            if(Character.Red.CanMoveTo(NeighbourX,NeighbourY) || newNeighbour.CellContains == CellTypes.RedLocator){
                if(!newNeighbour.visited)
                    newNeighbour.direction = 2;
                Neighbours.Add(newNeighbour);
            }
        }
        if(y < 11){
            NeighbourX = x;
            NeighbourY = y+1;
            newNeighbour = GameProcess.Cells[NeighbourX,NeighbourY];
            if(Character.Red.CanMoveTo(NeighbourX,NeighbourY) || newNeighbour.CellContains == CellTypes.RedLocator){
                if(!newNeighbour.visited)
                    newNeighbour.direction = 3;
                Neighbours.Add(newNeighbour);
            }
        }
        if(y > 0){
            NeighbourX = x;
            NeighbourY = y-1;
            newNeighbour = GameProcess.Cells[NeighbourX,NeighbourY];
            if(Character.Red.CanMoveTo(NeighbourX,NeighbourY) || newNeighbour.CellContains == CellTypes.RedLocator){
                if(!newNeighbour.visited)
                    newNeighbour.direction = 1;
                Neighbours.Add(newNeighbour);
            }
        }
        return Neighbours;
    }

    public bool _isEnabledForRed(){
        foreach (char cell in CellTypes.EnabledForRed){
            if(this.CellContains == cell)
                return true;
        }
        return false;
    }
}