using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelConditions : MonoBehaviour {
    public Text _levelNumber;
    public Text TasksText;
    [SerializeField] internal static List<GameObject> LevelStars;
    private TaskData[] _tasks;
    internal static bool[] PreviousResult;
    internal static bool[] CurrentResult;

    public void LoadInfo() {
        EndGame.EndEvent.RemoveAllListeners();
        int StarCount = 3;
        PreviousResult = LevelButActivator.LevelResults[LoadLevel.LevelNumber].GotStars.ToArray();
        CurrentResult = new bool[StarCount];
        LevelStars = new List<GameObject>(0);
        LevelPassConditions level_tasks = JsonUtility.FromJson<LevelPassConditions>(File.ReadAllText(Application.streamingAssetsPath + $"/Levels/level{LoadLevel.LevelNumber}-Tasks.json"));
        _levelNumber.text = $"Уровень {LoadLevel.LevelNumber+1}";
        _tasks = level_tasks.Tasks();
        TasksText.text = $"★ {_tasks[0].definition}";
        for (int i = 1; i < 3; i++)
            TasksText.text += $"\n\n★ {_tasks[i].definition}";
    }
}

[System.Serializable]
public class LevelPassConditions{
    public Task first;
    public Task second;
    public Task third;

    internal TaskData[] Tasks(){
        TaskData[] _tasks = new TaskData[3];
        _tasks[0] = first.DefineData(0);
        _tasks[1] = second.DefineData(1);
        _tasks[2] = third.DefineData(2);
        return _tasks;
    }
}

[System.Serializable]
public class Task{
    public int taskID;

    internal TaskData DefineData(int number){
        return new TaskData(taskID, number);
    }
}

internal class TaskData{
    internal string definition;
    internal Predicate<int> act;
    int StarNumber;

    internal TaskData(int ID, int _starNumber){
        EndGame.EndEvent.AddListener(DoCheck);
        StarNumber = _starNumber;
        switch(ID){
            case 0:
                act = (int x) => true;
                definition = "СИНИЙ ПОБЕДИЛ";
                break;
            case 1:
                act = (int x) => GridManager.Red.isNextToPermanent();
                definition = "В КОНЦЕ ИГРЫ КРАСНЫЙ ВОЗЛЕ ТВЕРДОГО ЛЬДА";
                break;
            case 2:
                act = (int x) => EndGame.reasonOfEnd == "Синий сбежал";
                definition = "СИНИЙ ПОКИНУЛ КОМНАТУ";
                break;
            case 3:
                act = (int x) => PointCounter._pointCounter > 30;
                definition = "НАБРАНО БОЛЬШЕ 30 ОЧКОВ";
                break;
            case 4:
                act = (int x) => Star.GetStarToLevelResult();
                definition = "ПОЛУЧЕНА ЗВЕЗДА В КОМНАТЕ";
                break;
            case 5:
                act = (int x) => GridManager.Red.isNextToWall();
                definition = "В КОНЦЕ ИГРЫ КРАСНЫЙ ВОЗЛЕ СТЕНЫ";
                break;
            default:
                act = (int x) => true;
                definition = "";
                break;
        }
    }

    public void DoCheck(){
        EndGame.EndEvent.RemoveListener(DoCheck);
        bool Result = act(0);
        LevelConditions.LevelStars[StarNumber]?.SetActive(Result);
        LevelConditions.CurrentResult[StarNumber] = Result;
        int MaxStarNumber = 2;
        if(StarNumber == MaxStarNumber) RewriteStars();
    }

    private int RewriteStars(){
        for (int i = 0; i < 3; i++) {
            if (LevelConditions.PreviousResult[i] && !LevelConditions.CurrentResult[i]) return 0;
        }
        LevelButActivator.LevelResults[LoadLevel.LevelNumber].GotStars = new List<bool>(LevelConditions.CurrentResult);
        LevelButActivator.SaveResults();
        return 0;
    }
}