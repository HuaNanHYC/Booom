using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    int[] steadyBullet;
    int[] steadyPos;
    private void OnEnable()
    {
        if (LevelManager.Instance == null) return;
        steadyBullet = LevelManager.Instance.GetCurentLevelSteadyBulletId();
        steadyPos = LevelManager.Instance.GetCurentLevelSteadyBulletPosition();
        SetHole();
    }
    public void SetHole()//根据关卡配置设置当前的弹匣的子弹
    {
        for(int i = 0; i < steadyBullet.Length; i++)
        {
            transform.GetChild(steadyPos[i]-1).GetComponent<BulletHole>().LoadBulletAuto(steadyBullet[i]);
        }
    }
}
