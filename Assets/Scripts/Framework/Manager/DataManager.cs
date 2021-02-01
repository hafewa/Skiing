using System;
using UnityEngine;

public class DataManager : IManager
{
    public PlayerData playerData;
    public StageData stageData;
    // public GuideData guideData;

    public override void OnInit() {
        playerData = new PlayerData();
        stageData = new StageData();
        // guideData = new GuideData();

        playerData.OnInit();
        stageData.OnInit();
        // guideData.OnInit();

        int nDay    = PlayerPrefs.GetInt("zd_ydate");
        int nNowD   = DateTime.Now.DayOfYear;
        if(nNowD != nDay)
        {
            PlayerPrefs.SetInt("zd_ydate", nNowD);
            PlayerPrefs.SetInt("zd_time_rolereward",2);

        }
    }
    public override void OnUpdate() {
        playerData.OnUpdate();
        stageData.OnUpdate();
        // guideData.OnUpdate();
    }
    public override void OnRelease() {
        playerData.OnRelease();
        stageData.OnRelease();
        // guideData.OnRelease();
    }

    public void SetData() {
        playerData.SetData();
        stageData.SetData();
        // guideData.SetData();
    }
}