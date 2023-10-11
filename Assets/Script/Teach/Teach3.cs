using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teach3 : MonoBehaviour
{
    public Canvas mainCanvas;
    private void OnEnable()
    {
        if (UIManager.Instance.if_Teach3) return;
        mainCanvas.gameObject.SetActive(true);
    }
    private void Update()
    {
        if(mainCanvas.isActiveAndEnabled)
        {
            if(Input.GetMouseButtonDown(0))
            {
                mainCanvas.gameObject.SetActive(false);
            }
        }
    }
}
