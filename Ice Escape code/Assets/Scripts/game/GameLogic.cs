using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public static int turn = 1;
    [SerializeField] private Text indicator;
    private static Color[] colors = {Color.red, Color.blue};
    [SerializeField] private Bonuses bonuses;
    private bool _isBonusActivated = false;

    public void ChangeTurn() {
        if (bonuses._areBonusesActivated[2] && !_isBonusActivated) {
            turn++;
            _isBonusActivated = true;
        }
        turn = (turn + 1) % 2;
        deactivateBonus();
        indicator.color = colors[turn];
    }

    public void ChangeTurnVirtually() {
        turn++;
    }

    public void RepeatTurn() {
        turn = turn % 2;
        deactivateBonus();
    }

    public void NobodyTurn() {
        turn += 2;
    }

    private void deactivateBonus() {
        if (turn == 0 && _isBonusActivated) {
            _isBonusActivated = false;
            bonuses.DeactivateBonus(2);
        }
    }
}