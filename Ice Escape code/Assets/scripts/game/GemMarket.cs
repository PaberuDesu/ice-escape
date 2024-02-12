using UnityEngine;
using UnityEngine.UI;


public class GemMarket : MonoBehaviour
{
    private Text GemVisualCount;
    private int gems;
    private int gemsIncome {
        set {
            gems += value;
            PlayerPrefs.SetInt("Gems", gems);
            GemVisualCount.text = $"{gems}";
        }
    }

    private void Awake(){
        GemVisualCount = this.gameObject.GetComponent<Text>();
        gems = PlayerPrefs.GetInt("Gems");
        GemVisualCount.text = $"{gems}";
    }

    public bool _isEnoughMoney(int price){return gems >= price;}

    public void Buy(int price){
        gemsIncome = -price;
    }

    public void Earn(int income) {
        gemsIncome = income;
    }
}