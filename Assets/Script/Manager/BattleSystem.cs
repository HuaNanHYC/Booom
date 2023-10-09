using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public bool if_TheLastLevel;
    [SerializeField]
    [Header("子弹读取顺序的index（无需设置）")]
    private int bulletIndexShoot;//射击时的index
    private int bulletIndexBeforeShoot;//射击前排队列的index

    public bool if_BattleCanStart;//是否开始战斗
    public bool if_ShootStart;//是否可以开始射击，这是在子弹队列真正排列完毕之后
    public int BulletIndexShoot { get => bulletIndexShoot; set => bulletIndexShoot = value; }
    public int BulletIndexBeforeShoot { get => bulletIndexBeforeShoot; set => bulletIndexBeforeShoot = value; }
    [Header("目前敌人(无需设置)")]
    public Enemy currentEnemy;
    [Header("战斗主页面")]
    public GameObject battlePage;//用于控制左轮旋转
    [Header("开枪按钮")]
    public Button playerShootButton;//到玩家时，让玩家点击射击开枪的按钮
    [Header("结束页面")]
    public GameEndPage endPage;//游戏结束页面
    public PlayerInfoPage playerInfoPage;
    public EnemyInfoPage enemyInfoPage;
    private void Start()
    {
        bulletIndexBeforeShoot = 0;
        currentEnemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();//找到本场景敌人
        currentEnemy.GetComponent<Enemy>().BattleSystem = this;

        if (LevelManager.Instance.currentLevelId == 30008) LevelManager.Instance.lastLevelJudge = true;
        else LevelManager.Instance.lastLevelJudge = false;
    }
    private void Update()
    {
        if (BulletManager.Instance.loadedBulletList.Count > 0 && !if_BattleCanStart) if_BattleCanStart = true;//如果没开始战斗且满足条件，设置开始战斗
    }
    public List<Bullet> bullets=new List<Bullet>();//射击子弹队列，设置成公有方便查看
    public void StartTheBattle()//赋给开始按钮
    {
        //初始化三个队列
        InitializeOddList();
        InitializeEvenList();
        InitializePrimeList();
        //*********
        AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_Spin);
        RandomBulletQueue();
        JudgeListEnable();
    }
   
    public void RandomBulletQueue()//随机选取子弹
    {
        //最后一关第一次必失败
        if(LevelManager.Instance.lastLevelJudge)
        {
            LevelManager.Instance.lastLevelJudge = false;
            bullets.Clear();
            for (int i = 0; i < BulletManager.Instance.loadedBulletList.Count; i++)
            { 
                bullets.Add(BulletManager.Instance.loadedBulletList[5]); 
            }
            Bullet newBullet = BulletManager.Instance.bulletDictionary[10001].GetComponent<Bullet>();
            if (if_PlayerShoot)
            {
                int random = Random.Range(1, 91);
                if (random <= 30) bullets[0] = newBullet;
                else if (random > 30 && random <= 60) bullets[2] = newBullet;
                else if (random > 60) bullets[4]= newBullet;
            }
            else if (!if_PlayerShoot)
            {
                int random = Random.Range(1, 91);
                if (random <= 30) bullets[1] = newBullet;
                else if (random > 30 && random <= 60) bullets[3] = newBullet;
                else if (random > 60) bullets[5] = newBullet;
            }
            return;
        }



        bullets.Clear();//清除所有，然后再次随机
        //随机到了一个子弹，并将其作为第一个
        int listLength = BulletManager.Instance.loadedBulletList.Count;
        int index = Random.Range(0, listLength);
        for (int i = index; i < listLength * 2; i++)
        {
            bullets.Add(BulletManager.Instance.loadedBulletList[i%listLength]);
            if (i % listLength == index - 1) break;//轮到同个元素的前一个就停下
        }
        if(bullets.Count > BulletManager.Instance.loadedBulletList.Count)//保证序列只有8个，因为有时会出16个的bug
        {
            bullets.RemoveRange(BulletManager.Instance.loadedBulletList.Count, bullets.Count - BulletManager.Instance.loadedBulletList.Count);
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
        bulletIndexShoot = 0;
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

                #region 奇数弹
                case 10005:
                    OddBullet();
                    break;
                #endregion

                #region 偶数弹
                case 10006:
                    EvenBullet();
                    break;
                #endregion

                #region 质数弹
                case 10007:
                    PrimeBullet();
                    break;
                #endregion

                default:
                    bulletIndexBeforeShoot++;
                    break;
            }
        }
        if_ShootStart = true;
        StartCoroutine(StartShootAfterSpin());
    }
    public void StartShoot()//开始射击后玩家和敌人的动作表现
    {
        if (ShootEnd()) return;
        JudgeWhoShootImage(if_PlayerShoot, !if_PlayerShoot);//回合图片显示
        if (if_PlayerShoot)
        {
            playerShootButton.gameObject.SetActive(true);
            //点击按钮后播放开枪动画
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(currentEnemy.EnemyShooting());
        }
    }

    [Header("判断这是谁先开枪（无需设置）")]
    public bool if_PlayerShoot;
    public void SetIfPlayerFirst(bool setting)//赋给按钮让玩家决定先后开枪顺序
    {
        if_PlayerShoot = setting;
    }
    public bool JudegeShoot()//开始射击，判断子弹技能,返回是否空枪的判断
    {
        if (bulletIndexShoot >= bullets.Count) StartShoot();
        //判断子弹

        #region 空包弹
        if (bullets[bulletIndexShoot].ID == 10000 && bullets[bulletIndexShoot].actualDamage == 0)
        {
            Shoot();
            bulletIndexShoot++;
            return false;
        }
        #endregion

        #region 旧子弹判断
        if (!if_haveJudgedFirstShootFailed && bullets[bulletIndexShoot].ID==10003)
        {
            if_PlayerShoot = !if_PlayerShoot;
            if_haveJudgedFirstShootFailed = true;
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_MissFire);
            return false;
        }
        else
            if_haveJudgedFirstShootFailed = false;
        #endregion

        #region 连发弹判断
        if (ShootTwice())
        {
            return true;
        }
        #endregion

        #region 其他子弹，直接射出并轮盘
        Shoot();
        bulletIndexShoot++;
        return true;
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
            Debug.Log($"玩家射击1次{bullets[BulletIndexShoot]}");
            return;
        }
        else if (!if_PlayerShoot)
        {
            currentEnemy.EnemyGetHurt(damage);
            if_PlayerShoot = true;
            Debug.Log($"敌人射击1次{bullets[BulletIndexShoot]}");
            return;
        }
    }
    public bool ShootEnd()//射击结束
    {
        if (InventoryManager.Instance.playerCurrentHealth <= 0)
        {
            endPage.gameObject.SetActive(true);
            endPage.Lose();
            return true;
        }
        else if (currentEnemy.CurrentHealth <= 0)
        {
            endPage.gameObject.SetActive(true);
            endPage.Win();
            return true;
        }
        if (bulletIndexShoot>bullets.Count-1)
        {
            if_haveJudgedFirstShootFailed = false;//第一次射击空枪判断恢复
            bulletIndexShoot = 0;
            //结束的代码
            endPage.gameObject.SetActive(true);
            endPage.Lose();
            return true;
        }
        
        return false;
    }
    IEnumerator StartShootAfterSpin()//左轮声音播完后开始的判断
    {
        battlePage.GetComponent<BattlePage>().SetRevolverBulletRotate(true);//左轮旋转画面
        while(AudioManager.Instance.AudioSource2EffectSource.isPlaying)
        {
            yield return null;
        }
        battlePage.GetComponent<BattlePage>().SetRevolverBulletRotate(false);
        StartShoot();
    }
    public void JudgeWhoShootImage(bool player,bool enemy)//谁射击的图片设置
    {
        playerInfoPage.huiHe.SetActive(player);
        enemyInfoPage.huiHe.SetActive(enemy);
    }

    #region 会影响队列的子弹技能
    public void NotTheFirst()//社恐弹
    {
        RandomBulletQueue();//那就重新排序
    }
    public void OddBullet()//奇数弹
    {
        if (oddList.Contains(bullets[0]))
        {
            RandomBulletQueue();
            JudgeListEnable();
        }
        else bulletIndexBeforeShoot++;
    }
    public void EvenBullet()//偶数弹
    {
        if (evenList.Contains(bullets[0]))
        {
            RandomBulletQueue();
            JudgeListEnable();
        }
        else bulletIndexBeforeShoot++;
    }
    public void PrimeBullet()//质数弹
    {
        if (primeList.Contains(bullets[0]))
        {
            RandomBulletQueue();
            JudgeListEnable();
        }
        else bulletIndexBeforeShoot++;
    }

    //用于以上子弹的队列在此：
    public List<Bullet> oddList = new List<Bullet>();
    public List<Bullet> evenList = new List<Bullet>();
    public List<Bullet> primeList = new List<Bullet>();
    //以上列表初始化
    void InitializeOddList()
    {
        oddList.Clear();
        for(int i=0;i<BulletManager.Instance.loadedBulletList.Count;i+=2)//将队列的奇数子弹装进来
        {
            oddList.Add(BulletManager.Instance.loadedBulletList[i]);
        }
    }
    void InitializeEvenList()
    {
        evenList.Clear();
        for (int i = 1; i < BulletManager.Instance.loadedBulletList.Count; i += 2)//将队列的偶数子弹装进来
        {
            evenList.Add(BulletManager.Instance.loadedBulletList[i]);
        }
    }
    void InitializePrimeList()
    {
        primeList.Clear();
        for (int i = 1; i < BulletManager.Instance.loadedBulletList.Count; i += 1)//将队列的质数子弹装进来
        {
            if (i == 1 || i == 2 || i == 4 || i == 6)
                primeList.Add(BulletManager.Instance.loadedBulletList[i]);
        }
    }
    #endregion

    #region 不影响队列的子弹技能
    public bool if_haveJudgedFirstShootFailed;//判断是否已经判定了第一次射击失效
    /*public bool TheFirstShootFailed()//旧子弹
    {
        if(bullets.Find(x=>x.ID==10003))
        {
            if_haveJudgedFirstShootFailed = true;
            return true;
        }
        return false;
    }*/
    public bool ShootTwice()//连发弹
    {
        if (bullets[bulletIndexShoot].ID==10004)
        {
            bullets[bulletIndexShoot].actualDamage = 0;
            Shoot();
            bulletIndexShoot++;
            if_PlayerShoot = !if_PlayerShoot;//还是同一个人射
            JudegeShoot();
            return true;
        }
        return false;
    }
    #endregion
}
