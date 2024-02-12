using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bonuses : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private GemMarket _gemMarket;

    [SerializeField] private GameObject _hammer;

    [SerializeField] PauseScript PauseSet;
    [SerializeField] Button PauseBut;
    [SerializeField] GameObject ExitBonusModeBut;
    private static bool _isInBonusMode;

    [SerializeField] Button[] BonusButs;
    [SerializeField] private GameObject[] UseVisualisers;
    public bool[] _areBonusesActivated = new bool[3];
    private static int[] Prices = {5, 10, 3};
    [SerializeField] Text[] PriceVisualisers;

    private void Start(){
        _mainCamera = Camera.main;
        for (int i = 0; i < 3; i++) {
            PriceVisualisers[i].text = $"{Prices[i]}";
            _areBonusesActivated[i] = false;
        }
        _isInBonusMode = false;
    }

    private void Update(){
        if (Input.GetMouseButtonUp(0) && _isInBonusMode){
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50)){
                Vector3 HitPoint = hit.point;
                HitPoint.x = Mathf.Round(HitPoint.x);
                HitPoint.y = 0;
                HitPoint.z = Mathf.Round(HitPoint.z);
                int x = (int) HitPoint.x;
                int y = (int) HitPoint.z;
                if (_areBonusesActivated[0] && GridManager.grid[x,y]._isPlayerDestroyable) {
                    StartCoroutine(HammerAnimate(x,y,HitPoint));
                    LeaveBonusMode();
                    _gemMarket.Buy(Prices[0]);
                    return;
                }
                if (_areBonusesActivated[1] && GridManager.grid[x,y].isEmpty) {
                    GridManager.grid[x,y] = Instantiate(CellTypes.furnace, HitPoint, Quaternion.identity);
                    LeaveBonusMode();
                    _gemMarket.Buy(Prices[1]);
                    return;
                }
            }
        }
    }

    private IEnumerator HammerAnimate(int x, int y, Vector3 HitPoint){
        Quaternion rotation = x <= 5 ? Quaternion.identity : Quaternion.Euler(0,180,0);
        GameObject _hammerObject = Instantiate(_hammer, HitPoint, rotation);
        Tile destroyableTile = GridManager.grid[x,y];
        Debug.Log(destroyableTile);
        destroyableTile.animator.Play("SmashingDestructable");
        GridManager.grid[x,y] = GridManager.grid[x,y].lowerTile;
        yield return new WaitForSeconds(1);
        Destroy(_hammerObject);
        Destroy(destroyableTile.gameObject);
    }

    public void useBonus(int bonusNumber){
        if(GameLogic.turn == blue.turn && _gemMarket._isEnoughMoney(Prices[bonusNumber])){
            if (bonusNumber < 2) {
                _areBonusesActivated[bonusNumber] = true;
                UseVisualisers[bonusNumber].SetActive(true);
                EnterBonusMode();
            }
            else if (GridManager.Red.CharacterMinAccessLevel() == 0) {
                _areBonusesActivated[bonusNumber] = true;
                UseVisualisers[bonusNumber].SetActive(true);
                BonusButs[bonusNumber].interactable = false;
                _gemMarket.Buy(Prices[bonusNumber]);
            }
        }
    }

    public void DeactivateBonus(int bonusNumber) {
        _areBonusesActivated[bonusNumber] = false;
        UseVisualisers[bonusNumber].SetActive(false);
        BonusButs[bonusNumber].interactable = true;
    }

    private void EnterBonusMode(){
        PauseSet.Pause();
        PauseBut.interactable = false;
        ExitBonusModeBut.SetActive(true);

        for (int i = 0; i < 3; i++)
            BonusButs[i].interactable = false;
        _isInBonusMode = true;
    }

    public void LeaveBonusMode(){
        if(_areBonusesActivated[0]) UseVisualisers[0].SetActive(false);
        else if(_areBonusesActivated[1]) UseVisualisers[1].SetActive(false);

        PauseSet.Resume();
        PauseBut.interactable = true;
        ExitBonusModeBut.SetActive(false);

        for (int i = 0; i < 2; i++) {
            BonusButs[i].interactable = true;
            _areBonusesActivated[i] = false;
        }
        if(!_areBonusesActivated[2]) BonusButs[2].interactable = true;
        _isInBonusMode = false;
    }
}