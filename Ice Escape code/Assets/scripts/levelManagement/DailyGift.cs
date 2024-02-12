using UnityEngine;
using UnityEngine.UI;
using System;

public class DailyGift : MonoBehaviour
{
    [SerializeField] private GemMarket _gemMarket;

    [SerializeField] private GameObject DailyGiftPanel;
    [SerializeField] private Transform RewardsVisual;
    [SerializeField] private Transform DayStreakVisual;

    private int DailyGiftSize;

    private string PreviousDate{
        get => PlayerPrefs.GetString("Date");
        set => PlayerPrefs.SetString("Date", value);
    }

    private string CurrentDate;

    private int DayStreak{
        get => PlayerPrefs.GetInt("DayStreak");
        set => PlayerPrefs.SetInt("DayStreak", value);
    }

    private int newDayStreak;

    private void Start(){
        const int MaxDayStreak = 7;
        int[] Rewards = new int[3]{1,3,5};
        int[] DailyGiftSizeArray = new int[MaxDayStreak]{Rewards[0], Rewards[0], Rewards[1], Rewards[1], Rewards[1], Rewards[2], Rewards[2]};
        CurrentDate = DateTime.UtcNow.ToString("dd/MM/yyyy");
        var Difference = DateTime.Parse(CurrentDate) - DateTime.Parse(PreviousDate);
        int DaysBetweenRewards = 1;

        if(Difference.TotalDays < DaysBetweenRewards) return;

        if(Difference.TotalDays == DaysBetweenRewards) newDayStreak = (DayStreak % MaxDayStreak)+1;
        else newDayStreak = 0;

        DailyGiftSize = DailyGiftSizeArray[newDayStreak];
        DailyGiftPanel.SetActive(true);
        for(int i = 0; i < Rewards.Length; i++){
            if (DailyGiftSize == Rewards[i]) RewardsVisual.GetChild(i).gameObject.SetActive(true);
        }
        for (int i = 0; i < MaxDayStreak; i++){
            Transform Day = DayStreakVisual.GetChild(i);
            Day.gameObject.GetComponent<Image>().color = Color.white;
            if(newDayStreak > i) Day.GetChild(0).gameObject.SetActive(true);
            else break;
        }
    }

    public void GetGift(){
        DayStreak = newDayStreak;
        _gemMarket.Earn(DailyGiftSize);
        DailyGiftPanel.SetActive(false);
        PreviousDate = CurrentDate;
    }
}