using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EnemyKeywordShow : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    public GameObject keywordDescriptionShow;
    private RectTransform keywordDescriptionRectTransform;//悬停显示UI的位置
    private string description;
    public string Description { get => description; set => description = value; }
    private void Start()
    {
        keywordDescriptionRectTransform = keywordDescriptionShow.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        keywordDescriptionShow.SetActive(true);
        keywordDescriptionShow.GetComponentInChildren<Text>().text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        keywordDescriptionShow?.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector2 localMousePos;
        // 获取UI元素的宽高
        float uiElementWidth = keywordDescriptionRectTransform.rect.width;
        float uiElementHeight = keywordDescriptionRectTransform.rect.height;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                keywordDescriptionRectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localMousePos))
        {
            // 根据UI元素的大小自动计算偏移量，确保不重叠
            Vector2 offset = new Vector2(uiElementWidth / 1.9f, -uiElementHeight );

            // 设置UI元素的位置
            keywordDescriptionRectTransform.localPosition = localMousePos + offset;
        }
    }
}
