using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class test : MonoBehaviour
{
    public void getbullet()
    {
        InventoryManager.Instance.AddBullet(10001);
        InventoryManager.Instance.AddBullet(10002); 
        InventoryManager.Instance.AddBullet(10003);
        InventoryManager.Instance.AddBullet(10004);
        InventoryManager.Instance.SavePlayerData();
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
