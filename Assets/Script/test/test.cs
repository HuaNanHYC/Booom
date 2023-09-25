using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public void getbullet()
    {
        InventoryManager.Instance.AddBullet(BulletType.NormalBullet);
        InventoryManager.Instance.AddBullet(BulletType.NullBullet);
    }
}
