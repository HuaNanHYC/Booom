using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int levelId;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCurrentLevel()
    {
        LevelManager.Instance.SetCurrentLevel(levelId);
    }
}
