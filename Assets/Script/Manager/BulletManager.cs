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
        bulletList.Clear();
        CreateBulletSelect();
    }
    [System.Serializable]
    public struct BulletInfo
    {
        public int ID;
        public GameObject prefab;
    }
    [SerializeField]
    public List<BulletInfo> bulletList = new List<BulletInfo>();//子弹列表
    public Dictionary<int, GameObject> bulletDictionary = new Dictionary<int, GameObject>();//子弹字典用于检索
    public void InitializeDictionary()
    {
        for(int i=0;i<bulletList.Count; i++)
        {
            bulletDictionary.Add(bulletList[i].ID, bulletList[i].prefab);
        }
    }//初始化字典

    public Bullet currentBullet;//现在选择的子弹
    #region 左轮设置与读取
    public Dictionary<int, Bullet> loadedBulletDictionary = new Dictionary<int, Bullet>();//存储左轮装的子弹及顺序
    [Header("空包弹id")]
    public int emptyBullet;
    [Header("用来查看左轮里子弹以方便测试的列表")]
    public List<Bullet> loadedBulletList = new List<Bullet>();
    [Header("左轮的ui")]
    public GameObject revolver;//用来遍历的左轮，底下有一堆hole的物体
    public void LoadBullet()//子弹装填进入字典，给确定按钮
    {
        //清除列表及列表生成的子物体
        for (int i = bulletList.Count - 1; i >= 0; i--)
        {
            if (i<transform.childCount)
                Destroy(transform.GetChild(i));
        }
        loadedBulletList.Clear();
        
        int holeNumber=1;
        while (holeNumber <= revolver.transform.childCount)
        { 
            for (int i = 0; i < revolver.transform.childCount; i++)
            {
                if (revolver.transform.GetChild(i).GetComponent<BulletHole>().number==holeNumber)
                {
                    Bullet bullet = revolver.transform.GetChild(i).GetComponent<BulletHole>().currentBullet;
                    if (bullet == null) bullet = bulletDictionary[emptyBullet].GetComponent<Bullet>();//空的话给空包弹
                    if (loadedBulletDictionary.ContainsKey(holeNumber)) loadedBulletDictionary[holeNumber] = bullet;//字典已有就替换
                    else loadedBulletDictionary.Add(holeNumber, bullet);//字典没有就添加
                    holeNumber++;
                    continue;
                }
                //空弹匣判断，装入空包弹
                Bullet nullBullet = bulletDictionary[emptyBullet].GetComponent<Bullet>();
                if (loadedBulletDictionary.ContainsKey(holeNumber)) loadedBulletDictionary[holeNumber] = nullBullet;//字典已有就替换
                else loadedBulletDictionary.Add(holeNumber, nullBullet);//此处应该装填空子弹属性
                holeNumber++;
                continue;
            }
        }
        //测试可查看的列表,同时要用的
        foreach(Bullet bullet in loadedBulletDictionary.Values)
        {
            string name = bullet.bulletName;
            GameObject newBulletObject=new GameObject(name);
            newBulletObject.transform.SetParent(transform);
            newBulletObject.AddComponent<Bullet>();
            Bullet newBullet= newBulletObject.GetComponent<Bullet>();
            newBullet.actualDamage = bullet.actualDamage;
            newBullet.settingDamage = bullet.settingDamage;
            newBullet.ID = bullet.ID;
            newBullet.bulletName = bullet.bulletName;
            newBullet.If_OnlyUseBullet = true;
            loadedBulletList.Add(newBullet);
        }
        for(int i = 0; i < loadedBulletList.Count; i++)
        {
            loadedBulletList[i].BulletInHoleNumber = i;
        }

    }

    [Header("子弹冲突ui")]
    public GameObject putIn_Conflict;
    public bool JudgeExistTogether_IfCanPutIn(int id)//判断是否可以装奇数弹等有无冲突
    {
        if (id == 10005)//需要判断是否有偶数弹和质数弹存在
        {
            for(int i=0;i<revolver.transform.childCount;i++)
            {
                Bullet bullet = revolver.transform.GetChild(i).GetComponent<BulletHole>().currentBullet;
                if (bullet != null)
                {
                    if (bullet.ID == 10006 || bullet.ID == 10007)
                    {
                        PutInConflicStart();
                        return false;
                    }
                }
            }
            return true;
        }
        else if (id == 10006)//需要判断是否有奇数弹和质数弹存在
        {
            for (int i = 0; i < revolver.transform.childCount; i++)
            {
                Bullet bullet = revolver.transform.GetChild(i).GetComponent<BulletHole>().currentBullet;
                if (bullet != null)
                {
                    if (bullet.ID == 10005 || bullet.ID == 10007)
                    {
                        PutInConflicStart();
                        return false;
                    }
                }
            }
            return true;
        }
        else if (id == 10007)//需要判断是否有奇数弹和偶数弹存在
        {
            for (int i = 0; i < revolver.transform.childCount; i++)
            {
                Bullet bullet = revolver.transform.GetChild(i).GetComponent<BulletHole>().currentBullet;
                if (bullet != null)
                {
                    if (bullet.ID == 10005 || bullet.ID == 10006)
                    {
                        PutInConflicStart();
                        return false;
                    }
                }
            }
            return true;
        }

        return true;
    }
    public void PutInConflicStart()
    {
        StopAllCoroutines();
        putIn_Conflict.SetActive(false);
        StartCoroutine(PutInConflict());
    }
    IEnumerator PutInConflict()
    {
        putIn_Conflict.SetActive(true);
        yield return new WaitForSeconds(2);
        putIn_Conflict.SetActive(false);
        yield return null;
    }
    #endregion

    #region 子弹选择界面读取可用子弹生成子弹
    [Header("选择子弹的ui界面")]
    public GameObject bulletSelect;
    public void CreateBulletSelect()// 生成子弹选择列表
    {
        ClearBulletSelect();
        /*InventoryManager.Instance.CheckOwnType();//先检测拥有的种类
        List<int> list = InventoryManager.Instance.ownBulletList;
        if (LevelManager.Instance != null)
            LevelManager.Instance.EnableBulletIdJudge(list);//去除不可用子弹
        //根据已有种类去搜索字典
        for (int i = 0; i < list.Count; i++)
        {
            //根据字典里对应种类拥有的数量进行生成
            for (int j = 0; j < InventoryManager.Instance.ownBulletDictionary[list[i]]; j++)
            {
                Instantiate(bulletDictionary[list[i]], bulletSelect.transform);
            }
        }*/


        //根据关卡提供的可用子弹生成子弹
        int currentLevel = LevelManager.Instance.currentLevelId;
        int[] bulletsToCreate = LevelManager.Instance.levelDictionary[currentLevel].ableBulletID;
        for (int i = 0; i < bulletsToCreate.Length; i++)
        {
            Instantiate(bulletDictionary[bulletsToCreate[i]], bulletSelect.transform);
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
