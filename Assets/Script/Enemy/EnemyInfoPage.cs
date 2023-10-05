using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyInfoPage : MonoBehaviour
{
    private string enemyName;
    private float enemyHealth;//最大生命
    private float currentHealth;//现在生命
    private Sprite headImage;//头像

    public Text enemyNameText;
    //public Slider enemyHealthSlide;
    public Image enemyHeadImage;
    public Text enemyHealthText;
    public Transform healthManage;//血量图片生成管理的父物体
    public GameObject healthImagePrefab;//血量图片

    private Enemy enemy;

    private List<Enemy.KeyWordAndDesc> keyWordAndDescsList = new List<Enemy.KeyWordAndDesc>();
    public GameObject keywordPrefab;//用来显示关键词的预制体
    private void Start()
    {
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
        enemy.InitializeEnemyImageAndIcon();
        InitializePage();
    }
    private void Update()
    {
        UpdateEnemyInfoPage();
    }
    public void InitializePage()//敌人数据初始化
    {
        enemyName = enemy.enemyName;
        headImage = enemy.HeadImage;
        enemyHealth = enemy.enemyHealth;

        enemyNameText.text = enemyName;
        if (enemy.HeadImage != null)
            enemyHeadImage.sprite = enemy.HeadImage;
        keyWordAndDescsList = enemy.keyWordAndDescsList;

        //enemyHealthSlide.maxValue = enemyHealth;

        ShowTheKeyWords();
        InstantiateBloodImage();//生成血量图片
    }
    public void UpdateEnemyInfoPage()//实时更新敌人血量等数据
    {
        currentHealth = enemy.CurrentHealth;
        //enemyHealthSlide.value = currentHealth;
        enemyHealthText.text = currentHealth.ToString() + "/" + enemyHealth.ToString();

        float falseBlood = enemyHealth - currentHealth;
        for(int i = healthManage.childCount-1; i > currentHealth-1; i--)
        {
            if(healthManage.GetChild(i) != null)
            {
                if (healthManage.GetChild(i).GetChild(0).gameObject.activeSelf)
                    healthManage.GetChild(i).GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    public void ShowTheKeyWords()
    {
        for (int i = 0; i < keyWordAndDescsList.Count; i++)
        {
            keywordPrefab.GetComponentInChildren<Text>().text = keyWordAndDescsList[i].keyWord;
            keywordPrefab.GetComponent<EnemyKeywordShow>().Description = keyWordAndDescsList[i].keyDecription;
            keywordPrefab.GetComponent<EnemyKeywordShow>().keywordDescriptionShow.transform.GetChild(0).GetComponent<Text>().text = keyWordAndDescsList[0].keyWord;
        }
    }
    public void InstantiateBloodImage()
    {
        for(int i=0;i<enemyHealth;i++)
        {
            Instantiate(healthImagePrefab, healthManage);
        }
    }
}
