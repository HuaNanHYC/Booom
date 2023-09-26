using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
