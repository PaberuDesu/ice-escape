using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    [SerializeField] private EndGame _end;
    [SerializeField] Text outText;

    [SerializeField] private GemMarket _gemMarket;
    private int PointAchieveCount = 1;
    private const int PointAchieveMultiplier = 20;

    public static int _pointCounter;
    
    private void Awake() {
        _pointCounter = 10;
    }

    public void RecountPointsForBlue(){
        Vector2 PlayerPosition = FindThePlayer(true);
        int numberOfIce  = CountPermanentIceAround(PlayerPosition);
        if (numberOfIce > 0){
            int FurnaceBelowCounter = (GameProcess.Cells[(int)PlayerPosition.x, (int)PlayerPosition.y] == CellTypes.FurnaceLocator) ? 1 : 0;
            _pointCounter -= 2 * (numberOfIce - FurnaceBelowCounter);
            outText.text = $"{_pointCounter}";
            if (_pointCounter <= 0) _end.GameOver(false, "Закончились очки");
        }
    }

    public void RecountPointsForRed(){
        Vector2 PlayerPosition = FindThePlayer(false);
        int numberOfIce  = CountPermanentIceAround(PlayerPosition);
        if (numberOfIce > 0){
            int FurnaceBelowCounter = (GameProcess.Cells[(int)PlayerPosition.x, (int)PlayerPosition.y] == CellTypes.FurnaceLocator) ? 1 : 0;
            _pointCounter += numberOfIce - FurnaceBelowCounter;
            CheckAchieve();
            outText.text = $"{_pointCounter}";
        }
    }

    private void CheckAchieve(){
        if(_pointCounter >= PointAchieveCount * PointAchieveMultiplier){
            PointAchieveCount++;
            _gemMarket.Earn(1);
        }
    }

    private Vector2 FindThePlayer(bool _isPlayerBlue){
        if (_isPlayerBlue) return new Vector2(Mathf.Floor(Character.Blue.transform.position.x / 2 + 6), Mathf.Floor(Character.Blue.transform.position.z / 2 + 6));
        else return new Vector2(Mathf.Floor(Character.Red.transform.position.x / 2 + 6), Mathf.Floor(Character.Red.transform.position.z / 2 + 6));
    }

    private int CountPermanentIceAround(Vector2 position){
        if (position == Vector2.zero) return 0;
        int CoordX = (int) position.x, CoordZ = (int) position.y;
        int PermanentIceCount = (CoordZ < 11)
            ? ((GameProcess.Cells[CoordX, CoordZ + 1] == CellTypes.PermanentIceLocator)
                ? 1 : 0)
            : 0;
        PermanentIceCount += (CoordZ > 0)
            ? ((GameProcess.Cells[CoordX, CoordZ - 1] == CellTypes.PermanentIceLocator)
                ? 1 : 0)
            : 0;
        PermanentIceCount += (CoordX < 11)
            ? ((GameProcess.Cells[CoordX + 1, CoordZ] == CellTypes.PermanentIceLocator)
                ? 1 : 0)
            : 0;
        PermanentIceCount += (CoordX > 0)
            ? ((GameProcess.Cells[CoordX - 1, CoordZ] == CellTypes.PermanentIceLocator)
                ? 1 : 0)
            : 0;
        return PermanentIceCount;
    }
}
