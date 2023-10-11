using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextButtonInDialogue : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.AudioSource2EffectSource.PlayOneShot(AudioManager.Instance.ClickMusic);
    }
}
