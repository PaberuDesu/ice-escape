using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Red : Character
{
    public FindPath path;
    Vector3[] Directions;

    private void Awake(){
        if(!_isStarted) _awake();
        CharacterTeam = Color.red;
        EnabledRoads = CellTypes.EnabledForRed;
        MoveEvent = new UnityEvent();
        CharacterIdentifier = CellTypes.RedLocator;
        GameObject parent = Camera.main.gameObject;
        path = parent.GetComponent<FindPath>();
        Directions = new Vector3[4] {Vector3.forward * 2, Vector3.right * 2, Vector3.back * 2, Vector3.left * 2};
    }

    public override void PreDeed(){
        if (CanMove())
            Deed();
        else if (!CanMoveInFuture())
            _end.GameOver(true, "Красный пойман");//End game: Blue wins. Red catched.
        else{
            MoveEvent.Invoke();
            PostDeed();
        }
    }

    public override void Deed(){
        WhoMovesIndicator.color = CharacterTeam;
        int PreviousCoordX = (int) Mathf.Floor(this.transform.position.x / 2 + 6);
        int PreviousCoordZ = (int) Mathf.Floor(this.transform.position.z / 2 + 6);

        Vector3 Direction = Vector3.zero;
        int DirectionID = path.Find();;
        if (DirectionID > 0 && DirectionID <= 5){
            Direction = Directions[DirectionID - 1];
            Vector3 HitPoint = this.transform.position + Direction;
            HitPoint.x = Mathf.Floor(HitPoint.x / 2) * 2 + 1;
            HitPoint.y = 0.5f;
            HitPoint.z = Mathf.Floor(HitPoint.z / 2) * 2 + 1;

            int CoordX = (int) Mathf.Floor(HitPoint.x / 2 + 6);
            int CoordZ = (int) Mathf.Floor(HitPoint.z / 2 + 6);

            if (GameProcess.Cells[CoordX, CoordZ] == CellTypes.BlueLocator) _end.GameOver(false, "Красный съел синего");
            GameProcess.Cells[CoordX, CoordZ].CellContains = CharacterIdentifier;
            PositionToLeave = this.transform.position;
        
            MoveEvent.Invoke();
            if (!GameProcess.Cells[PreviousCoordX, PreviousCoordZ].WasFreezed) CreateIce(PreviousCoordX, PreviousCoordZ);
            Move(HitPoint);
        }
        else{
            MoveEvent.Invoke();
            PostDeed();
        }
    }

    public override void PostDeed(){
        if (!CanMoveInFuture())_end.GameOver(true, "красный пойман");//End game: Blue wins. Red catched.
        else{
            DoorLocker.SetBool("_isEnemyCatched", !CanMove());
            _pointCounter.RecountPointsForRed();
            Character.Blue.PreDeed();
        }
        path.ClearCells();
    }

    public bool _isNextTo(char obstacleID){
        int CoordX = (int) Mathf.Floor(this.transform.position.x / 2 + 6);
        int CoordZ = (int) Mathf.Floor(this.transform.position.z / 2 + 6);
        bool upPathObstacle = (CoordZ < 11)
            ? GameProcess.Cells[CoordX, CoordZ + 1].CellContains == obstacleID
            : false;
        if (upPathObstacle) return true;
        bool downPathObstacle = (CoordZ > 0)
            ? GameProcess.Cells[CoordX, CoordZ - 1].CellContains == obstacleID
            : false;
        if (downPathObstacle) return true;
        bool rightPathObstacle = (CoordX < 11)
            ? GameProcess.Cells[CoordX + 1, CoordZ].CellContains == obstacleID
            : false;
        if (rightPathObstacle) return true;
        bool leftPathObstacle = (CoordX > 0)
            ? GameProcess.Cells[CoordX - 1, CoordZ].CellContains == obstacleID
            : false;
        if (leftPathObstacle) return true;
        return false;
    }
}