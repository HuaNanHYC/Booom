using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePage : MonoBehaviour
{
    public BattleSystem battleSystem;
    public GameObject startBattle;//开始的按钮
    [Header("开始后的左轮盘显示")]
    public GameObject revolverPage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JudgeCanBattle();
        //JudgeRevolverBulletRotate();
    }
    bool if_show_StartButton=false;//是否显示开始按钮
    public void JudgeCanBattle()//判断是否可以开始战斗，显示开始按钮
    {
        if (LevelManager.Instance.currentLevelId == 30008 && !if_show_StartButton)
        {
            startBattle.gameObject.SetActive(true);
            if_show_StartButton = true;
        }
        if(battleSystem.if_BattleCanStart&& !if_show_StartButton)//只让它显示一次
        {
            startBattle.gameObject.SetActive(true);
            if_show_StartButton = true;
        }
    }
    public void JudgeRevolverBulletRotate()//判断是否旋转左轮,10.7弃用
    {
        if(battleSystem.if_BattleCanStart&&!battleSystem.if_ShootStart)
        {
            //放左轮旋转动画
        }
        else if(battleSystem.if_ShootStart)//旋转结束，开始射击
        {
            //这里放左轮停止旋转的动画

            return;
        }
    }
}
