using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
public class EnableDialogueObject : MonoBehaviour
{
    public UIDialogue uIDialogue;
    [Header("Í£ÁôµÄÍ¼Æ¬")]
    public GameObject continueSprite;
    private void OnEnable()
    {
        StartCoroutine(StartDialogue());
    }
    IEnumerator StartDialogue()
    {
        if(LevelManager.Instance.ContinueSprite)
        {
            continueSprite.SetActive(true);
            LevelManager.Instance.ContinueSprite = false;
            yield return new WaitForSeconds(1);
        }


        Dialogue dialogue = null;
        bool if_StartDialogue = LevelManager.Instance.If_StartDialogue;
        if(if_StartDialogue)
            dialogue = LevelManager.Instance.NextDialogueInStartDialogue();
        else
            dialogue = LevelManager.Instance.NextDialogueInEndDialogue();
        if (dialogue == null)
        {
            yield return null;
            StopAllCoroutines();
        }
        yield return new WaitForSeconds(0.1f);
        DialogueManager.Instance.StartDialogue(dialogue);
        continueSprite.SetActive(false);
        uIDialogue.gameObject.SetActive(true);
    }
}
