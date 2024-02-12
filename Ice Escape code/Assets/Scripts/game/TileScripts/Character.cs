using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Character : Tile
{
    [SerializeField] private Character characterScript;
    public bool isBlue = false;
    [SerializeField] protected Tile ice;
    public GameLogic gameLogic;

    public UnityEvent MoveEvent = new UnityEvent();

    private void Awake() {
        isBlue = characterScript is blue;
    }

    protected void Move(int moveX, int moveY) {
        int newX = x + moveX;
        int newY = y + moveY;
        if (Mathf.Min(newX, newY) < 0 || Mathf.Max(newX, newY) > GridManager.length) return;
        if (GridManager.grid[newX,newY].canCharacterStepOn(isBlue)) {
            int RotationID = DefineAngleToTurn(moveX, moveY);
            animator.SetInteger("TurnIdentifier", RotationID);
            if (lowerTile.isWarm)
                GridManager.grid[x,y] = lowerTile;
            else if (lowerTile is Ice) {
                GridManager.grid[x,y] = lowerTile;
                ((Ice) lowerTile).MakePermanent();
            }
            else CreateIce(x,y,lowerTile);
            lowerTile = GridManager.grid[newX,newY];
            MoveEvent.Invoke();
            if (GridManager.grid[newX,newY] is Doors) {
                Invoke("StopCharacter", 1.5f);
                GridManager.end.GameOver(true, "Синий сбежал");
                return;
            }
            if (lowerTile is Character) {
                Invoke("StopCharacter", 1.5f);
                GridManager.end.GameOver(false, "Синий съеден");
                return;
            }
            GridManager.grid[newX,newY] = CellTypes.character(isBlue);
            x = newX;
            y = newY;
            gameLogic.NobodyTurn();
        }
    }

    private void StopCharacter() {
        animator.SetInteger("TurnIdentifier", 0);
    }

    private int DefineAngleToTurn(int x, int y){
        int currentAngle = (int) (this.transform.rotation.eulerAngles.y/90)%4;

        int targetAngle = 0;
        if(y == 1) targetAngle = 1;
        else if(x == 1) targetAngle = 2;
        else if(y == -1) targetAngle = 3;
        else if(x == -1) targetAngle = 4;
        int AngleToRotate = targetAngle - currentAngle;
        if(AngleToRotate <= 0) AngleToRotate += 4;
        return AngleToRotate;
    }

    public IEnumerator EndMove(){
        if (EndGame.reasonOfEnd != "") yield break;
        animator.SetInteger("TurnIdentifier", 0);
        yield return new WaitForSeconds(0.05f);
        this.transform.position = new Vector3(x, -0.5f, y);
        this.transform.eulerAngles = new Vector3(0, Mathf.Round(this.transform.rotation.eulerAngles.y/90)*90, 0);
        GridManager.pointCounter.RecountPointsFor(isBlue, x, y);
        if (isBlue && lowerTile is Star) {
            ((Star) lowerTile).GetStar();
            lowerTile = lowerTile.lowerTile;
        }
        if (!isBlue) GridManager.Red.ControlTheDoors();
        CheckIfOpponentMovable();
    }

    public void CreateIce(int x, int y, Tile tile) {
        Tile newIce = Instantiate(CellTypes.ice, new Vector3(x,0,y), Quaternion.identity, GridManager.gridTransform);
        newIce.name = $"ice({x}, {y})";
        newIce.lowerTile = tile;
        GridManager.grid[x,y] = newIce;
        MoveEvent.AddListener(((Ice) newIce).CountTillMeltDown);
    }

    private void CheckIfOpponentMovable() {
        if (isBlue) GridManager.Red.CheckIfMovable();
        else GridManager.Blue.CheckIfMovable();
    }

    public void CheckIfMovable(){
        int AccessLevel = CharacterMinAccessLevel();
        if (AccessLevel == 0) {
            gameLogic.ChangeTurn();
            return;
        }
        if (AccessLevel == 1) {
            gameLogic.ChangeTurnVirtually();
            MoveEvent.Invoke();
            GridManager.pointCounter.RecountPointsFor(isBlue, (int) transform.position.x, (int) transform.position.z);
            if (isBlue) GridManager.Red.CheckIfMovable();
            else GridManager.Blue.CheckIfMovable();
            return;
        }
        string character = isBlue ? "Синий" : "Красный";
        GridManager.end.GameOver(!isBlue, $"{character} пойман");
    }

    public int CharacterMinAccessLevel() {
        int UpAccess = DefineAccessLevel(GridManager.grid[x,y+1]);
        int DownAccess = y > 0 ? DefineAccessLevel(GridManager.grid[x,y-1]) : 0;
        int RightAccess = DefineAccessLevel(GridManager.grid[x+1,y]);
        int LeftAccess = DefineAccessLevel(GridManager.grid[x-1,y]);

        return MinAccessLevel(UpAccess, DownAccess, RightAccess, LeftAccess);
    }

    private int DefineAccessLevel(Tile tile) {
        if (tile.canCharacterStepOn(isBlue)) return 0;
        if (tile._isSelfDestroyable) return 1;
        return 2;
    }

    private int MinAccessLevel(int a, int b, int c, int d) {
        if (a > b) a = b;
        if (a > c) a = c;
        if (a > d) a = d;
        return a;
    }
}