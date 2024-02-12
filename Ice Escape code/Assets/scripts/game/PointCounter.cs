using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    [SerializeField] Text outText;

    [SerializeField] private GemMarket _gemMarket;
    private int PointAchieveCount = 1;
    private const int PointAchieveMultiplier = 20;

    public static int _pointCounter;

    void Awake() {
        _pointCounter = 10;
    }

    public void RecountPointsFor(bool isBlue, int x, int y) {
        int numberOfIce = CountPermanentIceAround(x,y);
        if (numberOfIce > 0){
            int FurnaceBelowCounter = (GridManager.grid[x,y] == CellTypes.furnace) ? 1 : 0;
            int multiplier = isBlue ? -2 : 1;
            _pointCounter += multiplier * (numberOfIce - FurnaceBelowCounter);
            outText.text = $"{_pointCounter}";
            if (multiplier < 0) {
                if (_pointCounter <= 0) GridManager.end.GameOver(false, "Закончились очки");
            }
            else CheckAchieve();
        }
    }

    private void CheckAchieve(){//if points reached 20, 40, 60... give gem
        if(_pointCounter >= PointAchieveCount * PointAchieveMultiplier){
            PointAchieveCount++;
            _gemMarket.Earn(1);
        }
    }

    private int CountPermanentIceAround(int x, int y){
        int PermanentIceCount = (y < 11)
            ? (IsPermanentIce(GridManager.grid[x, y+1]))
            : 0;
        PermanentIceCount += (y > 0)
            ? (IsPermanentIce(GridManager.grid[x, y-1]))
            : 0;
        PermanentIceCount += (x < 11)
            ? (IsPermanentIce(GridManager.grid[x+1, y]))
            : 0;
        PermanentIceCount += (x > 0)
            ? (IsPermanentIce(GridManager.grid[x-1, y]))
            : 0;
        return PermanentIceCount;
    }

    private int IsPermanentIce(Tile tile) {
        return (tile is Ice && !tile._isSelfDestroyable) ? 1 : 0;
    }
}