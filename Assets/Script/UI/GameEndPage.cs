using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueSystem;
public class GameEndPage : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public void Win()//游戏胜利的方法
    {
        LevelManager.Instance.DialogueAfterBlack();//进入关卡结束剧情
    }
    public void Lose()
    {
        //特殊关卡判断
        if (LevelManager.Instance.currentLevelId == 30001)
        {
            if (LevelManager.Instance.StartSpecialDialogue())
            {
                return;
            }
        }

        LevelManager.Instance.lastLevelJudge = true;



        lose.SetActive(true);
    }
    public void RestartButton()
    {
        UIManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReturnLevelSelectScene()
    {
        UIManager.Instance.LoadScene("Menu");
    }
    
}
