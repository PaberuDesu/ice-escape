public class Doors : Tile
{
    public bool isOpened = false;

    public void Open() {
        ChangeParameters(true);
    }

    public void Close() {
        ChangeParameters(false);
    }

    private void ChangeParameters(bool value) {
        isOpened = value;
        animator.SetBool("_isEnemyCatched", value);
        _canBlueStepOn = value;
    }
}