using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
public class EnableDialogueObject : MonoBehaviour
{
    public UIDialogue uIDialogue;
    [Header("停留的图片")]
    public GameObject continueSprite;
    public Canvas backGround;
    private bool ifPreviousContinue;
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
            ifPreviousContinue = true;
            yield return new WaitForSeconds(2);
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
        //bgm播放
        if (AudioManager.Instance.AudioSource1MainSource.clip != AudioManager.Instance.DialogueMusic)
        {
            AudioManager.Instance.AudioSource1MainSource.clip = AudioManager.Instance.DialogueMusic;
            AudioManager.Instance.AudioSource1MainSource.Play();
        }


        DialogueManager.Instance.StartDialogue(dialogue);
        continueSprite.SetActive(false);
        uIDialogue.gameObject.SetActive(true);
        //先出背景再出对话
        if (!ifPreviousContinue)
        {
            backGround.sortingOrder = 101;
            yield return new WaitForSeconds(1);
            backGround.sortingOrder = 0;
        }
    }
}
