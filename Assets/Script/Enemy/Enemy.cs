using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region 基础信息

    public int enemyID;
    public string enemyName;
    public float enemyHealth;
    public GameObject actionHand;//拿枪的手
    [TextArea]
    public string headImagePath;//图片路径
    protected Sprite headImage;//头像
    [Header("图片路径")]
    [TextArea]
    public string dialogueImagePath;//图片路径
    protected Sprite dialogueImage;//对话形象
    [Header("所有和开枪动作有关的图片路径")]
    [TextArea] public string actionImagePath;
    [TextArea] public string readyImagePath;   
    [TextArea] public string dodgeImagePath;
    [TextArea] public string shotImagePath;
    [TextArea] public string dodgeActionImagePath;
    protected Sprite actionImage;//拿枪动作形象
    protected Sprite readyImage;//准备开枪形象
    protected Sprite dodgeImage;//未中枪形象 
    protected Sprite shotImage;//中枪形象
    protected Sprite dodgeActionImage;//没中枪后放枪的形象

    protected SpriteRenderer enemySpriteRenderer;//敌人图像管理
    protected Animator animator;//动画机
    protected BattleSystem battleSystem;//战斗系统
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
    protected virtual void Start()
    {
        InitializeEnemyImageAndIcon();
    }
    protected void InitializeEnemyImageAndIcon()//初始化加载敌人的所有图片
    {
        headImage = Resources.Load<Sprite>(headImagePath);
        dialogueImage = Resources.Load<Sprite>(dialogueImagePath);

        actionImage= Resources.Load<Sprite>(actionImagePath);
        readyImage = Resources.Load<Sprite>(readyImagePath);
        dodgeImage = Resources.Load<Sprite>(dodgeImagePath);
        shotImage = Resources.Load<Sprite>(shotImagePath);
        dodgeActionImage = Resources.Load<Sprite>(dodgeActionImagePath);
    }
    public void InitializeEnemy()
    {
        currentHealth = enemyHealth;
    }

    #region 与战斗相关
    protected float currentHealth;//现在的血量
    protected bool if_Immute=false;//是否免疫
    public void EnemyGetHurt(float damage,int bulletIndex)//敌人受到伤害
    {
        if (if_Immute && bulletIndex == 0)
        {
            if_Immute = false;
            return;
        }
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }
    
    public virtual IEnumerator EnemyShooting()//敌人开枪
    {
        //敌人拿枪
        yield return new WaitForSeconds(0.5f);//等待0.5秒
        enemySpriteRenderer.sprite = actionImage;
        actionHand.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        actionHand.SetActive(false);
        enemySpriteRenderer.sprite = readyImage;
        //准备开枪
        yield return new WaitForSeconds(0.5f);
        if (battleSystem.JudegeShoot())
        {
            enemySpriteRenderer.sprite = shotImage;//中枪
            yield return new WaitForSeconds(0.5f);
            enemySpriteRenderer.sprite = readyImage;
            yield return new WaitForSeconds(0.5f);
            enemySpriteRenderer.sprite = actionImage;
            actionHand.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            enemySpriteRenderer.sprite = dialogueImage;
            battleSystem.StartShoot();
            StopAllCoroutines();
        }
        else
        {
            enemySpriteRenderer.sprite = dodgeImage;//没中枪
            yield return new WaitForSeconds(0.5f);
            enemySpriteRenderer.sprite = readyImage;
            yield return new WaitForSeconds(0.5f);
            enemySpriteRenderer.sprite = dodgeActionImage;
            actionHand.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            enemySpriteRenderer.sprite = dialogueImage;
            battleSystem.StartShoot();
            StopAllCoroutines();
        }
        
    }

    #endregion
}
