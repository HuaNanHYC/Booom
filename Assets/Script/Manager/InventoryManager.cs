using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance { get { return instance; } }

    public Sprite PlayerHeadImage { get => playerHeadImage; set => playerHeadImage = value; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
        DontDestroyOnLoad(gameObject);

        PlayerInfoInitialize();//���������Ϣ
    }
    private void Start()
    {
        LoadPlayerData();//���ֶ�ȡһ������
    }
    public Dictionary<int, int> ownBulletDictionary = new Dictionary<int, int>();//����ӵ�е��ӵ�����ӵ�е�����
    public List<int> ownBulletList = new List<int>();//����id�ж�
    /// <summary>
    /// ����ӵ��ķ���
    /// </summary>
    /// <param name="bullet"></param>
    public void AddBullet(int bullet)
    {
        if (ownBulletDictionary.ContainsKey(bullet)) ownBulletDictionary[bullet]++;
        else ownBulletDictionary.Add(bullet, 1);
    }//����ӵ����Ե����������
        
    public void CheckOwnType()//����ֵ���ӵ�е�type����Щ�����ڱ����ֵ�Ȼ�����ӽ��ӵ�ѡ��ҳ��
    {
        ownBulletList.Clear();//�������һ�ε�
        foreach(int bulletType in ownBulletDictionary.Keys)
        {
            if (ownBulletList.Contains(bulletType)) continue;
            ownBulletList.Add(bulletType);
        }
    }

    #region �������Ҳ���ϵ�����,���㱣��

    public string playerName;//�������
    public float playerMaxHealth;//�������
    public float playerCurrentHealth;//���ڵ�����
    private Sprite playerHeadImage;//��ҵ�ͷ��
    [TextArea]
    public string playerHeadImagePath;//���ͷ���·��
    public void PlayerGetHurt(float damage)//�ܵ��˺�
    {
        playerCurrentHealth = Mathf.Max(playerCurrentHealth - damage, 0);
        if(playerCurrentHealth<=0)
        {
            //��Ĵ���
        }
    }
    public void PlayerInfoInitialize()//��ʼ�������Ϣ��������
    {
        PlayerHeadImage = Resources.Load<Sprite>(playerHeadImagePath);
        playerCurrentHealth = playerMaxHealth;
    }
    #endregion



    #region ����������Եķ���

    private string PLAYER_DATA_FILE_NAME = "PlayerData.GameSave";

    [SerializeField]
    class PlayerSave//���ڱ������
    {
        [System.Serializable]
        public struct InventorySave
        {
            public int id;
            public int amount;
        }
        [SerializeField]
        public List<InventorySave> inventoryList = new List<InventorySave>();//�б����洢id����Ӧ����


        //����������Զ�Ӧ����
        public float playerHealth;


    }
    PlayerSave playerSave()//��ұ��������Ը�������
    {
        PlayerSave playerDataSave = new PlayerSave();
        //������Ϣ����
        foreach (KeyValuePair<int,int> ownBullet in ownBulletDictionary)
        {
            PlayerSave.InventorySave inventorySave=new PlayerSave.InventorySave();
            inventorySave.id = ownBullet.Key;
            inventorySave.amount = ownBullet.Value;
            playerDataSave.inventoryList.Add(inventorySave);
        }
        //����������Ա���
        playerDataSave.playerHealth = playerMaxHealth;



        return playerDataSave;
    }
    public void SavePlayerData()//����
    {
        SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, playerSave(), Application.persistentDataPath);
    }
    public void LoadPlayerData()//��ȡ
    {
        PlayerSave playerSave = SaveSystem.LoadFromJson<PlayerSave>(PLAYER_DATA_FILE_NAME, Application.persistentDataPath);
        if (playerSave == null) return;
        for(int i=0; i < playerSave.inventoryList.Count; i++)//�����ֵ������
        {
            if (ownBulletDictionary.ContainsKey(playerSave.inventoryList[i].id))
                ownBulletDictionary[playerSave.inventoryList[i].id] = playerSave.inventoryList[i].amount;
            else ownBulletDictionary.Add(playerSave.inventoryList[i].id, playerSave.inventoryList[i].amount);
        }
        playerMaxHealth = playerSave.playerHealth;
    }

    public void ClearPlayerData()//����浵
    {
        SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME, Application.persistentDataPath);
    }
    #endregion
}