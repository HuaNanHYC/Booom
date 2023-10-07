using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPage : MonoBehaviour
{
    public Slider volunmSlider;
    public Toggle if_FullScreen;
    private bool if_FullScreenOn;

    private void Start()
    {
        if_FullScreen.isOn = false;
        if_FullScreenOn = false;
    }
    private void Update()
    {
        
    }
    public void SettingsApply()
    {
        if (if_FullScreen.isOn && !if_FullScreenOn)
        {
            Screen.fullScreen = true;
            if_FullScreenOn = true;
        }
        else if(!if_FullScreen.isOn && if_FullScreenOn)
        {
            Screen.fullScreen = false;
            if_FullScreenOn = false;
        }
        AudioManager.Instance.SetVolumn(volunmSlider.value);
    }
    public void BackToLevelSelect()
    {
        UIManager.Instance.LoadScene("LevelScene");
    }
    public void SetVolumn(bool if_Silence)
    {
        if (if_Silence)
            AudioManager.Instance.SetVolumn(0);
        else AudioManager.Instance.SetVolumn(volunmSlider.value);
    }
}
