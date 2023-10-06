using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPage : MonoBehaviour
{
    //public Text playerNameText;
    public Text currentHealthText;
    public GameObject bloodImage;
    public GameObject huiHe;
    //public Slider currentHealthSlide;
    //public Image playerHeadImage;
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
        //if (InventoryManager.Instance.PlayerHeadImage) 
            //playerHeadImage.sprite = InventoryManager.Instance.PlayerHeadImage;//…Ë÷√ÕÊº“Õ∑œÒ
        //playerNameText.text = InventoryManager.Instance.playerName;
        //currentHealthSlide.maxValue = InventoryManager.playerMaxHealth;
    }
    public void UpdatePlayerInfoPage()
    {
        string health = InventoryManager.Instance.playerCurrentHealth.ToString();
        string maxHealth=InventoryManager.playerMaxHealth.ToString();
        //currentHealthSlide.value = InventoryManager.Instance.playerCurrentHealth;
        currentHealthText.text = health + "/" + maxHealth;
        if(InventoryManager.Instance.playerCurrentHealth==0)
        {
            if (bloodImage.transform.GetChild(0) != null) bloodImage.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
