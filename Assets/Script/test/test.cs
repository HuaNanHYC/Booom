using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public void getbullet()
    {
        InventoryManager.Instance.AddBullet(1001);
        InventoryManager.Instance.AddBullet(1002); 
        InventoryManager.Instance.AddBullet(1003);
    }
}
