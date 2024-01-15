using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour {
    public void StartGame(){SceneManager.LoadScene(1);}

    public void Exit() {Application.Quit();}
}