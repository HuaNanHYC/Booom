using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DialogueSystem.Demo
{
    public sealed class ManagerDialogueTrigger : MonoBehaviour
    {
        private DialogueManager DialogueManager => DialogueManager.Instance;

        private DialogueTrigger DialogueTrigger { get; set; }

        private void Awake()
        {
            DialogueTrigger = GetComponent<DialogueTrigger>();

            // SetDisplayActor
            var setDisplayActorEvent = new UnityEvent<string[]>();
            setDisplayActorEvent.AddListener(SetDisplayActor);
            DialogueTrigger.Triggers.Add(
                new DialogueSingleTrigger
                {
                    actionName = nameof(SetDisplayActor),
                    triggerEvent = setDisplayActorEvent
                });

            // ActorLeaveDialogue
            var actorLeaveDialogueEvent = new UnityEvent<string[]>();
            actorLeaveDialogueEvent.AddListener(ActorLeaveDialogue);
            DialogueTrigger.Triggers.Add(
                new DialogueSingleTrigger
                {
                    actionName = nameof(ActorLeaveDialogue),
                    triggerEvent = actorLeaveDialogueEvent
                });

            // ChangeActorSprite
            var changeSpriteEvent = new UnityEvent<string[]>();
            changeSpriteEvent.AddListener(ChangeActorSprite);
            DialogueTrigger.Triggers.Add(
                new DialogueSingleTrigger
                {
                    actionName = nameof(ChangeActorSprite),
                    triggerEvent = changeSpriteEvent
                });
            Debug.Log($"Add {nameof(ChangeActorSprite)} to {gameObject.name}");
            
            // ChangeBackgroundSprite
            var changeBackgroundSpriteEvent = new UnityEvent<string[]>();
            changeBackgroundSpriteEvent.AddListener(ChangeBackgroundSprite);
            DialogueTrigger.Triggers.Add(
                new DialogueSingleTrigger
                {
                    actionName = nameof(ChangeBackgroundSprite),
                    triggerEvent = changeBackgroundSpriteEvent
                });
        }

        public void SetDisplayActor(string[] args)
        {
            var actorIndexString = args.Length < 1 ? string.Empty : args[0];
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

        public void ActorLeaveDialogue(string[] _)
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
        
        public void ChangeActorSprite(string[] args)
        {
            if (args.Length < 1) return;
            var spriteIndexString = args[0];
            Debug.Log($"ChangeActorSprite {spriteIndexString}");
            if (int.TryParse(spriteIndexString, out var spriteIndex))
            {
                DialogueManager.CurrentActor.CurrentSpriteIndex = spriteIndex;
            }
        }

        [SerializeField] private Image imgTest;

        [SerializeField] private Sprite[] testSprites;
        
        
        public void ChangeBackgroundSprite(string[] args)
        {
            if (args.Length < 1) return;
            var spriteIndexString = args[0];
            Debug.Log($"ChangeBackgroundSprite {spriteIndexString}");
            if (int.TryParse(spriteIndexString, out var spriteIndex))
            {
                imgTest.sprite = testSprites[spriteIndex];
            }
        }
    }
}