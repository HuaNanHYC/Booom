using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : Enemy
{
    public Sprite shootToYouImage; //将枪口对准你的图片
    private bool if_ShootYou=false;
    protected override void Start()
    {
        base.Start();
        //让玩家免疫第一颗子弹
        InventoryManager.Instance.If_Immute = true;
    }
    public override IEnumerator EnemyShooting()
    {
        //敌人拿枪
        yield return new WaitForSeconds(0.5f);//等待0.5秒
        enemySpriteRenderer.sprite = actionImage;
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = readyImage;
        //准备开枪
        yield return new WaitForSeconds(0.5f);
        //剩1滴血时将枪口对准你
        if(currentHealth==1&&!if_ShootYou)
        {
            if_ShootYou = true;
            enemySpriteRenderer.sprite = shootToYouImage;
            battleSystem.if_PlayerShoot = true;
            battleSystem.JudegeShoot();
            yield return new WaitForSeconds(0.5f);
            enemySpriteRenderer.sprite = actionImage;
            yield return new WaitForSeconds(0.5f);
            enemySpriteRenderer.sprite = dialogueImage;
            battleSystem.StartShoot();
            StopAllCoroutines();//停止协程
        }
        if (battleSystem.JudegeShoot())
        {
            enemySpriteRenderer.sprite = shotImage;//中枪
        }
        else
        {
            enemySpriteRenderer.sprite = dodgeImage;//没中枪
        }
        //回到初始装态，敌人把枪放回
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = readyImage;
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = actionImage;
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = dialogueImage;
        battleSystem.StartShoot();
        yield return null;
    }
}
