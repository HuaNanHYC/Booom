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
    /*[TextArea]
    public string extraDescription;*/
    public float settingDamage=1;//设定的伤害
    public float actualDamage;//实际的伤害

    private Sprite bulletIcon;
    private Sprite bulletImageMain;
    private Sprite bulletImageHover;
    private Sprite bulletImageSelected;
    [TextArea] public string bulletIconPath;
    [TextArea] public string bulletImagePath;
    [TextArea] public string bulletImageHoverPath;
    [TextArea] public string bulletImageSelectedPath;

    private Image bulletImageSelf;
    private bool if_OnlyUseBullet;
    private void Start()
    {
        if (if_OnlyUseBullet) return;
        bulletInfoRectTransform = currentBulletInfo.GetComponent<RectTransform>();
        bulletImageSelf = transform.GetComponent<Image>();
        UpdateBulletImageAndIcon();//更新图片
        InitializeBullet();
        UpdateBulletInfo();//同步信息
    }
    private void Update()
    {
        if (if_OnlyUseBullet) return;
        IfSelected();
    }


    #region 鼠标悬停显示具体ui
    private RectTransform bulletInfoRectTransform;//UI的位置
    public GameObject currentBulletInfo;

    public Sprite BulletIcon { get => bulletIcon; set => bulletIcon = value; }//只可读
    //public Sprite BulletImage { get => bulletImage;}
    public int BulletInHoleNumber { get => bulletInHoleNumber; set => bulletInHoleNumber = value; }
    public bool If_OnlyUseBullet { get => if_OnlyUseBullet; set => if_OnlyUseBullet = value; }

    public void UpdateBulletInfo()//让信息更新
    {
        currentBulletInfo.transform.GetChild(0).GetComponent<Text>().text = bulletName;
        currentBulletInfo.transform.GetChild(1).GetComponent<Text>().text = bulletDescription;
        //currentBulletInfo.transform.GetChild(2).GetComponent<Text>().text = extraDescription;
    }
    private bool if_PointerEnter;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if_PointerEnter = true;

        // 鼠标进入对象时显示提示
        currentBulletInfo.SetActive(true);
        //图片形象变换
        bulletImageSelf.sprite = bulletImageHover;
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if_PointerEnter = false;

        // 鼠标离开对象时隐藏提示
        currentBulletInfo.SetActive(false);

        bulletImageSelf.sprite = bulletImageMain;
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

   public void IfSelected()
    {
        if(BulletManager.Instance.currentBullet==this)
        {
            bulletImageSelf.sprite = bulletImageSelected;
            transform.GetChild(0).GetComponent<Text>().color=Color.white;
        }
        else if(if_PointerEnter==false)
        {
            bulletImageSelf.sprite = bulletImageMain;
            transform.GetChild(0).GetComponent<Text>().color = new Color(0.9568627f, 0.4392157f, 0.1607843f);
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
        bulletIcon = Resources.Load<Sprite>(bulletIconPath);
        bulletImageMain = Resources.Load<Sprite>(bulletImagePath);
        bulletImageHover = Resources.Load<Sprite>(bulletImageHoverPath);
        bulletImageSelected = Resources.Load<Sprite>(bulletImageSelectedPath);

        bulletImageSelf.sprite = bulletImageMain;
    }

    #endregion
}
