using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    protected override void Start()
    {
        base.Start();
        //让玩家和敌人都免疫
        if_Immute = true;
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
        if(if_Immute)
        {
            enemySpriteRenderer.sprite = dodgeImage;//没中枪
            battleSystem.JudegeShoot();
        }
        else if (battleSystem.JudegeShoot())
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
