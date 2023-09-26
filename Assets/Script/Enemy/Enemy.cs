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
    [TextArea]
    public string dialogueImagePath;//图片路径
    private Sprite dialogueImage;//对话形象

    [System.Serializable]
    public struct KeyWordAndDesc
    {
        public string keyWord;
        public string keyDecription;
    }
    [SerializeField]
    public List<KeyWordAndDesc> keyWordAndDescsList = new List<KeyWordAndDesc>();

    public Sprite HeadImage { get => headImage; }
    public Sprite DialogueImage { get => dialogueImage; }

    #endregion
    protected void Start()
    {
        UpdateEnemyImageAndIcon();
    }

    protected void Update()
    {

    }

    protected void UpdateEnemyImageAndIcon()
    {
        Sprite imageSprite = Resources.Load<Sprite>(headImagePath);
        if (imageSprite != null)
        {
            // 获取物体上的Image组件
            headImage = imageSprite;
        }
        else
        {
            Debug.Log("没有找到路径图片: " + headImagePath);
        }

        Sprite imageSprite2 = Resources.Load<Sprite>(dialogueImagePath);
        if (imageSprite2 != null)
        {
            // 获取物体上的Image组件
            dialogueImage = imageSprite;
        }
        else
        {
            Debug.Log("没有找到路径图片: " + dialogueImagePath);
        }
    }
    public void InitializeEnemy()
    {
        currentHealth = enemyHealth;
    }
    #region 与战斗相关
    private float currentHealth;//现在的血量




    #endregion
}
