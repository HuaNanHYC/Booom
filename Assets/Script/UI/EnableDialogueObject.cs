using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
public class EnableDialogueObject : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(StartDialogue());
    }
    IEnumerator StartDialogue()
    {
        Dialogue dialogue = LevelManager.Instance.NextDialogueInStartDialogue();
        if (dialogue == null) yield return null;
        bool if_StartDialogue = LevelManager.Instance.If_StartDialogue;
        yield return new WaitForSeconds(0.4f);
        if (if_StartDialogue)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}
