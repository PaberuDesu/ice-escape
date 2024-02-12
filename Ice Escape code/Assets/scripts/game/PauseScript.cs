using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private LevelConditions LevelInfo;
    public static bool _isOnPause{get; private set;} = false;

    private void Start() {
        LevelInfo.LoadInfo();
    }

    public void Pause(){
        Time.timeScale = 0;
        _isOnPause = true;
    }
    public void Resume(){
        Time.timeScale = 1;
        _isOnPause = false;
    }
}