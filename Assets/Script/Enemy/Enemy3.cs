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
        EnemyAction(true);
        yield return new WaitForSeconds(0.5f);
        EnemyReady(false);
        //准备开枪
        yield return new WaitForSeconds(0.5f);
        if (!if_Immute)
        {
            if (battleSystem.JudegeShoot())
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
        else
        {
            battleSystem.JudegeShoot();
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
