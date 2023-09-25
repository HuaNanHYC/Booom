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

    public Dictionary<int, Bullet> loadBulletDictionary = new Dictionary<int, Bullet>();//存储左轮装的子弹及顺序
    public GameObject revolver;//用来遍历的左轮，底下有一堆hole的父物体
    public void LoadBullet()//子弹装填进入字典，给确定按钮
    {
        int holeNumber=1;
        while (holeNumber <= revolver.transform.childCount)
        { 
            for (int i = 0; i < revolver.transform.childCount; i++)
            {
                if (revolver.transform.GetChild(i).GetComponent<BulletHole>().number==holeNumber)
                {
                    Bullet bullet = revolver.transform.GetChild(i).GetComponent<BulletHole>().currentBullet;
                    if (loadBulletDictionary.ContainsKey(holeNumber)) loadBulletDictionary[holeNumber] = bullet;//字典已有就替换
                    else loadBulletDictionary.Add(holeNumber, bullet);//字典没有就添加
                    holeNumber++;
                    continue;
                }
                //空值判断
                if (loadBulletDictionary.ContainsKey(holeNumber)) loadBulletDictionary[holeNumber] = null;//字典已有就替换
                else loadBulletDictionary.Add(holeNumber, null);//此处应该装填空子弹属性
                holeNumber++;
                continue;
            }
        }
    }
}
