using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootButton : MonoBehaviour
{
    public BattleSystem battleSystem;
    private SpriteRenderer playerSprite;
    private void Start()
    {
        playerSprite = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
    }
    public void JudgeShoot()
    {
        StartCoroutine(Shoot());
    }
    public IEnumerator Shoot()//玩家进行射击
    {
        //拿枪
        playerSprite.sprite = InventoryManager.Instance.PlayerActionImage;
        yield return new WaitForSeconds(0.5f);
        //在头顶
        playerSprite.sprite = null;
        yield return new WaitForSeconds(1f);
        //开枪
        
        if (battleSystem.JudegeShoot())
        {
            playerSprite.sprite = InventoryManager.Instance.PlayerShotImage;
        }
        //把枪放回去
        yield return new WaitForSeconds(0.5f);
        playerSprite.sprite = InventoryManager.Instance.PlayerActionImage;
        yield return new WaitForSeconds(0.5f);
        playerSprite.sprite = null;
        battleSystem.StartShoot();
    }
}
