using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletHole : MonoBehaviour
{
    private Image image;
    public int number;//左轮洞的序号
    public Bullet currentBullet;
    public bool if_Load;
    public bool if_AutoLoad;//判断这个洞玩家是否能用来装自己的子弹
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
        
    }

    #region 装填子弹
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

            image.sprite = currentBullet.BulletIcon;//设置成装填图片样式

            //BulletManager.Instance.currentBullet.gameObject.SetActive(false);//将列表中的子弹隐藏

            BulletManager.Instance.currentBullet = null;
        }
        else if (currentBullet != null)
        {
            if_Load = false;

            //currentBullet.gameObject.SetActive(true);//返还选中子弹

            image.sprite = unLoadSprite;//设置成未装填图片样式

            currentBullet = null;
        }

    }

    public void LoadBulletAuto(int bulletID)//电脑初始装子弹
    {
        if_AutoLoad = true;
        currentBullet = BulletManager.Instance.bulletDictionary[bulletID].GetComponent<Bullet>();
        currentBullet.BulletIcon = Resources.Load<Sprite>(currentBullet.bulletIconPath);
        if (currentBullet.BulletIcon == null) return;
        image.sprite = currentBullet.BulletIcon;//设置成装填图片样式
    }
    #endregion
}
