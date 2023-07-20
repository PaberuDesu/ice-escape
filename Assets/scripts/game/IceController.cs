using UnityEngine;

public class IceController : MonoBehaviour
{
    private int moveCounter = 0;
    private const int moveLimit = 10;

    private int x, y;

    private Animator IceChange;

    private void Start() {IceChange = GetComponent<Animator>();}

    public void Coordinate(int x, int y){
        this.x = x; this.y = y;
        this.gameObject.name = $"{CellTypes.IceLocator}({x}, {y})";
    }

    public void Addition(){//count till melt down
        moveCounter++;
        if (moveCounter >= moveLimit){
            GameProcess.Cells[x, y].CellContains = CellTypes.EmptyLocator;
            IceChange.SetBool("_isDestroy", true);
            Character.Blue.MoveEvent.RemoveListener(Addition);
            Character.Blue.MoveEvent.AddListener(Permanency);
            Character.Red.MoveEvent.RemoveListener(Addition);
            Character.Red.MoveEvent.AddListener(Permanency);
        }
    }

    public void Permanency(){//make a permanent ice in cell where character was twice permanent
        if (Character.PositionToLeave == transform.position) {
            this.gameObject.name = $"{CellTypes.PermanentIceLocator}({x}, {y})";
            GameProcess.Cells[x, y].CellContains = CellTypes.EmptyLocator;
            GameProcess.Cells[x, y].UnmovableBlock = CellTypes.PermanentIceLocator;
            IceChange.SetBool("_isIceUndestructible", true);
            Character.Blue.MoveEvent.RemoveListener(Permanency);
            Character.Red.MoveEvent.RemoveListener(Permanency);
        }
    }
}