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
        EnemyAction(true);
        yield return new WaitForSeconds(0.5f);
        EnemyReady(false);
        //准备开枪
        yield return new WaitForSeconds(0.5f);
        //剩1滴血时将枪口对准你
        if(currentHealth==1&&!if_ShootYou)
        {
            if_ShootYou = true;
            enemySpriteRenderer.sprite = shootToYouImage;
            battleSystem.if_PlayerShoot = true;
            yield return new WaitForSeconds(0.5f);
            battleSystem.JudegeShoot();
            yield return new WaitForSeconds(0.5f);
            EnemyAction(true);
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.if_PlayerShoot = true;
            battleSystem.StartShoot();
            StopAllCoroutines();//停止协程
        }
        else if (battleSystem.JudegeShoot())
        {
            EnemyShot();//中枪
            yield return new WaitForSeconds(0.5f);
            EnemyReady(false);
            yield return new WaitForSeconds(0.5f);
            EnemyAction(true);
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.StartShoot();
            StopAllCoroutines();
        }
        else
        {
            EnemyDodge();//没中枪
            yield return new WaitForSeconds(0.5f);
            EnemyReady(false);
            yield return new WaitForSeconds(0.5f);
            EnemyDodgeAction();
            actionHand.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.StartShoot();
            StopAllCoroutines();
        }
    }
}
