using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class GemMarket : MonoBehaviour
{
    private int GemCounter;
    private Text GemVisualCount;

    private void Awake(){
        GemVisualCount = this.gameObject.GetComponent<Text>();
        GemCounter = PlayerPrefs.GetInt("Gems");
        GemVisualCount.text = $"{GemCounter}";
    }

    public bool _isEnoughMoney(int price){return GemCounter >= price;}

    public void Buy(int price){
        GemCounter -= price;
        PlayerPrefs.SetInt("Gems", GemCounter);
        GemVisualCount.text = $"{GemCounter}";
    }

    public void Earn(int income){
        GemCounter += income;
        PlayerPrefs.SetInt("Gems", GemCounter);
        GemVisualCount.text = $"{GemCounter}";
    }
}