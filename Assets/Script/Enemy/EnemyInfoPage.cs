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
    public Slider enemyHealthSlide;
    public Image enemyHeadImage;
    public Text enemyHealthText;

    private Enemy enemy;

    private List<Enemy.KeyWordAndDesc> keyWordAndDescsList = new List<Enemy.KeyWordAndDesc>();
    public Text keyWordsText;
    private void Start()
    {
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
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

        enemyHealthSlide.maxValue = enemyHealth;

        ShowTheKeyWords();
    }
    public void UpdateEnemyInfoPage()//实时更新敌人血量等数据
    {
        currentHealth = enemy.CurrentHealth;
        enemyHealthSlide.value = currentHealth;
        enemyHealthText.text = currentHealth.ToString() + "/" + enemyHealth.ToString();
    }
    public void ShowTheKeyWords()
    {
        for(int i=0;i<keyWordAndDescsList.Count;i++)
        {
            keyWordsText.text += keyWordAndDescsList[i].keyWord + "\t";
        }
    }
}
