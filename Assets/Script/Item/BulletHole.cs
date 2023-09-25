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
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = unLoadSprite;//开始设置为未装填状态
    }

    void Update()
    {
        
    }

    #region 按钮用
    public Sprite unLoadSprite;
    public Sprite loadSprite;//装填子弹

    public void  LoadBullet()
    {
        if (!if_Load && BulletManager.Instance.currentBullet != null)
        {
            if_Load = true;
            currentBullet = BulletManager.Instance.currentBullet;//记录现在装填的子弹

            image.sprite = currentBullet.sprite;//设置成装填图片样式

            BulletManager.Instance.currentBullet.gameObject.SetActive(false);//将列表中的子弹隐藏

            BulletManager.Instance.currentBullet = null;
        }
        else if (currentBullet != null)
        {
            if_Load = false;

            currentBullet.gameObject.SetActive(true);//返还选中子弹

            image.sprite = unLoadSprite;//设置成未装填图片样式

            currentBullet = null;
        }

    }

    #endregion
}
