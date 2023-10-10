using System;
using UnityEngine;

namespace DialogueSystem.Demo
{
    public sealed class ManagerDialogueTrigger : MonoBehaviour
    {
        private DialogueManager DialogueManager => DialogueManager.Instance;

        public void SetDisplayActor(string actorIndexString)
        {
            Debug.Log($"SetCurrentActor {actorIndexString}");
            if (!int.TryParse(actorIndexString, out var actorIndex))
            {
                for (int i = 0; i < DialogueManager.actors.Length; i++)
                {
                    var actor = DialogueManager.actors[i];
                    if (actor) continue;
                    DialogueManager.actors[i] = DialogueManager.CurrentActor;
                    return;
                }
                
                return;
            }

            DialogueManager.actors[actorIndex] = DialogueManager.CurrentActor;
        }

        public void ActorLeaveDialogue()
        {
            Debug.Log("ActorLeaveDialogue");
            for (int i = 0; i < DialogueManager.actors.Length; i++)
            {
                var actor = DialogueManager.actors[i];
                if (actor != DialogueManager.CurrentActor) continue;
                DialogueManager.actors[i] = null;
                return;
            }
        }

        public void NextDialogue()//重新黑屏加载本场景，由另一个物体判断对话
        {
            LevelManager.Instance.DialogueAfterBlack();
        }
    }
}