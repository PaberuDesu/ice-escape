using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{
    public static Blue Blue;
    public static Red Red;
    protected static Camera _mainCamera{get; private set;}
    protected static EndGame _end{get; private set;}
    protected static Text WhoMovesIndicator{get; private set;}
    protected static GameObject Ice{get; private set;}
    public static Animator DoorLocker;

    public UnityEvent MoveEvent;
    protected Color CharacterTeam;
    protected char CharacterIdentifier;
    protected char[] EnabledRoads;

    protected bool _isStarted{get; private set;} = false;

    public static Vector3 PositionToLeave = Vector3.zero;

    protected Animator characterAnimator;

    protected PointCounter _pointCounter;

    protected static bool _wasBlueMoveLast;

    public void _awake(){
        _mainCamera = Camera.main;
        GameObject parent = Camera.main.gameObject;
        _end = parent.GetComponent<EndGame>();
        Ice = parent.GetComponent<IcePreinstance>().Ice;
        _pointCounter = GameObject.Find("Points").GetComponent<PointCounter>();
        WhoMovesIndicator = GameObject.Find("WhoMovesIndicator").GetComponent<Text>();
        characterAnimator = this.gameObject.GetComponent<Animator>();
        _isStarted = true;
        _wasBlueMoveLast = false;
    }

    public void Move(Vector3 MovePosition){
        int RotationID = DefineAngleToTurn(MovePosition);
        characterAnimator.SetInteger("TurnIdentifier", RotationID);
    }

    public IEnumerator EndMove(){
        characterAnimator.SetInteger("TurnIdentifier", 0);
        yield return new WaitForSeconds(0.05f);
        this.transform.position = new Vector3(RoundPosition(transform.position.x),transform.position.y,RoundPosition(transform.position.z));
        this.transform.eulerAngles = new Vector3(0, Mathf.Round(this.transform.rotation.eulerAngles.y/90)*90, 0);
        if (EndGame.reasonOfEnd == "")
            PostDeed();
    }

    private int RoundPosition(float num){return (int) Mathf.Round((num-1)/2)*2+1;}

    private int DefineAngleToTurn(Vector3 MovePosition){
        int currentAngle = (int) (this.transform.rotation.eulerAngles.y/90)%4;
        Vector3 MovePath = MovePosition - this.transform.position;
        int pathLength = 2;
        int x = (int) MovePath.x, z = (int) MovePath.z;

        int targetAngle = 0;
        if(z == pathLength) targetAngle = 1;
        else if(x == pathLength) targetAngle = 2;
        else if(z == -pathLength) targetAngle = 3;
        else if(x == -pathLength) targetAngle = 4;
        int AngleToRotate = targetAngle - currentAngle;
        if(AngleToRotate <= 0) AngleToRotate += 4;
        return AngleToRotate;
    }

    public virtual void RunAway(Vector3 MovePosition){;}

    public abstract void PreDeed();

    public abstract void Deed();

    public abstract void PostDeed();

    public void CreateIce(int PreviousCoordX, int PreviousCoordZ){
        if(GameProcess.Cells[PreviousCoordX,PreviousCoordZ] != CellTypes.FurnaceLocator){
            GameProcess.Cells[PreviousCoordX, PreviousCoordZ].CellContains = CellTypes.IceLocator;
            GameObject newIce = Instantiate(Ice, this.transform.position, Quaternion.identity);
            IceController iceScript = newIce.GetComponent<IceController>();
            MoveEvent.AddListener(iceScript.Addition);
            iceScript.Coordinate(PreviousCoordX, PreviousCoordZ);
            GameProcess.Cells[PreviousCoordX, PreviousCoordZ].WasFreezed = true;
        }else{
            GameProcess.Cells[PreviousCoordX, PreviousCoordZ].CellContains = CellTypes.EmptyLocator;
        }
    }

    public bool CanMoveTo(int x, int y){
        char RoadDefiner = GameProcess.Cells[x,y].CellContains;
        char UnderRoadDefiner = GameProcess.Cells[x,y].UnmovableBlock;
        bool _isRoadClear = false;
        bool _isUnderRoadClear = false;
        foreach (char status in EnabledRoads){
            if(RoadDefiner == status)
                _isRoadClear = true;
            if(UnderRoadDefiner == status)
                _isUnderRoadClear = true;
        }
        if(_isRoadClear && _isUnderRoadClear) return true;
        return (RoadDefiner == CellTypes.DoorLocator && CharacterIdentifier == CellTypes.BlueLocator && DoorLocker.GetBool("_isEnemyCatched"));
    }

    public bool CanMove(){
        int CoordX = (int) Mathf.Floor(this.transform.position.x / 2 + 6);
        int CoordZ = (int) Mathf.Floor(this.transform.position.z / 2 + 6);
        bool upPathClear = (CoordZ < 11)
            ? CanMoveTo(CoordX, CoordZ+1)
            : false;
        if (upPathClear) return true;
        bool downPathClear = (CoordZ > 0)
            ? CanMoveTo(CoordX, CoordZ-1)
            : false;
        if (downPathClear) return true;
        bool rightPathClear = (CoordX < 11)
            ? CanMoveTo(CoordX+1, CoordZ)
            : false;
        if (rightPathClear) return true;
        bool leftPathClear = (CoordX > 0)
            ? CanMoveTo(CoordX-1, CoordZ)
            : false;
        if (leftPathClear) return true;
        return false;
    }

    public bool CanMoveInFuture(){
        int CoordX = (int) Mathf.Floor(this.transform.position.x / 2 + 6);
        int CoordZ = (int) Mathf.Floor(this.transform.position.z / 2 + 6);
        bool upPathClear = (CoordZ < 11)
            ? _isBreakable(GameProcess.Cells[CoordX, CoordZ + 1])
            : false;
        if (upPathClear) return true;
        bool downPathClear = (CoordZ > 0)
            ? _isBreakable(GameProcess.Cells[CoordX, CoordZ - 1])
            : false;
        if (downPathClear) return true;
        bool rightPathClear = (CoordX < 11)
            ? _isBreakable(GameProcess.Cells[CoordX + 1, CoordZ])
            : false;
        if (rightPathClear) return true;
        bool leftPathClear = (CoordX > 0)
            ? _isBreakable(GameProcess.Cells[CoordX - 1, CoordZ])
            : false;
        if (leftPathClear) return true;
        return false;
    }

    private bool _isBreakable(Cell cell){
        if(CharacterIdentifier == CellTypes.BlueLocator && cell == CellTypes.BlueCellLocator) return true;
        if(CharacterIdentifier == CellTypes.RedLocator && cell == CellTypes.RedCellLocator) return true;
        if(cell == CellTypes.FurnaceLocator) return true;
        return !cell._isObstacleSolid();
    }
}