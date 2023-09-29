using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 基础信息

    public int enemyID;
    public string enemyName;
    public float enemyHealth;
    [TextArea]
    public string headImagePath;//图片路径
    private Sprite headImage;//头像
    [Header("图片路径")]
    [TextArea]
    public string dialogueImagePath;//图片路径
    private Sprite dialogueImage;//对话形象
    [Header("所有和开枪动作有关的图片路径")]
    [TextArea] public string actionImagePath;
    [TextArea] public string readyImagePath;   
    [TextArea] public string dodgeImagePath;
    [TextArea] public string shotImagePath;
    private Sprite actionImage;//拿枪动作形象
    private Sprite readyImage;//准备开枪形象
    private Sprite dodgeImage;//未中枪形象 
    private Sprite shotImage;//中枪形象

    private SpriteRenderer enemySpriteRenderer;//敌人图像管理
    private Animator animator;//动画机
    private BattleSystem battleSystem;//战斗系统
    [System.Serializable]
    public struct KeyWordAndDesc
    {
        public string keyWord;
        [TextArea]
        public string keyDecription;
    }
    [SerializeField]
    public List<KeyWordAndDesc> keyWordAndDescsList = new List<KeyWordAndDesc>();

    public Sprite HeadImage { get => headImage; }
    public Sprite DialogueImage { get => dialogueImage; }
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public BattleSystem BattleSystem { get => battleSystem; set => battleSystem = value; }

    #endregion
    protected void Awake()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        InitializeEnemy();
    }
    protected void Start()
    {
        InitializeEnemyImageAndIcon();
    }

    protected void Update()
    {

    }

    protected void InitializeEnemyImageAndIcon()//初始化加载敌人的所有图片
    {
        headImage = Resources.Load<Sprite>(headImagePath);
        dialogueImage = Resources.Load<Sprite>(dialogueImagePath);

        actionImage= Resources.Load<Sprite>(actionImagePath);
        readyImage = Resources.Load<Sprite>(readyImagePath);
        dodgeImage = Resources.Load<Sprite>(dodgeImagePath);
        shotImage = Resources.Load<Sprite>(shotImagePath);
    }
    public void InitializeEnemy()
    {
        currentHealth = enemyHealth;
    }

    #region 与战斗相关
    private float currentHealth;//现在的血量
    public void EnemyGetHurt(float damage)//敌人受到伤害
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }
    public IEnumerator EnemyShooting()//敌人开枪
    {
        //敌人拿枪
        yield return new WaitForSeconds(0.5f);//等待0.5秒
        enemySpriteRenderer.sprite = actionImage;
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = readyImage;
        //准备开枪
        yield return new WaitForSeconds(0.5f);
        if (battleSystem.JudegeShoot())
        {
            enemySpriteRenderer.sprite = shotImage;//中枪
            //子弹爆炸的图片
        }
        else enemySpriteRenderer.sprite = dodgeImage;//没中枪
        //回到初始装态，敌人把枪放回
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = readyImage;
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = actionImage;
        battleSystem.StartShoot();
    }

    #endregion
}
