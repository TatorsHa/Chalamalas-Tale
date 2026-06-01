using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextScene = "dragon_killing_you";

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    // skip video
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadSceneAsync(nextScene);
        }
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadSceneAsync(nextScene);
    }
}