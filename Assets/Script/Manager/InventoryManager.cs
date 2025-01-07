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

        PlayerInfoInitialize();//���������Ϣ
    }
    private void Start()
    {
        //LoadPlayerData();//���ֶ�ȡһ������
    }
    public Dictionary<int, int> ownBulletDictionary = new Dictionary<int, int>();//����ӵ�е��ӵ�����ӵ�е�����
    [Header("����ϵͳ�����ã�ֻʹ���������")]
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
    [Header("�������")]
    public string playerName;//�������
    public const float playerMaxHealth=1;//�������
    public float playerCurrentHealth;//���ڵ�����
    private Sprite playerHeadImage;//��ҵ�ͷ��
    [Header("��ҵ�ͼƬ")]
    [TextArea] public string playerHeadImagePath;//���ͷ���·��
    [TextArea] public string playerActionImagePath;//�����ǹͼƬ·��
    [TextArea] public string playerHurtImagePath;//�����ǹͼƬ·��
    private Sprite playerActionImage;
    private Sprite playerHurtImage;
    private bool if_Immute = false;
    public BattleSystem battleSystem;//ս��ϵͳ
    public Sprite PlayerHeadImage { get => playerHeadImage;}
    public Sprite PlayerActionImage { get => playerActionImage; }
    public Sprite PlayerHurtImage { get => playerHurtImage; }
    public bool If_Immute { get => if_Immute; set => if_Immute = value; }

    public void PlayerGetHurt(float damage)//�ܵ��˺�
    {
        if (if_Immute && damage != 0)//���ߵķ���
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_MissFire);
            if_Immute = false;
            return;
        }
        playerCurrentHealth = Mathf.Max(playerCurrentHealth - damage, 0);
        /*if (damage == 0 && battleSystem.bullets[battleSystem.BulletIndexShoot].ID == 10003)//���ӵ��ж�
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_MissFire);
            return;
        }*/
        if (damage>0)//��������
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_Fire);
        }
        else
        {
            AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.Revolver_NoBullet);
        }
    }
    public void PlayerInfoInitialize()//��ʼ�������Ϣ��������
    {
        playerHeadImage = Resources.Load<Sprite>(playerHeadImagePath);
        playerActionImage = Resources.Load<Sprite>(playerActionImagePath);
        playerHurtImage = Resources.Load<Sprite>(playerHurtImagePath);
        playerCurrentHealth = playerMaxHealth;
    }
    #endregion



    #region ����������Եķ�����9.29������
    /*
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
    }

    /*public void ClearPlayerData()//����浵
    {
        SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME, Application.persistentDataPath);
    }*/
    #endregion
}