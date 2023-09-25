using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    [Header("子弹读取顺序的index")]
    private int bulletIndex;

    public bool if_BattleStart;//是否开始战斗

    [Header("战斗页面")]
    public GameObject battlePage;//用于抖动之类的吧。。暂时没用

    private void Update()
    {
        if (BulletManager.Instance.loadedBulletDictionary.Count>0) if_BattleStart = true;
    }
    public void FirstRamdom()//第一次随机子弹
    {

    }


}
