using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePage : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {

    }
    public void SetThisShowState(bool ifShow)
    {
        anim.SetBool("If_Show", ifShow);
    }//关闭按钮使用
}
