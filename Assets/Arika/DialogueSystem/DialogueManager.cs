#define ENABLE_LOGS

using System;
using System.Collections.Generic;
using System.Linq;
using Condition;
using UnityEngine;

namespace DialogueSystem
{
    public sealed class DialogueManager : Singleton<DialogueManager>
    {
        private Dialogue CurrentDialogue { get; set; }
        public DialogueNode CurrentNode { get; set; }

        public DialogueActor FirstActor { get; set; }
        public DialogueActor SecondActor { get; set; }
        public DialogueActor ThirdActor { get; set; }
        
        [Header("Dialogue Event Channel")] [SerializeField]
        private DialogueEventChannel dialogueEventChannel;

        [Tooltip("ScriptableObject represents the State that Player is in dialogue (Set it to InDialogue by default)")]
        [SerializeField]
        private DialogueState dialogueStateInDialogue;

        private DialogueState _cachedDialogueState;

        public event Action OnConversationStarted;
        public event Action OnConversationUpdated;
        public event Action OnConversationEnded;


        [SerializeField] private ConditionEvaluator conditionEvaluator;
        [SerializeField] private DialogueStateMachine dialogueStateMachine;


        private void OnEnable()
        {
            CurrentDialogue = null;
            CurrentNode = null;
        }

        public void StartDialogue(Dialogue newDialogue)
        {
            if (CurrentDialogue)
                return;

            this.Log("Starting dialogue with " + newDialogue.name);
            OnConversationStarted?.Invoke();

            _cachedDialogueState = dialogueStateMachine.CurrentState;
            dialogueEventChannel.RaiseDialogueStateRequest(dialogueStateInDialogue);

            CurrentDialogue = newDialogue;
            CurrentNode = CurrentDialogue.GetRootNode();
            TriggerNodeEnterAction();
            OnConversationUpdated?.Invoke();
            Next();
        }

        public void QuitDialogue()
        {
            CurrentDialogue = null;
            TriggerNodeExitAction();
            CurrentNode = null;
            OnConversationUpdated?.Invoke();
            OnConversationEnded?.Invoke();
            dialogueEventChannel.RaiseDialogueStateRequest(_cachedDialogueState);
            _cachedDialogueState = null;
        }

        public bool IsInDialogue => CurrentDialogue is not null;

        public string CurrentNodeText => CurrentNode is DialogueNodeBasic basicNode
            ? basicNode.Text
            : null;

        public string CurrentActorName => (CurrentNode is DialogueNodeBasic basicNode) ? basicNode.ActorName : null;

        public void Next()
        {
            DialogueNodeBasic[] aiChildren = FilterByCondition(CurrentDialogue.GetAllChildren(CurrentNode))
                .Select(x => (DialogueNodeBasic) x).ToArray();
            if (aiChildren.Length == 0)
            {
                QuitDialogue();
                return;
            }

            //Advance To Next Node

            int randomIndex = UnityEngine.Random.Range(0, aiChildren.Length);
            TriggerNodeExitAction();
            CurrentNode = aiChildren[randomIndex];
            TriggerNodeEnterAction();
            OnConversationUpdated?.Invoke();
        }

        public bool HasNext() => FilterByCondition(CurrentDialogue.GetAllChildren(CurrentNode)).Any();

        private IEnumerable<DialogueNode> FilterByCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (conditionEvaluator.Evaluate(node.Condition))
                    yield return node;
            }
        }


        #region Node Actions

        private void TriggerNodeEnterAction()
        {
            if (CurrentNode is DialogueNodeBasic basicNode) TriggerNodeActions(basicNode.OnEnterActions);
        }

        private void TriggerNodeExitAction()
        {
            if (CurrentNode is DialogueNodeBasic basicNode) TriggerNodeActions(basicNode.OnExitActions);
        }

        private void TriggerNodeActions(IReadOnlyList<NodeAction> actions)
        {
            if (actions.Count == 0) return;

            // var dialogueInteractableDialogueTrigger = CurrentDialogueInteractable.GetComponent<DialogueTrigger>();
            // var playerDialogueTrigger = PlayerDialogueComponent.GetComponent<DialogueTrigger>();
            //
            // foreach (var action in actions)
            // {
            //     if (action.isPlayerAction)
            //     {
            //         if (playerDialogueTrigger) playerDialogueTrigger.TriggerAction(action);
            //         break;
            //     }
            //
            //     dialogueInteractableDialogueTrigger.TriggerAction(action);
            // }
        }

        #endregion
    }
}