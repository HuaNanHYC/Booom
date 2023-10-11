using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teach1 : MonoBehaviour
{
    public Text teachText;
    public Canvas[] canvas;
    public Canvas mainCanvas;
    public int clickIndex;
    public int maxIndex;
    public bool if_OpenLoadPage;
    private bool setOnce;
    public Vector3[] textPosition;
    private void Start()
    {
        if (UIManager.Instance.if_Teach1)
        {
            mainCanvas.gameObject.SetActive(false);
            return;
        }
            maxIndex = canvas.Length;
        NextTeach();
    }
    private void Update()
    {
        if (UIManager.Instance.if_Teach1)
        {
            return;
        }
        PlayerOperation();
    }
    
    public void NextTeach()
    {
        if (clickIndex < maxIndex)
            canvas[clickIndex].sortingOrder = 82;
        switch (clickIndex)
        {
            case 0:
                teachText.transform.parent.transform.localPosition = textPosition[0];
                teachText.text = "敌我的血量会显示在这里，确保自己不被击中，并且一轮过后能将对方的血量打到0吧！";
                break;
            case 1:
                teachText.transform.parent.transform.localPosition = textPosition[1];
                teachText.text = "对方的手枪已经拿出来了，点击装弹开始装载子弹吧！";
                break;
            case 2:
                teachText.transform.parent.transform.localPosition = textPosition[2];
                teachText.text = "这里显示的是左轮枪的弹巢，弹巢上的每个弹仓都有对应的编号，子弹会根据所在的弹仓编号产生特定的效果";
                break;
            case 3:
                teachText.transform.parent.transform.localPosition = textPosition[3];
                teachText.text = "这里显示的是你的弹药袋，列出了你能使用的所有子弹，悬停可以查看子弹的具体效果，点击子弹再点击任意弹仓就可以进行装弹啦！";
                break;
            default:break;
        }
    }
    private bool ifDelay;
    public void PlayerOperation()//判断玩家的点击操作
    {
        if (clickIndex > maxIndex - 1)
        {
            UIManager.Instance.if_Teach1 = true;
            gameObject.SetActive(false);
            SetAllCanvas();
            return;
        }
        if (if_OpenLoadPage && !setOnce)
        {
            mainCanvas.gameObject.SetActive(true);
            clickIndex++;
            NextTeach();
            setOnce = true;
        }

        if (Input.GetMouseButtonDown(0)&&!ifDelay)
        {
            SetAllCanvas();
            if (clickIndex < 1 || if_OpenLoadPage)
            {
                if (ifDelay) return;
                clickIndex++;
                if(clickIndex==2)
                {
                    ifDelay = true;
                    StartCoroutine(NextTeachDelay());
                    return;
                }
                NextTeach();
            }
            else if(!if_OpenLoadPage)
            {
                mainCanvas.gameObject.SetActive(false);
                canvas[1].gameObject.SetActive(false);
            }
            
        }
    }
    public void SetAllCanvas()
    {
        canvas[0].sortingOrder = 0;
        canvas[1].sortingOrder = 0;
        canvas[2].sortingOrder = 2;
        canvas[3].sortingOrder = 3;
        teachText.text = "";
    }
    public void SetIfOpenLoadPage(bool setting)
    {
        if_OpenLoadPage=setting; 
    }//给按钮判断

    IEnumerator NextTeachDelay()
    {
        yield return new WaitForSeconds(0.6f);
        NextTeach();
        ifDelay = false;
    }
}
