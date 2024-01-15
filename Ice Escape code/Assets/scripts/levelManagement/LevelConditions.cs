using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
        TasksText.text = "";
        for (int i = 0; i < 3; i++)
            TasksText.text += $"\n★ {_tasks[i].definition}\n";
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
                definition = "Синий победил";
                break;
            case 1:
                act = (int x) => Character.Red._isNextTo(CellTypes.PermanentIceLocator);
                definition = "В конце игры красный возле твердого льда";
                break;
            case 2:
                act = (int x) => EndGame.reasonOfEnd == "Синий сбежал";
                definition = "Синий покинул комнату";
                break;
            case 3:
                act = (int x) => PointCounter._pointCounter > 30;
                definition = "набрано больше 30 очков";
                break;
            case 4:
                act = (int x) => GetStarOnField.starObject?.activeInHierarchy == false;
                definition = "Собрана звезда в комнате";
                break;
            case 5:
                act = (int x) => Character.Red._isNextTo(CellTypes.WallLocator);
                definition = "В конце игры красный возле стены";
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
        for(int i=0; i<3; i++){if (LevelConditions.PreviousResult[i] && !LevelConditions.CurrentResult[i]) return 0;}
        LevelButActivator.LevelResults[LoadLevel.LevelNumber].GotStars = new List<bool>(LevelConditions.CurrentResult);
        LevelButActivator.SaveResults();
        return 0;
    }
}