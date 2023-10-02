using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
{
    public override IEnumerator EnemyShooting()
    {
        //第三次射击时免疫伤害
        if (battleSystem.BulletIndexShoot == 3)
        {
            battleSystem.bullets[battleSystem.BulletIndexShoot].actualDamage = 0;
        }
        base.EnemyShooting();
        yield return null;
    }
}
