using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPage : MonoBehaviour
{
    public Slider volunmSlider;
    public Toggle if_FullScreen;

    private void Start()
    {
        if_FullScreen.isOn = true;
    }
    private void Update()
    {
        
    }
    public void SettingsApply()
    {
        Screen.fullScreen = if_FullScreen.isOn;
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
