using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Blue : Character
{
    private bool _isBlueMove = false;
    private Color ExtraMoveIndicator;

    private void Awake(){
        if(!_isStarted) _awake();
        CharacterTeam = Color.blue;
        ExtraMoveIndicator = Color.green;
        EnabledRoads = CellTypes.EnabledForBlue;
        MoveEvent = new UnityEvent();
        CharacterIdentifier = CellTypes.BlueLocator;
    }

    private void Start(){PreDeed();}

    public void Update(){
        if (_isBlueMove) Deed();
    }

    public override void RunAway(Vector3 MovePosition){
        _end.GameOver(true, "Синий сбежал");
        Move(MovePosition);
    }

    public override void PreDeed(){
        if (CanMove()) _isBlueMove = true;
        else if (!CanMoveInFuture()) _end.GameOver(false, "Синий пойман");//End game: Red wins. Blue catched.
        else{
            MoveEvent.Invoke();
            PostDeed();
        }
    }

    public override void Deed(){
        if(_wasBlueMoveLast) WhoMovesIndicator.color = ExtraMoveIndicator;
        else WhoMovesIndicator.color = CharacterTeam;
        if (Input.GetMouseButton(0) && !PauseScript._isOnPause){
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50)){
                Vector3 HitPoint = hit.point;
                HitPoint.x = Mathf.Floor(HitPoint.x / 2) * 2 + 1;
                HitPoint.y = 0.5f;
                HitPoint.z = Mathf.Floor(HitPoint.z / 2) * 2 + 1;
                int CoordX = (int) Mathf.Floor(HitPoint.x / 2 + 6);
                int CoordZ = (int) Mathf.Floor(HitPoint.z / 2 + 6);
                int PreviousCoordX = (int) Mathf.Floor(this.transform.position.x / 2 + 6);
                int PreviousCoordZ = (int) Mathf.Floor(this.transform.position.z / 2 + 6);
                if (Mathf.Abs(CoordX - PreviousCoordX) + Mathf.Abs(CoordZ - PreviousCoordZ) == 1){
                    if (CanMoveTo(CoordX, CoordZ)){
                        if(GameProcess.Cells[CoordX, CoordZ] == CellTypes.DoorLocator)
                            RunAway(HitPoint);
                        else{
                            _isBlueMove = false;//player moved just now
                            GameProcess.Cells[CoordX, CoordZ].CellContains = CharacterIdentifier;
                            PositionToLeave = transform.position;
                        
                            MoveEvent.Invoke();
                            if (!GameProcess.Cells[PreviousCoordX, PreviousCoordZ].WasFreezed) CreateIce(PreviousCoordX, PreviousCoordZ);
                            Move(HitPoint);
                        }
                    }
                }
            }
        }
    }

    public override void PostDeed(){
        if (!CanMoveInFuture()) _end.GameOver(false, "Синий пойман");//End game: Red wins. Blue catched.
        else{
            _pointCounter.RecountPointsForBlue();
            if (Bonuses._isMoveActivated && !_wasBlueMoveLast){
                _wasBlueMoveLast = true;
                PreDeed();
            }
            else{
                Bonuses._isMoveActivated = false;
                _wasBlueMoveLast = false;
                Red.PreDeed();
            }
        }
    }
}