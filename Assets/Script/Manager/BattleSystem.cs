using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    [Header("子弹读取顺序的index")]
    private int bulletIndexShoot;//射击时的index
    private int bulletIndexBeforeShoot;//射击前排队列的index

    public bool if_BattleCanStart;//是否开始战斗
    public bool if_ShootStart;//是否可以开始射击，这是在子弹队列真正排列完毕之后

    [Header("战斗页面")]
    public GameObject battlePage;//用于抖动之类的吧。。暂时没用
    private void Start()
    {
        bulletIndexBeforeShoot = 0;
    }
    private void Update()
    {
        if (BulletManager.Instance.loadedBulletList.Count > 0 && !if_BattleCanStart) if_BattleCanStart = true;//如果没开始战斗且满足条件，设置开始战斗
    }
    public List<Bullet> bullets=new List<Bullet>();//射击子弹队列，设置成公有方便查看
    public void StartTheBattle()//赋给开始按钮
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
        bulletIndexBeforeShoot = 0;//索引也要清零一次
    }
    public void JudgeListEnable()//判断队列是否可以作为战斗队列开启，主要放能影响队列的子弹
    {
        while(bulletIndexBeforeShoot < bullets.Count)
        {
            switch(bullets[bulletIndexBeforeShoot].ID)
            {
                #region 社恐弹 
                case 10002:
                    if (bulletIndexBeforeShoot != 0)
                    {
                        bulletIndexBeforeShoot++; 
                        break;
                    }
                    NotTheFirst();
                    JudgeListEnable();
                    break;
                #endregion


                default:
                    bulletIndexBeforeShoot++;
                    break;
            }
        }
        if_ShootStart = true;
    }
    public void StartShootAnim()//开始射击后玩家和敌人的动作表现
    {
        if (if_PlayerShoot)
        {
            //让玩家点击开枪才开枪
        }
        else
        {
            //敌人自动拿枪开枪的动画
            JudegeShoot();
        }
    }

    [Header("判断这是谁先开枪")]
    public bool if_PlayerShoot;
    public void JudegeShoot()//开始射击，判断子弹技能
    {
        if(bulletIndexShoot>bullets.Count-1)
        {
            bulletIndexShoot = 0;
            //结束的代码
            return;
        }
        //判断子弹

        #region 旧子弹判断
        if (TheFirstShootFailed()&&bulletIndexShoot==0)
        {
            if_PlayerShoot = !if_PlayerShoot;
            StartShootAnim();//再次开始射击判断
            return;
        }
        #endregion

        #region 连发弹判断
        if (ShootTwice())
        {
            StartShootAnim();
            return;
        }
        #endregion

        #region 普通子弹判断
        Shoot();
        bulletIndexShoot++;
        StartShootAnim();
        #endregion
    }
    public void Shoot()//根据目前谁射击判断子弹对谁造成伤害
    {
        if (bulletIndexShoot > bullets.Count) return;
        //造成伤害
        if (if_PlayerShoot)
        {
            InventoryManager.Instance.PlayerGetHurt(bullets[bulletIndexShoot].actualDamege);
            return;
        }
        else if (!if_PlayerShoot)
        {
            //对敌人造成伤害
            return;
        }

    }

    #region 会影响队列的子弹技能

    public void NotTheFirst()//社恐弹
    {
        RandomBulletQueue();//那就重新排序
    }


    #endregion

    #region 不影响队列的子弹技能
    public bool TheFirstShootFailed()//旧子弹
    {
        if(bullets.Find(x=>x.ID==1003))
        {
            return true;
        }
        return false;
    }
    public bool ShootTwice()//连发弹
    {
        if (bullets[bulletIndexShoot].ID==10004)
        {
            Shoot();
            bulletIndexShoot++;
            Shoot();
            bulletIndexShoot++;
            return true;
        }
        return false;
    }
    #endregion
}
