using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
{
    private int immuteCount;//免疫计数
    public override IEnumerator EnemyShooting()
    {
        immuteCount++;
        //第三次射击时免疫伤害
        /*if (battleSystem.BulletIndexShoot == 2)
        {
            battleSystem.bullets[battleSystem.BulletIndexShoot].actualDamage = 0;
        }*/
        //敌人拿枪
        yield return new WaitForSeconds(beforeActionInterval);//等待0.5秒
        EnemyAction(true);
        yield return new WaitForSeconds(actionToReadyInterval);
        EnemyReady(false,true);
        EnemyBulletTurnAudio();
        //准备开枪
        yield return new WaitForSeconds(readyToShootInterval);
        if (immuteCount == 3)
        {
            if_Immute = true;
            battleSystem.JudegeShoot();
            EnemyDodge();//没中枪
            yield return new WaitForSeconds(shootToReadyInterval);
            EnemyReady(false, false);
            yield return new WaitForSeconds(readyToActionInterval);
            EnemyDodgeAction();
            actionHand.SetActive(true);
            yield return new WaitForSeconds(actionToIdleInterval);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.StartShoot();
            StopAllCoroutines();
        }
        else if (battleSystem.JudegeShoot())
        {
            EnemyShot();//中枪
            yield return new WaitForSeconds(shootToReadyInterval);
            EnemyReady(false,false);
            yield return new WaitForSeconds(readyToActionInterval);
            EnemyAction(true);
            yield return new WaitForSeconds(actionToIdleInterval);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.StartShoot();
            StopAllCoroutines();
        }
        else
        {
            EnemyDodge();//没中枪
            yield return new WaitForSeconds(shootToReadyInterval);
            EnemyReady(false,false);
            yield return new WaitForSeconds(readyToActionInterval);
            EnemyDodgeAction();
            actionHand.SetActive(true);
            yield return new WaitForSeconds(actionToIdleInterval);
            actionHand.SetActive(false);
            EnemyIdle();
            battleSystem.StartShoot();
            StopAllCoroutines();
        }

    }
}
