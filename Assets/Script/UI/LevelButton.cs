using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelId;
    public string figureScene;
    private Button button;
    void Start()
    {
        button = GetComponent<Button>();
        ButtonOnClick();

    }
    public void SetCurrentLevel()//按钮点击后设置当前关卡
    {
        LevelManager.Instance.SetCurrentLevel(levelId);
    }
    public void StartGame()//按钮点击后开始游戏，跳转场景
    {
        SceneManageSystem.Instance.GoToFigureScene(figureScene);
    }
    public void ButtonOnClick()
    {
        button.onClick.AddListener(()=>SetCurrentLevel());
        button.onClick.AddListener(()=>StartGame());
    }

}
