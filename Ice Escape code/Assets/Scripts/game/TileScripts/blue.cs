public class blue : Character
{
    public const int turn = 1;
    private void FixedUpdate()
    {
        if (GameLogic.turn == turn) {
            if (DragController.x + DragController.y != 0) {
                Move(DragController.x, DragController.y);
                DragController.x = 0;
                DragController.y = 0;
            }
            else return;
        }
    }
}