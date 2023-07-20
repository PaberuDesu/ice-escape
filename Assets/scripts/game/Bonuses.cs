using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bonuses : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField] private GemMarket _gemMarket;

    private GameObject _hammerObject;

    [SerializeField] private GameObject _hammer;
    [SerializeField] private GameObject _furnace;

    [SerializeField] PauseScript PauseSet;

    [SerializeField] GameObject PauseBut;
    [SerializeField] GameObject ExitBonusModeBut;

    private const int HammerPrice = 0;//5
    private const int FurnacePrice = 10;
    private const int MovePrice = 3;

    [SerializeField] Button hammerBut;
    [SerializeField] Button furnaceBut;
    [SerializeField] Button moveBut;

    [SerializeField] Text hammerPriceVisualiser;
    [SerializeField] Text furnacePriceVisualiser;
    [SerializeField] Text movePriceVisualiser;

    private static bool _isHammerActivated;
    private static bool _isFurnaceActivated;
    public static bool _isMoveActivated;

    private static bool _isInBonusMode;
    
    public static GameObject destroyableObject;

    private void Start(){
        _mainCamera = Camera.main;
        hammerPriceVisualiser.text = $"{HammerPrice}";
        furnacePriceVisualiser.text = $"{FurnacePrice}";
        movePriceVisualiser.text = $"{MovePrice}";
        _isHammerActivated = false;
        _isFurnaceActivated = false;
        _isMoveActivated = false;
        _isInBonusMode = false;
        Character.Red.MoveEvent.AddListener(moveButActivate);
    }

    private void Update(){
        if(destroyableObject)
            StartCoroutine(destroyProcess());
        if (Input.GetMouseButtonUp(0) && _isInBonusMode){
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50)){
                Vector3 HitPoint = hit.point;
                HitPoint.x = Mathf.Floor(HitPoint.x / 2) * 2 + 1;
                HitPoint.y = 0.5f;
                HitPoint.z = Mathf.Floor(HitPoint.z / 2) * 2 + 1;
                int x = (int) Mathf.Floor(HitPoint.x / 2 + 6);
                int y = (int) Mathf.Floor(HitPoint.z / 2 + 6);
                if (_isHammerActivated){
                    if (GameProcess.Cells[x,y]._isObstacleDestroyable()){
                        StartCoroutine(HammerAnimate(x,y,HitPoint));
                        _gemMarket.Buy(HammerPrice);
                    }
                }
                if (_isFurnaceActivated){
                    if (GameProcess.Cells[x,y]._isAbsolutelyEmpty()){
                        Instantiate(_furnace, HitPoint, Quaternion.identity);
                        GameProcess.Cells[x,y] = new Cell(CellTypes.FurnaceLocator, x, y);
                        LeaveBonusMode();
                        _gemMarket.Buy(FurnacePrice);
                    }
                }
            }
        }
    }

    private IEnumerator destroyProcess(){
        destroyableObject.GetComponent<Animator>().Play("SmashingDestructable");
        yield return new WaitForSeconds(1);
        Destroy(destroyableObject);
    }

    private IEnumerator HammerAnimate(int x, int y, Vector3 HitPoint){
        _hammerObject = Instantiate(_hammer, HitPoint, Quaternion.identity);
        GameProcess.Cells[x,y].DestroyObstacle();
        LeaveBonusMode();
        yield return new WaitForSeconds(1);
        Destroy(_hammerObject);
    }

    public void Hammer(){
        if(_gemMarket._isEnoughMoney(HammerPrice)){
            EnterBonusMode();
            _isHammerActivated = true;
        }
    }

    public void Furnace(){
        if(_gemMarket._isEnoughMoney(FurnacePrice)){
            EnterBonusMode();
            _isFurnaceActivated = true;
        }
    }

    public void Move(){
        if(_gemMarket._isEnoughMoney(MovePrice)){
            _isMoveActivated = true;
            moveBut.interactable = false;
            _gemMarket.Buy(MovePrice);
        }
    }

    private void moveButActivate(){
        moveBut.interactable = true;
    }

    private void EnterBonusMode(){
        PauseSet.Pause();
        PauseBut.SetActive(false);
        ExitBonusModeBut.SetActive(true);

        hammerBut.interactable = false;
        furnaceBut.interactable = false;
        moveBut.interactable = false;
        _isInBonusMode = true;
    }

    public void LeaveBonusMode(){
        PauseSet.Resume();
        PauseBut.SetActive(true);
        ExitBonusModeBut.SetActive(false);

        hammerBut.interactable = true;
        furnaceBut.interactable = true;
        if(!_isMoveActivated)moveBut.interactable = true;
        _isHammerActivated = false;
        _isFurnaceActivated = false;
        _isInBonusMode = false;
    }
}