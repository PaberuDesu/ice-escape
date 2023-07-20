using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private LevelConditions LevelInfo;
    [SerializeField] private GameObject _table;
    public static string LevelNumText;
    public static string LevelTasksText;
    public static int LevelNumber = 0;

    public void Load(int levelNumber){
        LevelNumber = levelNumber;
        LevelInfo.LoadInfo();
        LevelNumText = LevelInfo._levelNumber.text;
        LevelTasksText = LevelInfo.TasksText.text;
        _table.SetActive(true);
    }

    public void StartGame(){
        SceneManager.LoadScene(2);
    }
}