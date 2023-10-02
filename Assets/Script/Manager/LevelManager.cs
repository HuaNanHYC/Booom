using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }
    public int currentLevelId;
    [System.Serializable]
    public struct LevelInfo
    {
        public int levelID;
        public string targetScene;
        public int enemyID;
        [Header("固定数组id和位置需要对应")]
        public int[] steadyBulletID;//固定子弹数组id
        public int[] steadyBulletPosition;//固定子弹位置
        public int[] ableBulletID;//玩家不可用子弹
    }
    [SerializeField]
    public List<LevelInfo> levelList = new List<LevelInfo>();//关卡列表
    public Dictionary<int, LevelInfo> levelDictionary = new Dictionary<int, LevelInfo>();//关卡字典用于检索

    private void Awake()
    {
        if(instance == null)instance = this;
        else Destroy(instance);
        DontDestroyOnLoad(this);

        InitializeLevelDictionary();
    }
    public void InitializeLevelDictionary()//初始化字典
    {
        for(int i = 0; i < levelList.Count; i++)
        {
            levelDictionary.Add(levelList[i].levelID, levelList[i]);
        }
    }
    public void SetCurrentLevel(int levelId)//给按钮使用的设置当前关卡
    {
        currentLevelId = levelId;
    }
    public List<int> EnableBulletIdJudge(List<int> list)//提供给玩家的可用子弹
    {
        int[] steadyBulletId = levelDictionary[currentLevelId].ableBulletID;
        for(int i=0;i<steadyBulletId.Length;i++)
        {
            if(list.Contains(steadyBulletId[i]))
                list.Remove(steadyBulletId[i]);
        }
        return list;
    }
    public int[] GetCurentLevelSteadyBulletId()//提供当前关卡配置的固定子弹id
    {
        return levelDictionary[currentLevelId].steadyBulletID;
    }
    public int[] GetCurentLevelSteadyBulletPosition()//提供当前关卡配置的固定子弹位置的id
    {
        return levelDictionary[currentLevelId].steadyBulletPosition;
    }
}
