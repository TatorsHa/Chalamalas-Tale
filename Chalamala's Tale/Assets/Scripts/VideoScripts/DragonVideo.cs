using UnityEngine;
using UnityEngine.Video;

public class DragonVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.source = VideoSource.Url;
       videoPlayer.url = Application.streamingAssetsPath + "/dragon_video.mp4";
        videoPlayer.Play();
    }
}