using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePage : MonoBehaviour
{
    public BattleSystem battleSystem;
    public Button startBattle;//开始的按钮

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JudgeCanBattle();
    }
    public void JudgeCanBattle()//判断是否可以开始战斗，显示开始按钮
    {
        if(battleSystem.if_BattleStart)
        {
            startBattle.gameObject.SetActive(true);
        }
        else startBattle.gameObject.SetActive(false);
    }

}
