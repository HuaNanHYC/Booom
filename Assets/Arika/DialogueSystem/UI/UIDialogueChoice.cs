// using System;
// using DialogueSystem;
// using TMPro;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// public class UIDialogueChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
// {
//     [SerializeField] private Button btnChoice;
//     [SerializeField] private GameObject selectionIndicator;
//     [SerializeField] private TextMeshProUGUI tmpChoiceText;
//     [SerializeField] private Color selectedColor = new(255, 214, 136);
//     [SerializeField] private Color unselectedColor = Color.white;
//
//     private DialogueNodeBasic _choice;
//
//     public event Action<DialogueNodeBasic> OnChoiceSelected;
//
//     public void Init(DialogueNodeBasic choice)
//     {
//         _choice = choice;
//         tmpChoiceText.text = choice.LocalizedText;
//     }
//
//
//     private void Awake()
//     {
//         btnChoice.onClick.AddListener(ButtonAction);
//         selectionIndicator.SetActive(false);
//         tmpChoiceText.color = unselectedColor;
//     }
//
//     private void ButtonAction()
//     {
//         OnChoiceSelected?.Invoke(_choice);
//     }
//
//     public void OnPointerEnter(PointerEventData eventData)
//     {
//         if (selectionIndicator) selectionIndicator.SetActive(true);
//         if (tmpChoiceText) tmpChoiceText.color = selectedColor;
//     }
//
//     public void OnPointerExit(PointerEventData eventData)
//     {
//         if (selectionIndicator) selectionIndicator.SetActive(false);
//         if (tmpChoiceText) tmpChoiceText.color = unselectedColor;
//     }
// }