using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(this);

        InitializeLevelDictionary();
        InitializeDialogueDic();
    }
    #region 关卡配置

    public int currentLevelId;
    public bool lastLevelJudge;//最后一关的第一次必输判断
    [System.Serializable]
    public struct LevelInfo
    {
        public int levelID;
        //public string targetScene;
        public int enemyID;
        [Header("固定数组id和位置需要对应")]
        public int[] steadyBulletID;//固定子弹数组id
        public int[] steadyBulletPosition;//固定子弹位置
        public int[] ableBulletID;//玩家不可用子弹
    }
    [SerializeField]
    public List<LevelInfo> levelList = new List<LevelInfo>();//关卡列表
    public Dictionary<int, LevelInfo> levelDictionary = new Dictionary<int, LevelInfo>();//关卡字典用于检索

    
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
    public void NextLevel()//给调用去往下一关,包括场景跳转
    {
        SetCurrentLevel(currentLevelId+1);
        SceneManageSystem.Instance.GoToFigureScene("Dialogue");
    }
    public void CurrentLevel()//调用加载本关
    {
        SceneManageSystem.Instance.GoToFigureScene("Level" + (currentLevelId - 30000).ToString());
    }
    #endregion

    #region 剧情配置

    [Header("特殊剧情，例如第一关失败插入的剧情等")]
    public Dialogue dialogue1;//第一关输了的剧情
    public Dialogue dialogue2;//最后一关必定失败的剧情
    public int specialDialogueIndex;//到时后需要特殊剧情时设置一下
    public void StartSpecialDialogue()
    {
        if(specialDialogueIndex == 1)
        {
            DialogueManager.Instance.StartDialogue(dialogue1);
        }
        else if (specialDialogueIndex == 2)
        {
            DialogueManager.Instance.StartDialogue(dialogue2);
        }
    }
    [System.Serializable]
    public struct DialogueInfo
    {
        public int levelId;
        public string[] startDialoguePath;
        public string[] endDialoguePath;
        private int startDialogueIndex;
        private int endDialogueIndex;
        public void StartIndexAdd()
        {
            this.startDialogueIndex += 1;
        }
        public void EndIndexAdd()
        {
            this.endDialogueIndex += 1;
        }
        public int StartDialogueIndex { get => startDialogueIndex; }
        public int EndDialogueIndex { get => endDialogueIndex;}
    }
    [SerializeField]
    public List<DialogueInfo> dialogueList = new List<DialogueInfo>();//关卡列表
    public Dictionary<int, DialogueInfo> dialogueDic = new Dictionary<int, DialogueInfo>();//关卡字典方便检索
    public void InitializeDialogueDic()//字典初始化
    {
        for(int i=0;i<dialogueList.Count;i++)
        {
            dialogueDic.Add(dialogueList[i].levelId, dialogueList[i]);
        }
    }
    private bool if_StartDialogue = true;//是否是开始剧情，否则播结束剧情
    public bool If_StartDialogue { get => if_StartDialogue; set => if_StartDialogue = value; }
    public Dialogue NextDialogueInStartDialogue()//启动下一段对话，没有就可以进入关卡了
    {
        int index = dialogueDic[currentLevelId].StartDialogueIndex;
        if (index < dialogueDic[currentLevelId].startDialoguePath.Length)
        {
            Dialogue dialogue = Resources.Load<Dialogue>(dialogueDic[currentLevelId].startDialoguePath[index]);
            //修改字典里的值
            DialogueInfo info = dialogueDic[currentLevelId];
            info.StartIndexAdd();
            dialogueDic[currentLevelId] = info;
            return dialogue;
        }
        else
        {
            if_StartDialogue = false;
            CurrentLevel();//对话没了，进入本关
            return null;
        }
    }
    public Dialogue NextDialogueInEndDialogue()//启动下一段对话，没有就可以下一关卡了
    {
        int index = dialogueDic[currentLevelId].EndDialogueIndex;
        if (index < dialogueDic[currentLevelId].endDialoguePath.Length)
        {
            Dialogue dialogue = Resources.Load<Dialogue>(dialogueDic[currentLevelId].endDialoguePath[index]);

            DialogueInfo info = dialogueDic[currentLevelId];
            info.EndIndexAdd();
            dialogueDic[currentLevelId] = info;

            return dialogue;
        }
        else
        {
            if_StartDialogue=true;//下次就是开始剧情了
            NextLevel();//对话没了，进入下一关
            return null;
        }
    }
    public void DialogueAfterBlack()//黑屏转场，给对话结束时用，dialogue的场景会有一个onenable就调用一次启动对话的物体
    {
        SceneManageSystem.Instance.GoToFigureScene("Dialogue");
    }
    #endregion
}
