using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
public class EnableDialogueObject : MonoBehaviour
{
    public UIDialogue uIDialogue;
    private void OnEnable()
    {
        StartCoroutine(StartDialogue());
    }
    IEnumerator StartDialogue()
    {
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
        yield return new WaitForSeconds(0.2f);
        DialogueManager.Instance.StartDialogue(dialogue);
        uIDialogue.gameObject.SetActive(true);
    }
}
