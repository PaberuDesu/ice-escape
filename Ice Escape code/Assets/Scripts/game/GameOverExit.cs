using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverExit : MonoBehaviour
{
    public void Restart(){SceneManager.LoadScene(2);}

    public void MainMenu(){SceneManager.LoadScene(0);}
}