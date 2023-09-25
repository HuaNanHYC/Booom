using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static BulletManager instance;
    public static BulletManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(instance);

        InitializeDictionary();
    }
    private void Start()
    {
        CreateBulletSelect();
    }
    [System.Serializable]
    public struct BulletInfo
    {
        public BulletType type;
        public GameObject prefab;
    }
    [SerializeField]
    public List<BulletInfo> bulletList = new List<BulletInfo>();//子弹列表
    public Dictionary<BulletType, GameObject> bulletDictionary = new Dictionary<BulletType, GameObject>();//子弹字典用于检索
    public void InitializeDictionary()
    {
        for(int i=0;i<bulletList.Count; i++)
        {
            bulletDictionary.Add(bulletList[i].type, bulletList[i].prefab);
        }
    }//初始化字典

    public Bullet currentBullet;//现在选择的子弹
    #region 左轮设置与读取
    public Dictionary<int, Bullet> loadedBulletDictionary = new Dictionary<int, Bullet>();//存储左轮装的子弹及顺序
    [Header("用来查看左轮里子弹以方便测试的列表")]
    public List<Bullet> loadedBulletList = new List<Bullet>();
    [Header("左轮的ui")]
    public GameObject revolver;//用来遍历的左轮，底下有一堆hole的父物体
    public void LoadBullet()//子弹装填进入字典，给确定按钮
    {
        loadedBulletList.Clear();
        int holeNumber=1;
        while (holeNumber <= revolver.transform.childCount)
        { 
            for (int i = 0; i < revolver.transform.childCount; i++)
            {
                if (revolver.transform.GetChild(i).GetComponent<BulletHole>().number==holeNumber)
                {
                    Bullet bullet = revolver.transform.GetChild(i).GetComponent<BulletHole>().currentBullet;
                    if (loadedBulletDictionary.ContainsKey(holeNumber)) loadedBulletDictionary[holeNumber] = bullet;//字典已有就替换
                    else loadedBulletDictionary.Add(holeNumber, bullet);//字典没有就添加
                    holeNumber++;
                    continue;
                }
                //空弹匣判断，装入空包弹
                Bullet nullBullet = bulletDictionary[BulletType.NullBullet].GetComponent<Bullet>();
                if (loadedBulletDictionary.ContainsKey(holeNumber)) loadedBulletDictionary[holeNumber] = nullBullet;//字典已有就替换
                else loadedBulletDictionary.Add(holeNumber, nullBullet);//此处应该装填空子弹属性
                holeNumber++;
                continue;
            }
        }
        //测试可查看的列表
        foreach(Bullet bullet in loadedBulletDictionary.Values)
        {
            loadedBulletList.Add(bullet);
        }

    }
    #endregion

    #region 子弹选择界面读取背包生成子弹
    [Header("选择子弹的ui界面")]
    public GameObject bulletSelect;
    /// <summary>
    /// 生成子弹选择列表
    /// </summary>
    public void CreateBulletSelect()
    {
        ClearBulletSelect();
        InventoryManager.Instance.CheckOwnType();//先检测拥有的种类
        List<BulletType> list = InventoryManager.Instance.ownBulletList;
        //根据已有种类去搜索字典
        for (int i = 0; i < list.Count; i++)
        {
            //根据字典里对应种类拥有的数量进行生成
            for (int j = 0; j < InventoryManager.Instance.ownBulletDictionary[list[i]]; j++)
            {
                Instantiate(bulletDictionary[list[i]], bulletSelect.transform);
            }
        }
        
    }
    public void ClearBulletSelect()//清除一次子弹选择界面
    {
        for(int i=0;i<bulletSelect.transform.childCount;i++)
        {
            Destroy(bulletSelect.transform.GetChild(i).gameObject);
        }
    }

    #endregion
}
