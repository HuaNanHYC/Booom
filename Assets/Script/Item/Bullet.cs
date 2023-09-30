using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour,IPointerEnterHandler,IPointerMoveHandler,IPointerExitHandler
{
    public int ID;
    public string bulletName;
    private int bulletInHoleNumber;
    [TextArea]
    public string bulletDescription;
    [TextArea]
    public string extraDescription;
    public float settingDamage=1;//设定的伤害
    public float actualDamage;//实际的伤害

    private Sprite bulletIcon;
    private Sprite bulletImage;
    [TextArea]
    public string bulletIconPath;
    [TextArea]
    public string bulletImagePath;

    private void Start()
    {
        bulletInfoRectTransform = currentBulletInfo.GetComponent<RectTransform>();
        bulletImage = GetComponent<Image>().sprite;
        UpdateBulletImageAndIcon();//更新图片
        InitializeBullet();
        UpdateBulletInfo();//同步信息
    }


    #region 鼠标悬停显示具体ui
    private RectTransform bulletInfoRectTransform;//UI的位置
    public GameObject currentBulletInfo;

    public Sprite BulletIcon { get => bulletIcon; }//只可读
    public Sprite BulletImage { get => bulletImage;}
    public int BulletInHoleNumber { get => bulletInHoleNumber; set => bulletInHoleNumber = value; }

    public void UpdateBulletInfo()//让信息更新
    {
        currentBulletInfo.transform.GetChild(0).GetComponent<Image>().sprite = bulletIcon;
        currentBulletInfo.transform.GetChild(0).GetComponentInChildren<Text>().text = bulletName;
        currentBulletInfo.transform.GetChild(1).GetComponent<Text>().text = bulletDescription;
        currentBulletInfo.transform.GetChild(2).GetComponent<Text>().text = extraDescription;
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

    #region 子弹初始化
    public void InitializeBullet()
    {
        actualDamage = settingDamage;//恢复伤害
    }

    public void UpdateBulletImageAndIcon()
    {
        Sprite imageSprite = Resources.Load<Sprite>(bulletIconPath);
        if (imageSprite != null)
        {
            // 获取物体上的Image组件
            bulletIcon = imageSprite;
        }
        else
        {
            Debug.Log("没有找到路径图片: " + bulletIconPath);
        }

        Sprite imageSprite2 = Resources.Load<Sprite>(bulletImagePath);
        if (imageSprite2 != null)
        {
            // 获取物体上的Image组件
            bulletImage = imageSprite;
        }
        else
        {
            Debug.Log("没有找到路径图片: " + bulletImagePath);
        }
    }
    #endregion
}
