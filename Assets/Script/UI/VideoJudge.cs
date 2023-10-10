using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoJudge : MonoBehaviour
{
    public VideoPlayer startPlayer;
    public VideoPlayer endPlayer;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if(!LevelManager.Instance.StartVideoPlay)
        {
            startPlayer.Play();
            startPlayer.loopPointReached += OnStartVideoEnd;
            LevelManager.Instance.StartVideoPlay = true;
            return;
        }
        if(!LevelManager.Instance.EndVideoPlay)
        {
            endPlayer.Play();
            endPlayer.loopPointReached += OnEndVideoEnd;
            LevelManager.Instance.StartVideoPlay = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnStartVideoEnd(VideoPlayer vp)
    {
        if (vp == startPlayer)
        {
            LevelManager.Instance.NextLevel();
            Debug.Log("开头视频播放完成！");
        }
    }
    public void OnEndVideoEnd(VideoPlayer vp)
    {
        if (vp == endPlayer)
        {
            UIManager.Instance.LoadScene("Menu");
            Debug.Log("结尾视频播放完成！");
        }
    }
}
