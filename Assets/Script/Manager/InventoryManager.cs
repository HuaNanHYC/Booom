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

        PlayerInfoInitialize();//更新玩家信息
    }
    private void Start()
    {
        //LoadPlayerData();//开局读取一次数据
    }
    public Dictionary<int, int> ownBulletDictionary = new Dictionary<int, int>();//背包拥有的子弹及其拥有的数量
    [Header("背包系统已弃用，只使用玩家属性")]
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
    [Header("玩家属性")]
    public string playerName;//玩家名字
    public const float playerMaxHealth=1;//最大生命
    public float playerCurrentHealth;//现在的生命
    private Sprite playerHeadImage;//玩家的头像
    [Header("玩家的图片")]
    [TextArea] public string playerHeadImagePath;//玩家头像的路径
    [TextArea] public string playerActionImagePath;//玩家拿枪图片路径
    [TextArea] public string playerShotImagePath;//玩家中枪图片路径
    private Sprite playerActionImage;
    private Sprite playerShotImage;

    public Sprite PlayerHeadImage { get => playerHeadImage;}
    public Sprite PlayerActionImage { get => playerActionImage; }
    public Sprite PlayerShotImage { get => playerShotImage; }

    public void PlayerGetHurt(float damage)//受到伤害
    {
        playerCurrentHealth = Mathf.Max(playerCurrentHealth - damage, 0);
    }
    public void PlayerInfoInitialize()//初始化玩家信息，或重置
    {
        playerHeadImage = Resources.Load<Sprite>(playerHeadImagePath);
        playerActionImage = Resources.Load<Sprite>(playerActionImagePath);
        playerShotImage = Resources.Load<Sprite>(playerShotImagePath);
        playerCurrentHealth = playerMaxHealth;
    }
    #endregion



    #region 保存玩家属性的方法，9.29日弃用

    private string PLAYER_DATA_FILE_NAME = "PlayerData.GameSave";

    [SerializeField]
    class PlayerSave//用于保存的类
    {
        [System.Serializable]
        public struct InventorySave
        {
            public int id;
            public int amount;
        }
        [SerializeField]
        public List<InventorySave> inventoryList = new List<InventorySave>();//列表，存储id及对应数量


        //放入玩家属性对应类型
        public float playerHealth;


    }
    PlayerSave playerSave()//玩家背包和属性给保存用
    {
        PlayerSave playerDataSave = new PlayerSave();
        //背包信息保存
        foreach (KeyValuePair<int,int> ownBullet in ownBulletDictionary)
        {
            PlayerSave.InventorySave inventorySave=new PlayerSave.InventorySave();
            inventorySave.id = ownBullet.Key;
            inventorySave.amount = ownBullet.Value;
            playerDataSave.inventoryList.Add(inventorySave);
        }
        //添加玩家属性保存
        playerDataSave.playerHealth = playerMaxHealth;



        return playerDataSave;
    }
    public void SavePlayerData()//保存
    {
        SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, playerSave(), Application.persistentDataPath);
    }
    public void LoadPlayerData()//读取
    {
        PlayerSave playerSave = SaveSystem.LoadFromJson<PlayerSave>(PLAYER_DATA_FILE_NAME, Application.persistentDataPath);
        if (playerSave == null) return;
        for(int i=0; i < playerSave.inventoryList.Count; i++)//背包字典的添加
        {
            if (ownBulletDictionary.ContainsKey(playerSave.inventoryList[i].id))
                ownBulletDictionary[playerSave.inventoryList[i].id] = playerSave.inventoryList[i].amount;
            else ownBulletDictionary.Add(playerSave.inventoryList[i].id, playerSave.inventoryList[i].amount);
        }
    }

    public void ClearPlayerData()//清除存档
    {
        SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME, Application.persistentDataPath);
    }
    #endregion
}
