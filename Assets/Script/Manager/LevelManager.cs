using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (currentLevelId + 1 >= 30009)
        {
            UIManager.Instance.LoadScene("StartAndEnd");
            return;
        }
        SetCurrentLevel(currentLevelId+1);
        SceneManageSystem.Instance.GoToFigureScene("Dialogue");
    }
    public void CurrentLevel()//调用加载本关
    {
        if_StartDialogue = false;
        SceneManageSystem.Instance.GoToFigureScene("Level" + (currentLevelId - 30000).ToString());
    }
    #endregion

    #region 剧情配置

    /*[Header("特殊剧情，例如第一关失败插入的剧情等,手动拖拽配置")]
    public Dialogue dialogue1;//第一关输了的剧情
    private bool if_Dialogue1Show;//第一关输的剧情是否已经播放过
    public Dialogue dialogue8;//最后一关必定失败的剧情
    private bool if_Dialogue8Show;//第八关输的剧情是否已经播放过*/
    public void LastLevelDialogue()//最后一关的特殊对话判断
    {
        if (lastLevelJudge&&currentLevelId==30008)
        {
            if(StartSpecialDialogue())
                return;
        }
    }
    public bool StartSpecialDialogue()
    {
        DialogueInfo dialogueInfo = dialogueDic[currentLevelId];
        Dialogue dialogue = Resources.Load<Dialogue>(dialogueInfo.specialDialoguePath);
        bool if_HaveShow = dialogueInfo.If_SpecialDialogueShow;
        if (dialogue != null&&!if_HaveShow)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
            GameObject.FindWithTag("UiDialogue").transform.GetChild(0).gameObject.SetActive(true);

            dialogueInfo.IfSpecialDialogueShow(true);
            dialogueDic[currentLevelId] = dialogueInfo;
            return true;
        }
        return false;
    }
    [System.Serializable]
    public struct DialogueInfo
    {
        public int levelId;
        public string[] startDialoguePath;
        public string[] endDialoguePath;
        public string specialDialoguePath;
        private bool if_SpecialDialogueShow;//特殊剧情是否播放过
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
        public void IfSpecialDialogueShow(bool setting)
        {
            this.if_SpecialDialogueShow = setting;
        }
        public int StartDialogueIndex { get => startDialogueIndex; }
        public int EndDialogueIndex { get => endDialogueIndex;}
        public bool If_SpecialDialogueShow { get => if_SpecialDialogueShow;}
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
    private bool continueSprite;//停留图片判断
    public bool ContinueSprite { get { return continueSprite; } set { continueSprite = value; } }



    public void DialogueAfterBlack()//黑屏转场，给对话结束时用，dialogue的场景会有一个onenable就调用一次启动对话的物体
    {
        SceneManageSystem.Instance.GoToFigureScene("Dialogue");
    }
    public void DialogueNoBlack()
    {
        continueSprite = true;
        SceneManager.LoadScene("Dialogue");
    }
    #endregion

    //开头和结尾视频播放
    [SerializeField]
    private bool startVideoPlay;
    private bool endVideoPlay;
    public bool StartVideoPlay { get => startVideoPlay; set => startVideoPlay = value; }
    public bool EndVideoPlay { get => endVideoPlay; set => endVideoPlay = value; }

}
