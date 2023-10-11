using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Enemy
{
    bool have_ShootTwice = false;
    public override IEnumerator EnemyShooting()//敌人开枪
    {
        //敌人拿枪
        yield return new WaitForSeconds(beforeActionInterval);//等待0.5秒
        EnemyAction(true);
        yield return new WaitForSeconds(actionToReadyInterval);
        EnemyReady(false);
        EnemyBulletTurnAudio();
        //准备开枪
        yield return new WaitForSeconds(readyToShootInterval);
A:      if (battleSystem.JudegeShoot())
        {
            EnemyShot();//中枪
            yield return new WaitForSeconds(shootToReadyInterval);
            EnemyReady(false);
            yield return new WaitForSeconds(readyToActionInterval);
            if (!have_ShootTwice)
            {
                battleSystem.if_PlayerShoot = false;
                have_ShootTwice = true;
                goto A;
            }
            else
            {
                have_ShootTwice = false;
            }
            EnemyAction(true);
            yield return new WaitForSeconds(actionToIdleInterval);
            actionHand.SetActive(false);
            EnemyIdle();
        }
        else
        {
            EnemyDodge();//没中枪
            yield return new WaitForSeconds(shootToReadyInterval);
            EnemyReady(false);
            yield return new WaitForSeconds(readyToActionInterval);
            if (!have_ShootTwice)
            {
                battleSystem.if_PlayerShoot = false;
                have_ShootTwice = true;
                goto A;
            }
            else
            {
                have_ShootTwice = false;
            }
            EnemyDodgeAction();
            actionHand.SetActive(true);
            yield return new WaitForSeconds(actionToIdleInterval);
            actionHand.SetActive(false);
            EnemyIdle();
        }
       
        //连开两枪的判断
        /*if (!have_ShootTwice)
        {
            battleSystem.if_PlayerShoot = false;
            have_ShootTwice = true;
            goto A;
        }
        else
        {
            have_ShootTwice = false;
        }*/
        battleSystem.StartShoot();
        yield return null;
    }
}
