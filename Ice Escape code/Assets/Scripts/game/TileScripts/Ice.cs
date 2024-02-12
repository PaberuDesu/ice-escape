public class Ice : Tile
{
    private int moveCounter = 0;
    private const int moveLimit = 10;

    public void CountTillMeltDown() {
        moveCounter++;
        if (moveCounter >= moveLimit){
            _canBlueStepOn = lowerTile._canBlueStepOn;
            _canRedStepOn = lowerTile._canRedStepOn;
            animator.SetBool("_isDestroy", true);
            GridManager.Blue.MoveEvent.RemoveListener(CountTillMeltDown);
            GridManager.Red.MoveEvent.RemoveListener(CountTillMeltDown);
        }
    }

    public void MakePermanent() {
        _canBlueStepOn = false;
        _canRedStepOn = false;
        _isSelfDestroyable = false;
        animator.SetBool("_isIceUndestructible", true);
    }
}