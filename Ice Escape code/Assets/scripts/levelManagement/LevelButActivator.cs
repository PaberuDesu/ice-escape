using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButActivator : MonoBehaviour
{
    [SerializeField] private Transform[] _levelButs;
    public static LevelBunch LevelResults;

    private void Awake(){
        LevelResults = new LevelBunch();
        LevelResults = JsonUtility.FromJson<LevelBunch>(File.ReadAllText(Application.streamingAssetsPath + $"/Levels/levelBunch{0}-Progress.json"));
        for(int i = 0; i < 20; i++){
            Transform Button = _levelButs[i];
            LevelResult result = LevelResults[i];
            Button.GetComponent<Button>().interactable = i>0 ? LevelResults[i-1].IsCompleted : true;
            if(result.IsCompleted){
                for(int j = 0; j < 3; j++)
                    Button.GetChild(j).gameObject.SetActive(result.GotStars[j]);
            } else break;
        }
    }

    public static void SaveResults(){
        File.WriteAllText(Application.streamingAssetsPath + $"/Levels/levelBunch{0}-Progress.json", JsonUtility.ToJson(LevelResults));
    }
}

[System.Serializable]
public class LevelBunch{
    public List<LevelResult> allLevelsOfBunch;

    public LevelResult this[int key]{
        get {return allLevelsOfBunch[key];}
        set {allLevelsOfBunch[key] = value;}
    }
}

[System.Serializable]
public class LevelResult{
    public bool IsCompleted;
    public List<bool> GotStars;
}