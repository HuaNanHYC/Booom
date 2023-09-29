using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
