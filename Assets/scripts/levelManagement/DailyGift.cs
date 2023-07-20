using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyGift : MonoBehaviour
{
    [SerializeField] private GemMarket _gemMarket;

    [SerializeField] private GameObject DailyGiftPanel;
    [SerializeField] private Transform RewardsVisual;
    [SerializeField] private Transform DayStreakVisual;

    private int[] DailyGiftSizeArray;
    private int DailyGiftSize;

    private int[] Rewards;

    private string PreviousDate{
        get => PlayerPrefs.GetString("Date");
        set => PlayerPrefs.SetString("Date", value);
    }
    private string CurrentDate;

    private const int DaysBetweenRewards = 1;
    private const int MaxDayStreak = 7;
    private int DayStreak{
        get => PlayerPrefs.GetInt("DayStreak");
        set => PlayerPrefs.SetInt("DayStreak", value);
    }
    private int newDayStreak;

    private void Start(){
        Rewards = new int[3]{1,3,5};
        DailyGiftSizeArray = new int[MaxDayStreak]{Rewards[0], Rewards[0], Rewards[1], Rewards[1], Rewards[1], Rewards[2], Rewards[2]};
        CurrentDate = DateTime.UtcNow.ToString("dd/MM/yyyy");
        var Difference = DateTime.Parse(CurrentDate) - DateTime.Parse(PreviousDate);
        if(Difference.TotalDays >= DaysBetweenRewards){
            if(Difference.TotalDays == DaysBetweenRewards) newDayStreak = (DayStreak % MaxDayStreak);
            else newDayStreak = 0;
            DailyGiftSize = DailyGiftSizeArray[newDayStreak];
            DailyGiftPanel.SetActive(true);
            for(int i = 0; i < Rewards.Length; i++){
                if (DailyGiftSize == Rewards[i]) RewardsVisual.GetChild(i).gameObject.SetActive(true);//image of size of gift
            }
            for (int i = 0; i < MaxDayStreak; i++){
                if(newDayStreak > i) DayStreakVisual.GetChild(i).GetChild(0).gameObject.SetActive(true);//passed days visualisation
                if(newDayStreak < i) DayStreakVisual.GetChild(i).gameObject.GetComponent<Image>().color = Color.grey;//deactivate days that are not reached
                else DayStreakVisual.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void GetGift(){
        DayStreak = newDayStreak;
        _gemMarket.Earn(DailyGiftSize);
        DailyGiftPanel.SetActive(false);
        PreviousDate = CurrentDate;
    }
}