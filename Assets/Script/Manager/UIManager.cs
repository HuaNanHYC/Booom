using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    [Header("敌人播放动画速度")]
    public float beforeActionInterval;
    public float actionToReadyInterval;
    public float readyToShootInterval;
    public float shootToReadyInterval;
    public float readyToActionInterval;
    public float actionToIdleInterval;
    [Header("玩家播放动画速度")]
    public float beforeActionIntervalPlayer;
    public float actionToReadyIntervalPlayer;
    public float readyToShootIntervalPlayer;
    public float shootToReadyIntervalPlayer;
    public float readyToActionIntervalPlayer;
    public float actionToIdleIntervalPlayer;
    public bool if_Teach1;//判断各关卡的教程，保证只有一次教程
    public bool if_Teach2;
    public bool if_Teach3;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        SetResolution();
    }


    public void LoadScene(string targetSceneName)//场景跳转
    {
        SceneManageSystem.Instance.GoToFigureScene(targetSceneName);
    }
    /*public void SaveGame()//保存游戏
    {
        InventoryManager.Instance.SavePlayerData();
    }
    public void ClearData()//清除存档
    {
        InventoryManager.Instance.ClearPlayerData();
    }*/

    public void QuitTheGame()//退出游戏
    {
        Application.Quit();
        Application.Quit();

        Application.Quit();

    }
    public void SetResolution()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        bool isFullscreen = true; // 你可以根据需要更改这个值
        Screen.SetResolution(1920, 1080, isFullscreen);
    }
}
