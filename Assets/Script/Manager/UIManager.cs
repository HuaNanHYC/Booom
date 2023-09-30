using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance.gameObject);
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
