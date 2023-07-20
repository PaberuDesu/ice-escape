using UnityEngine;

public class GameOverExit : MonoBehaviour
{
    public void Restart(){Camera.main.GetComponent<RestartTheGame>().Restart();}

    public void MainMenu(){Camera.main.GetComponent<BackToMenu>().ToMainMenu();}
}