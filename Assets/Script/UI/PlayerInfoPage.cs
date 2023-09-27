using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPage : MonoBehaviour
{
    public Text playerNameText;
    public Text currentHealthText;
    public Slider currentHealthSlide;
    public Image playerHeadImage;
    private void Start()
    {
        InitializePlayerInfoPage();
    }
    private void Update()
    {
        UpdatePlayerInfoPage();
    }
    public void InitializePlayerInfoPage()
    {
        if (InventoryManager.Instance.PlayerHeadImage)
            playerHeadImage.sprite = InventoryManager.Instance.PlayerHeadImage;//…Ë÷√ÕÊº“Õ∑œÒ
        playerNameText.text = InventoryManager.Instance.playerName;
        currentHealthSlide.maxValue = InventoryManager.Instance.playerMaxHealth;
    }
    public void UpdatePlayerInfoPage()
    {
        string health = InventoryManager.Instance.playerCurrentHealth.ToString();
        string maxHealth=InventoryManager.Instance.playerMaxHealth.ToString();
        currentHealthSlide.value = InventoryManager.Instance.playerCurrentHealth;
        currentHealthText.text = health + "/" + maxHealth;
    }
}
