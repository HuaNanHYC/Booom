using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Enemy
{
    bool have_ShootTwice = false;
    public override IEnumerator EnemyShooting()//敌人开枪
    {
        //敌人拿枪
        yield return new WaitForSeconds(0.5f);//等待0.5秒
        enemySpriteRenderer.sprite = actionImage;
        yield return new WaitForSeconds(0.5f);
        enemySpriteRenderer.sprite = readyImage;
        //准备开枪
        yield return new WaitForSeconds(0.5f);
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
        //连开两枪的判断
        if (!have_ShootTwice)
        {
            battleSystem.if_PlayerShoot = false;
            have_ShootTwice = true;
        }
        else
        {
            have_ShootTwice = false;
        }
        battleSystem.StartShoot();
        yield return null;
    }
}
