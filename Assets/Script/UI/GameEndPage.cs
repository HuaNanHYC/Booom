using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPage : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;
    public void Win()
    {
        win.SetActive(true);
    }
    public void Lose()
    {
        lose.SetActive(true);
    }
    public void RestartButton()
    {
        UIManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReturnLevelSelectScene()
    {
        UIManager.Instance.LoadScene("Menu");
    }
    
}
