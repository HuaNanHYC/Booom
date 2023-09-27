using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region ������Ϣ

    public int enemyID;
    public string enemyName;
    public float enemyHealth;
    [TextArea]
    public string headImagePath;//ͼƬ·��
    private Sprite headImage;//ͷ��
    [TextArea]
    public string dialogueImagePath;//ͼƬ·��
    private Sprite dialogueImage;//�Ի�����
    private Animator animator;//������
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
    public float CurrentHealth { get => currentHealth; set => currentHealth = value; }

    #endregion
    protected void Awake()
    {
        animator = GetComponent<Animator>();
        InitializeEnemy();
    }
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
            // ��ȡ�����ϵ�Image���
            headImage = imageSprite;
        }
        else
        {
            Debug.Log("û���ҵ�·��ͼƬ: " + headImagePath);
        }

        Sprite imageSprite2 = Resources.Load<Sprite>(dialogueImagePath);
        if (imageSprite2 != null)
        {
            // ��ȡ�����ϵ�Image���
            dialogueImage = imageSprite;
        }
        else
        {
            Debug.Log("û���ҵ�·��ͼƬ: " + dialogueImagePath);
        }
    }
    public void InitializeEnemy()
    {
        currentHealth = enemyHealth;
    }

    #region ��ս�����
    private float currentHealth;//���ڵ�Ѫ��
    public void EnemyGetHurt(float damage)//�����ܵ��˺�
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        if(currentHealth <= 0)
        {
            //��Ϸʤ������ת

        }
    }


    #endregion
}