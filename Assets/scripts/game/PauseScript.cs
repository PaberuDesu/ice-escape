using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool _isOnPause{get; private set;} = false;
    public void Pause(){
        Time.timeScale = 0;
        _isOnPause = true;
    }
    public void Resume(){
        Time.timeScale = 1;
        _isOnPause = false;
    }
}