using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace DialogueSystem
{
    public sealed class UIDialogue : MonoBehaviour
    {
        [Header("Actor Name")] [SerializeField]
        private TextMeshProUGUI[] tmpActorNameDisplay = new TextMeshProUGUI[3];

        [Header("Actor Image")] [SerializeField]
        private Image[] imgActorDisplay = new Image[3];


        [Header("Text")] [SerializeField] private TextMeshProUGUI tmpDialogueText;
        [Header("Button")] [SerializeField] private Button btnNext;
        [SerializeField] private Button btnQuit;

        public bool IsActive => DialogueManager.IsInDialogue;

        private DialogueManager DialogueManager => DialogueManager.Instance;

        private void Awake()
        {
            if (btnNext)
                btnNext.onClick.AddListener(() => { DialogueManager.Next(); });
            if (btnQuit)
                btnQuit.onClick.AddListener(() => DialogueManager.QuitDialogue());
        }

        private void Start()
        {
            DialogueManager.OnConversationStarted += ConversationStarted;
            DialogueManager.OnConversationUpdated += ConversationUpdated;
            DialogueManager.OnConversationEnded += ConversationEnded;

            RefreshPanel();
        }


        private void ConversationStarted()
        {
            gameObject.SetActive(true);
        }

        private void ConversationUpdated()
        {
            RefreshPanel();
        }

        private void ConversationEnded()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            RefreshPanel();
        }

        private void OnDestroy()
        {
            if (DialogueManager)
                DialogueManager.OnConversationUpdated -= RefreshPanel;
            if (btnNext) btnNext.onClick.RemoveAllListeners();
            if (btnQuit) btnQuit.onClick.RemoveAllListeners();
        }

        private void RefreshPanel()
        {
            Debug.Log("RefreshPanel");
            if (!DialogueManager)
                return;
            transform.gameObject.SetActive(DialogueManager.IsInDialogue);
            if (!DialogueManager.IsInDialogue) return;
            RefreshDialogue();
            RefreshCharactersDisplay();
        }


        private void RefreshDialogue()
        {
            tmpDialogueText.text = DialogueManager.CurrentNodeText;
        }

        [Header("CharactersDisplay")] [SerializeField]
        private Color actorSpeakingColor = Color.white;

        [SerializeField] private Color actorNotSpeakingColor = Color.gray;


        private void RefreshCharactersDisplay()
        {
            for (int i = 0; i < DialogueManager.actors.Length; i++)
            {
                if (DialogueManager.actors[i])
                {
                    var actor = DialogueManager.actors[i];

                    var tmpActorName = tmpActorNameDisplay[i];

                    tmpActorName.text = actor.ActorName;
                    tmpActorName.transform.parent.gameObject.SetActive(true);

                    var imgActor = imgActorDisplay[i];

                    imgActor.sprite = actor.ActorSprite;
                    imgActorDisplay[i].gameObject.SetActive(true);
                }
                else
                {
                    tmpActorNameDisplay[i].text = string.Empty;
                    tmpActorNameDisplay[i].transform.parent.gameObject.SetActive(false);

                    imgActorDisplay[i].sprite = null;
                    imgActorDisplay[i].gameObject.SetActive(false);
                }

                if (DialogueManager.CurrentActor == DialogueManager.actors[i])
                {
                    tmpActorNameDisplay[i].color = actorSpeakingColor;
                    imgActorDisplay[i].color = actorSpeakingColor;
                }
                else
                {
                    tmpActorNameDisplay[i].color = actorNotSpeakingColor;
                    imgActorDisplay[i].color = actorNotSpeakingColor;
                }
            }
        }
    }
}