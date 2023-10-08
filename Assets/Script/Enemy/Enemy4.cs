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
        EnemyAction(true);
        yield return new WaitForSeconds(0.5f);
        EnemyReady(false);
        //准备开枪
        yield return new WaitForSeconds(0.5f);
A:      if (battleSystem.JudegeShoot())
        {
            EnemyShot();//中枪
            yield return new WaitForSeconds(0.5f);
            EnemyReady(false);
            yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(0.5f);
            actionHand.SetActive(false);
            EnemyIdle();
        }
        else
        {
            EnemyDodge();//没中枪
            yield return new WaitForSeconds(0.5f);
            EnemyReady(false);
            yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(0.5f);
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
