using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelButton : MonoBehaviour
{
    //public int levelId;
    //public string figureScene;
    private Button button;
    public bool if_Last;
    [Header("仅用作测试，快速跳关取1-8")]
    public int index=1;
    void Start()
    {
        button = GetComponent<Button>();
        ButtonOnClick();

    }
    public void SetCurrentLevel()//按钮点击后设置当前关卡
    {
        if (if_Last) return;
        LevelManager.Instance.SetCurrentLevel(LevelManager.Instance.currentLevelId + index);
    }
    public void StartGame()//按钮点击后开始游戏，跳转场景
    {
        LevelManager.Instance.DialogueAfterBlack();
    }
    public void ButtonOnClick()
    {
        button.onClick.AddListener(()=>SetCurrentLevel());
        button.onClick.AddListener(()=>StartGame());
    }

}
