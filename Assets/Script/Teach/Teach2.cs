using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teach2 : MonoBehaviour
{
    public Canvas mainCanvas;
    public bool if_Start;

    private void Start()
    {
        if (UIManager.Instance.if_Teach2) mainCanvas.gameObject.SetActive(false);
    }
    public void SetIfStart()
    {
        if_Start = true;
    }
    private void Update()
    {
        if (UIManager.Instance.if_Teach2) return;
        if (mainCanvas.isActiveAndEnabled && Input.GetMouseButtonDown(0))
        {
            UIManager.Instance.if_Teach2 = true;
            mainCanvas.gameObject.SetActive(false);
        }
        if(if_Start)
        {
            if_Start = false;
            StartCoroutine(WaitForBulletPageOpen());
        }
    }
    IEnumerator WaitForBulletPageOpen()
    {
        yield return new WaitForSeconds(0.6f);
        mainCanvas.gameObject.SetActive(true);
    }

}
