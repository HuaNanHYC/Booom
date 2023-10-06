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
    [TextArea] public string shotImagePath;
    public SpriteRenderer shootText;//开枪或空枪的文字，用子物体的赋予
    public Sprite dodgeActionText;//空枪嘲讽
    public Sprite dodgeText;//空枪字
    public Sprite shotText;//中枪字
    protected Sprite actionImage;//拿枪动作形象
    protected Sprite readyImage;//准备开枪形象
    protected Sprite shotImage;//中枪形象

    protected SpriteRenderer enemySpriteRenderer;//敌人图像管理
    protected Animator animator;//动画机
    protected BattleSystem battleSystem;//战斗系统
    protected GameObject gunSprite;
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
        gunSprite = GameObject.FindWithTag("Gun");
        InitializeEnemyImageAndIcon();
    }
    public void InitializeEnemyImageAndIcon()//初始化加载敌人的所有图片
    {
        headImage = Resources.Load<Sprite>(headImagePath);
        dialogueImage = Resources.Load<Sprite>(dialogueImagePath);

        actionImage= Resources.Load<Sprite>(actionImagePath);
        readyImage = Resources.Load<Sprite>(readyImagePath);
        shotImage = Resources.Load<Sprite>(shotImagePath);
    }
    public void InitializeEnemy()
    {
        currentHealth = enemyHealth;
    }

    #region 与战斗相关
    protected float currentHealth;//现在的血量
    protected bool if_Immute=false;//是否免疫
    public void EnemyGetHurt(float damage)//敌人受到伤害
    {
        if (if_Immute && damage != 0)
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_MissFire);
            if_Immute = false;
            return;
        }
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        /*if(damage == 0 && battleSystem.bullets[battleSystem.BulletIndexShoot].ID==10003)//旧子弹判定
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_MissFire);
            return;
        }*/
        if (damage > 0)
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_Fire);//射中声音
        }
        else
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_NoBullet);//空弹声音
        }
    }
    
    public virtual IEnumerator EnemyShooting()//敌人开枪
    {
        //敌人拿枪
        yield return new WaitForSeconds(0.5f);//等待0.5秒
        EnemyAction(true);
        yield return new WaitForSeconds(0.5f);
        EnemyReady(false);
        
        //准备开枪
        yield return new WaitForSeconds(0.5f);
        if (battleSystem.JudegeShoot())
        {
            EnemyShot();//中枪
            yield return new WaitForSeconds(0.5f);
            EnemyReady(false);
            yield return new WaitForSeconds(0.5f);
            EnemyAction(true);
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.StartShoot();
            StopAllCoroutines();
        }
        else
        {
            EnemyDodge();//没中枪
            yield return new WaitForSeconds(0.5f);
            EnemyReady(false);
            yield return new WaitForSeconds(0.5f);
            EnemyDodgeAction();
            actionHand.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.StartShoot();
            StopAllCoroutines();
        }

    }

    protected void EnemyDodgeAction()
    {
        enemySpriteRenderer.sprite = actionImage;
        shootText.sprite = dodgeActionText;
    }

    protected void EnemyDodge()
    {
        enemySpriteRenderer.sprite = readyImage;
        shootText.sprite = dodgeText;//咔哒的文字
    }

    protected void EnemyIdle()
    {
        gunSprite.SetActive(true);
        enemySpriteRenderer.sprite = dialogueImage;
        shootText.sprite = null;
    }

    protected void EnemyShot()
    {
        enemySpriteRenderer.sprite = shotImage;
        shootText.sprite = shotText;
    }

    protected void EnemyReady(bool hand)
    {
        enemySpriteRenderer.sprite = readyImage;
        actionHand.SetActive(hand);
        shootText.sprite = null;
    }

    protected void EnemyAction(bool hand)
    {
        gunSprite.SetActive(false);
        enemySpriteRenderer.sprite = actionImage;
        actionHand.SetActive(hand);
        shootText.sprite = null;
    }


    #endregion
}
