using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePrefab : MonoBehaviour
{
    private bool ifDone;//是否加载场景完毕
    private bool ifPressAnyButton;//是否按下任意键
    public GameObject pressAnyButtonToStart;//按任意键开始游戏提示的组件
    private Animator animator;//动画组件
    public bool IfDone { get => ifDone; set => ifDone = value; }
    public bool IfPressAnyButton { get => ifPressAnyButton; set => ifPressAnyButton = value; }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if(IfDone==true)
        {
            //pressAnyButtonToStart.SetActive(true);
            //if(Input.anyKey)
            //{
                //pressAnyButtonToStart.SetActive(false);
                IfPressAnyButton = true;
                IfDone = false;
                SceneManageSystem.Instance.IfAllowSceneActivation = true;
                animator.SetBool("FadeOut", true);
            //}
        }

    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
