using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    [Header("子弹读取顺序的index（无需设置）")]
    private int bulletIndexShoot;//射击时的index
    private int bulletIndexBeforeShoot;//射击前排队列的index

    public bool if_BattleCanStart;//是否开始战斗
    public bool if_ShootStart;//是否可以开始射击，这是在子弹队列真正排列完毕之后
    [Header("目前敌人(无需设置)")]
    public Enemy currentEnemy;
    [Header("战斗页面")]
    public GameObject battlePage;//用于抖动之类的吧。。暂时没用
    public Button playerShootButton;//到玩家时，让玩家点击射击开枪的按钮
    private void Start()
    {
        bulletIndexBeforeShoot = 0;
        currentEnemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();//找到本场景敌人
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
        if(bullets.Count > 8)//保证序列只有8个，因为有时会出16个的bug
        {
            bullets.RemoveRange(8, bullets.Count);
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
        StartShootAnim();
    }
    
    public void StartShootAnim()//开始射击后玩家和敌人的动作表现
    {
        if (ShootEnd()) return;
        if (if_PlayerShoot)
        {
            playerShootButton.gameObject.SetActive(true);
            //点击按钮后播放开枪动画
        }
        else
        {
            //敌人自动拿枪开枪的动画
            JudegeShoot();
        }
    }

    [Header("判断这是谁先开枪（无需设置）")]
    public bool if_PlayerShoot;
    public void SetIfPlayerFirst(bool setting)//赋给按钮让玩家决定先后开枪顺序
    {
        if_PlayerShoot = setting;
    }
    public void JudegeShoot()//开始射击，判断子弹技能
    {
        //判断子弹

        #region 旧子弹判断
        if (!if_haveJudgedFirstShootFailed)
        {
            if (TheFirstShootFailed())
            {
                if_PlayerShoot = !if_PlayerShoot;
                StartShootAnim();//再次开始射击判断
                return;
            }
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
        //造成伤害
        float damage = bullets[bulletIndexShoot].actualDamage;
        if (if_PlayerShoot)
        {
            InventoryManager.Instance.PlayerGetHurt(damage);
            if_PlayerShoot = false;
            return;
        }
        else if (!if_PlayerShoot)
        {
            currentEnemy.EnemyGetHurt(damage);
            if_PlayerShoot = true;
            return;
        }

    }
    public bool ShootEnd()
    {
        if(bulletIndexShoot>bullets.Count-1)
        {
            if_haveJudgedFirstShootFailed = false;//第一次射击空枪判断恢复
            bulletIndexShoot = 0;
            //结束的代码







            return true;
        }
        return false;
    }
    #region 会影响队列的子弹技能

    public void NotTheFirst()//社恐弹
    {
        RandomBulletQueue();//那就重新排序
    }


    #endregion

    #region 不影响队列的子弹技能
    public bool if_haveJudgedFirstShootFailed;//判断是否已经判定了第一次射击失效
    public bool TheFirstShootFailed()//旧子弹
    {
        if(bullets.Find(x=>x.ID==10003))
        {
            if_haveJudgedFirstShootFailed = true;
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
            if_PlayerShoot = !if_PlayerShoot;//还是同一个人射
            Shoot();
            bulletIndexShoot++;
            return true;
        }
        return false;
    }
    #endregion
}
