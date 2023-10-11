using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootButton : MonoBehaviour
{
    public BattleSystem battleSystem;
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer playerHurtSpriteRenderer;
    private GameObject gunSprite;
    private float beforeActionIntervalPlayer;
    private float actionToReadyIntervalPlayer;
    private float readyToShootIntervalPlayer;
    private float shootToReadyIntervalPlayer;
    private float readyToActionIntervalPlayer;
    private float actionToIdleIntervalPlayer;
    private void Start()
    {
        playerSpriteRenderer = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        playerHurtSpriteRenderer = playerSpriteRenderer.transform.GetChild(0).GetComponent<SpriteRenderer>();
        gunSprite = GameObject.FindWithTag("Gun");

        beforeActionIntervalPlayer = UIManager.Instance.beforeActionIntervalPlayer;
        actionToReadyIntervalPlayer = UIManager.Instance.actionToReadyIntervalPlayer;
        readyToShootIntervalPlayer = UIManager.Instance.readyToShootIntervalPlayer;
        shootToReadyIntervalPlayer = UIManager.Instance.shootToReadyIntervalPlayer;
        readyToActionIntervalPlayer = UIManager.Instance.readyToActionIntervalPlayer;
        actionToIdleIntervalPlayer = UIManager.Instance.actionToIdleIntervalPlayer;
    }
    public void SetAuto()
    {
        battleSystem.if_AutoShoot = true;
    }
    public void JudgeShoot()
    {
        StartCoroutine(Shoot());
    }
    public IEnumerator Shoot()//玩家进行射击
    {
        //拿枪
        yield return new WaitForSeconds(beforeActionIntervalPlayer);
        playerSpriteRenderer.sprite = InventoryManager.Instance.PlayerActionImage;
        yield return new WaitForSeconds(actionToReadyIntervalPlayer);
        //在头顶
        playerSpriteRenderer.sprite = null;
        gunSprite.SetActive(false);
        yield return new WaitForSeconds(readyToShootIntervalPlayer);
        //开枪
        if(InventoryManager.Instance.If_Immute)
        {
            playerHurtSpriteRenderer.sprite = null;
            battleSystem.JudegeShoot();
        }
        else if (battleSystem.JudegeShoot())
        {
            playerHurtSpriteRenderer.sprite = InventoryManager.Instance.PlayerHurtImage;
        }
        //把枪放回去
        yield return new WaitForSeconds(shootToReadyIntervalPlayer);
        playerHurtSpriteRenderer.sprite = null;
        yield return new WaitForSeconds(readyToActionIntervalPlayer);
        playerSpriteRenderer.sprite = InventoryManager.Instance.PlayerActionImage;
        gunSprite.SetActive(true);
        yield return new WaitForSeconds(actionToIdleIntervalPlayer);
        playerSpriteRenderer.sprite = null;
        battleSystem.StartShoot();
    }
}
