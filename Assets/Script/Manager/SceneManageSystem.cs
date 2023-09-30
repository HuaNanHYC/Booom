using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManageSystem : MonoBehaviour
{
    private static SceneManageSystem instance;
    public static SceneManageSystem Instance { get { return instance; } }
    
    public void Awake()
    {
        if(instance == null)instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private bool ifAllowSceneActivation;//由过渡组件设置是否可以加载目标场景

    public GameObject fadePrefab;//场景过渡加载的预制体

    public bool IfAllowSceneActivation { get => ifAllowSceneActivation; set => ifAllowSceneActivation = value; }
    public void GoToFigureScene(string name)
    {
        StartCoroutine(ChangeScene(name));
    }

    AsyncOperation asyncOperation;
    static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();//用于while
    IEnumerator ChangeScene(string name)
    {
        GameObject fade=Instantiate(fadePrefab);
        asyncOperation=SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSecondsRealtime(1);
        while(asyncOperation.progress<0.9f)
        {
            yield return _endOfFrame;
        }
        fade.GetComponent<FadePrefab>().IfDone = true;//fade那边开始判定
        while (true)
        {
            if (IfAllowSceneActivation)
            {
                asyncOperation.allowSceneActivation = IfAllowSceneActivation;
                IfAllowSceneActivation = false;
                break;
            }
            yield return _endOfFrame;
        }
    }



}
