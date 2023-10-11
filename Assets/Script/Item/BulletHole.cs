using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BulletHole : MonoBehaviour,IPointerEnterHandler,IPointerMoveHandler,IPointerExitHandler
{
    private Image image;
    public int number;//左轮洞的序号
    public Bullet currentBullet;
    public bool if_Load;
    public bool if_AutoLoad;//判断这个洞玩家是否能用来装自己的子弹
    public GameObject bulletDesc;//子弹的描述框显示
    private GameObject descriptionBoard;
    private RectTransform bulletInfoRectTransform;
    private void Awake()
    {
        if_AutoLoad = false;//开始玩家可以装填
        image = GetComponent<Image>();
    }
    void Start()
    {
        if(image.sprite==null)
            image.sprite = unLoadSprite;//开始设置为未装填状态
    }

    void Update()
    {
        if (currentBullet == null && descriptionBoard != null) descriptionBoard.SetActive(false);
    }

    #region 装填子弹
    public Sprite loadedSprite;//已经装填的样式
    public Sprite unLoadSprite;
    public Sprite loadSprite;//装填子弹

    public void  LoadBulletPlayer()//玩家装填子弹
    {
        if (if_AutoLoad) return;
        if (!if_Load && BulletManager.Instance.currentBullet != null)
        {
            if (BulletManager.Instance.JudgeExistTogether_IfCanPutIn(BulletManager.Instance.currentBullet.ID) == false) return;
            if_Load = true;
            currentBullet = BulletManager.Instance.currentBullet;//记录现在装填的子弹

            image.sprite = loadedSprite;//设置成装填图片样式
            UpdateLoadedSpriteInChild(true);

            //BulletManager.Instance.currentBullet.gameObject.SetActive(false);//将列表中的子弹隐藏

            BulletManager.Instance.currentBullet = null;
        }
        else if (currentBullet != null)
        {
            if_Load = false;

            //currentBullet.gameObject.SetActive(true);//返还选中子弹

            image.sprite = unLoadSprite;//设置成未装填图片样式
            UpdateLoadedSpriteInChild(false);

            currentBullet = null;
        }

    }

    public void LoadBulletAuto(int bulletID)//电脑初始装子弹
    {
        if_AutoLoad = true;
        currentBullet = BulletManager.Instance.bulletDictionary[bulletID].GetComponent<Bullet>();
        currentBullet.BulletIcon = Resources.Load<Sprite>(currentBullet.bulletIconPath);
        if (currentBullet.BulletIcon == null) return;
        image.sprite = loadedSprite;//设置成装填图片样式
        UpdateLoadedSpriteInChild(true);
    }

    public void UpdateLoadedSpriteInChild(bool if_setIcon)//生成一个子物体用来承载子弹的icon
    {
        if (if_setIcon)
        {
            if (!transform.Find("bulletIcon"))
            {
                GameObject childIcon = new GameObject("bulletIcon");
                childIcon.transform.SetParent(transform,false);
                Image childIconImage = childIcon.AddComponent<Image>();
                childIconImage.sprite = currentBullet.BulletIcon;
            }
            else
            {
                Image childIconImage = transform.Find("bulletIcon").GetComponent<Image>();
                childIconImage.sprite = currentBullet.BulletIcon;
            }
        }
        else
        {
            if (!transform.Find("bulletIcon"))
            {
                return;
            }
            else
            {
                Image childIconImage = transform.Find("bulletIcon").GetComponent<Image>();
                childIconImage.sprite = unLoadSprite;
            }
        }
    }
    public void UpdateBulletInfo()//让信息更新
    {
        descriptionBoard.transform.GetChild(0).GetComponent<Text>().text = currentBullet.bulletName;
        descriptionBoard.transform.GetChild(1).GetComponent<Text>().text = currentBullet.bulletDescription;
        descriptionBoard.transform.GetComponent<Image>().SetNativeSize();
        //currentBulletInfo.transform.GetChild(2).GetComponent<Text>().text = extraDescription;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentBullet!=null)
        {
            if (descriptionBoard != null)
            {
                UpdateBulletInfo();
                descriptionBoard.SetActive(true);
                return;
            }
            descriptionBoard = Instantiate(bulletDesc);
            descriptionBoard.transform.SetParent(transform,false);
            bulletInfoRectTransform = descriptionBoard.GetComponent<RectTransform>();
            descriptionBoard.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
            UpdateBulletInfo();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (bulletInfoRectTransform == null) return;
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
            Vector2 offset = new Vector2(0, -uiElementHeight / 1f);

            // 设置UI元素的位置
            bulletInfoRectTransform.localPosition = localMousePos + offset;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(descriptionBoard!=null)
        {
            descriptionBoard.SetActive(false);
        }
    }
    #endregion
}
