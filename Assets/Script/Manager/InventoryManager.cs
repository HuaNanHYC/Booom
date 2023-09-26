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
    public Dictionary<int, int> ownBulletDictionary = new Dictionary<int, int>();//背包拥有的子弹及其拥有的数量
    public List<int> ownBulletList = new List<int>();//用于id判断
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

    #region 玩家属性也整合到这里,方便保存





    #endregion



    #region 保存玩家属性的方法
    private string PLAYER_DATA_FILE_NAME = "PlayerData.GameSave";


    [SerializeField]
    class PlayerSave//用于保存的类
    {
        public struct InventorySave
        {
            public int id;
            public int amount;
        }
        public List<InventorySave> inventoryList = new List<InventorySave>();//列表，存储id及对应数量


        //放入玩家属性对应类型



    }
    PlayerSave playerSave()//玩家背包和属性保存
    {
        PlayerSave playerDataSave = new PlayerSave();
        foreach (KeyValuePair<int,int> ownBullet in ownBulletDictionary)
        {
            PlayerSave.InventorySave inventorySave=new PlayerSave.InventorySave();
            inventorySave.id = ownBullet.Key;
            inventorySave.amount = ownBullet.Value;
            playerDataSave.inventoryList.Add(inventorySave);
        }


        //添加玩家属性保存




        return playerDataSave;
    }
    public void SavePlayerData()//保存
    {
        if (SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, playerSave(), Application.persistentDataPath))
        {
            Debug.Log($"保存成功{Application.persistentDataPath}");
        }
    }

    #endregion
}
