using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    [Header("子弹读取顺序的index")]
    private int bulletIndex;

    public bool if_BattleCanStart;//是否开始战斗
    public bool if_ShootStart;//是否可以开始射击，这是在子弹队列真正排列完毕之后

    [Header("战斗页面")]
    public GameObject battlePage;//用于抖动之类的吧。。暂时没用
    private void Start()
    {
        bulletIndex = 0;
    }
    private void Update()
    {
        if (BulletManager.Instance.loadedBulletList.Count > 0 && !if_BattleCanStart) if_BattleCanStart = true;//如果没开始战斗且满足条件，设置开始战斗
    }
    public List<Bullet> bullets=new List<Bullet>();//射击子弹队列，设置成公有方便查看
    public void StartTheBattle()//可以赋值给按钮
    {
        RandomBulletQueue();
        JudgeListEnable();
    }
    public void RandomBulletQueue()//随机选取子弹
    {
        bullets.Clear();//清除所有，然后再次随机
        //随机到了一个子弹，并将其作为第一个
        int listLength = BulletManager.Instance.loadedBulletList.Count;
        int index = Random.Range(0, listLength);
        for (int i = index; i < listLength * 2; i++)
        {
            bullets.Add(BulletManager.Instance.loadedBulletList[i%listLength]);
            if (i % listLength == index - 1) break;//轮到同个元素的前一个就停下
        }
        InitializeAllBullets();//清零已设置的子弹属性

    }
    public void InitializeAllBullets()//让所有子弹伤害变回来之类的
    {
        if (bullets == null) return;
        for(int i=0; i<bullets.Count; i++)
        {
            bullets[i].InitializeBullet();
        }
        bulletIndex = 0;//索引也要清零一次
    }
    public void JudgeListEnable()//判断队列是否可以作为战斗队列开启
    {
        while(bulletIndex < bullets.Count)
        {
            switch(bulletIndex)
            {
                #region 非第一颗子弹的判定
                case 1002:
                    if (bulletIndex != 0)
                    {
                        bulletIndex++; 
                        break;
                    }
                    NotTheFirst();
                    JudgeListEnable();
                    break;
                #endregion

                #region 下一颗子弹伤害翻倍的判断
                case 1003:
                    DoubleTheNextBullet();
                    break;

                #endregion







                default:
                    bulletIndex++;
                    break;
            }
        }
        StartShoot();
    }
    public void StartShoot()//可以开始射击
    {
        if_ShootStart = true;

    }


    #region 子弹技能

    public void NotTheFirst()//不能作为第一个
    {
        RandomBulletQueue();//那就重新排序
    }
    private void DoubleTheNextBullet()//下一颗子弹翻倍伤害
    {
        bullets[bulletIndex + 1].actualDamege *= 2;
    }


    #endregion
}
