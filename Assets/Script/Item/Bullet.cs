using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum BulletType
{
    NullBullet,
    NormalBullet,
}

public class Bullet : MonoBehaviour,IPointerEnterHandler,IPointerMoveHandler,IPointerExitHandler
{
    public int ID;
    public string bulletName;
    public string description;
    public BulletType type;
    public float damage;
    public Sprite sprite;

    private void Start()
    {
        bulletInfoRectTransform = currentBulletInfo.GetComponent<RectTransform>();
        UpdateBulletInfo();//同步信息
    }


    #region 鼠标悬停显示具体ui
    private RectTransform bulletInfoRectTransform;//UI的位置
    public GameObject currentBulletInfo;
    public void UpdateBulletInfo()//让信息更新
    {
        currentBulletInfo.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        currentBulletInfo.transform.GetChild(0).GetComponentInChildren<Text>().text = bulletName;
        currentBulletInfo.transform.GetChild(1).GetComponent<Text>().text = description;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标进入对象时显示提示
        currentBulletInfo.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标离开对象时隐藏提示
        currentBulletInfo.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 localMousePos;
        // 获取UI元素的宽高
        float uiElementWidth = bulletInfoRectTransform.rect.width;
        float uiElementHeight = bulletInfoRectTransform.rect.height;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                bulletInfoRectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localMousePos))
        {
            // 根据UI元素的大小自动计算偏移量，确保不重叠
            Vector2 offset = new Vector2(-uiElementWidth / 1.9f, -uiElementHeight / 1.9f);

            // 设置UI元素的位置
            bulletInfoRectTransform.localPosition = localMousePos + offset;
        }
    }
    #endregion

    #region 用于按钮操作
    public void SetCurrentBullet()//设置现在选择的子弹
    {
        BulletManager.Instance.currentBullet = this;
    }

    #endregion

    #region 子弹效果选择，根据类型判断
    protected virtual void Effect()
    {
        //子弹效果，写到继承的类中
    }


    #endregion
}
