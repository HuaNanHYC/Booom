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
        yield return new WaitForSeconds(beforeActionInterval);//等待0.5秒
        EnemyAction(true);
        yield return new WaitForSeconds(actionToReadyInterval);
        EnemyReady(false,true);
        EnemyBulletTurnAudio();
        //准备开枪
        yield return new WaitForSeconds(readyToShootInterval);
        if (!if_Immute)
        {
            if (battleSystem.JudegeShoot())
            {
                EnemyShot();//中枪
                yield return new WaitForSeconds(shootToReadyInterval);
                EnemyReady(false, false);
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
        }
        else
        {
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
    }
}
