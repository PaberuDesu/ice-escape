using UnityEngine;
using UnityEngine.UI;

public class GetLevelData : MonoBehaviour
{
    [SerializeField] private Text _levelNumber;
    [SerializeField] private Text TasksText;
    private void Start(){
        _levelNumber.text = LoadLevel.LevelNumText;
        TasksText.text = LoadLevel.LevelTasksText;
    }
}