using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartGame : MonoBehaviour
{
    Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        ButtonOnClick();
    }
    public void StartTheGame()
    {
        UIManager.Instance.LoadScene("StartAndEnd");
    }
    public void ContinueTheGame()
    {
        LevelManager.Instance.LoadTheGame();
        if (LevelManager.Instance.currentLevelId == 30000)
        {
            StartTheGame();
            return;
        }
        else
        {
            LevelManager.Instance.DialogueAfterBlack();
        }
    }
    public void ButtonOnClick()
    {
    }
}
