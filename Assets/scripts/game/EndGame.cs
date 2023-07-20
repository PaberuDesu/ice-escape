using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject oldCanvasBlocker;

    [SerializeField] private Transform YouWon;
    [SerializeField] private Transform YouLose;
    [SerializeField] private Camera EventCam;

    public static string reasonOfEnd;
    public static UnityEvent EndEvent = new UnityEvent();

    private void Awake() {
        reasonOfEnd = "";
    }

    public void GameOver(bool victory, string reason){
        if(reasonOfEnd == ""){
            reasonOfEnd = reason;
            Transform Gates = victory ? Instantiate(YouWon) : Instantiate(YouLose);
            oldCanvasBlocker.SetActive(true);
            Transform _canvas = Gates.GetChild(2).GetChild(0).GetChild(0).GetChild(0);
            _canvas.GetComponent<Canvas>().worldCamera = EventCam;
            Text _reason = _canvas.GetChild(0).GetChild(0).GetComponent<Text>();
            _reason.text = reason;
            if (victory){
                Transform Stars = _canvas.GetChild(4);
                foreach(Transform star in Stars)
                    LevelConditions.LevelStars.Add(star.gameObject);
                LevelButActivator.LevelResults[LoadLevel.LevelNumber].IsCompleted = true;
                EndEvent.Invoke();
            }
        }
    }
}
