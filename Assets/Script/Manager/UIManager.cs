using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    public bool if_Teach1;//判断各关卡的教程，保证只有一次教程
    public bool if_Teach2;
    public bool if_Teach3;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }


    public void LoadScene(string targetSceneName)//场景跳转
    {
        SceneManageSystem.Instance.GoToFigureScene(targetSceneName);
    }
    public void SaveGame()//保存游戏
    {
        InventoryManager.Instance.SavePlayerData();
    }
    public void ClearData()//清除存档
    {
        InventoryManager.Instance.ClearPlayerData();
    }

    public void QuitTheGame()//退出游戏
    {
        Application.Quit();
    }
}
