using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public struct LevelInfo
    {
        public int iD;
        public GameObject prefab;
        public int levelID;
        public int enemyID;
        public int[] steadyBulletID;//固定子弹数组id
        public int[] steadyBulletPosition;//固定子弹位置
        public int[] enableBulletID;//玩家可用子弹
        public int levelStartStoryID;
        public int levelOverStoryID;
    }
    [SerializeField]
    public List<LevelInfo> levelList = new List<LevelInfo>();//关卡列表
    public Dictionary<int, LevelInfo> levelDictionary = new Dictionary<int, LevelInfo>();//关卡字典用于检索
}
