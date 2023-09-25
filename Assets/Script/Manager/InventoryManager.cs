using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
        DontDestroyOnLoad(gameObject);
    }
    public Dictionary<int, int> ownBulletDictionary = new Dictionary<int, int>();//拥有的数量
    public List<int> ownBulletList = new List<int>();
    /// <summary>
    /// 获得子弹的方法
    /// </summary>
    /// <param name="bullet"></param>
    public void AddBullet(int bullet)
    {
        if (ownBulletDictionary.ContainsKey(bullet)) ownBulletDictionary[bullet]++;
        else ownBulletDictionary.Add(bullet, 1);
    }//获得子弹可以调用这个方法
        
    public void CheckOwnType()//检测字典里拥有的type有哪些，用于遍历字典然后添加进子弹选择页面
    {
        ownBulletList.Clear();//先清除上一次的
        foreach(int bulletType in ownBulletDictionary.Keys)
        {
            if (ownBulletList.Contains(bulletType)) continue;
            ownBulletList.Add(bulletType);
        }
    }
}
