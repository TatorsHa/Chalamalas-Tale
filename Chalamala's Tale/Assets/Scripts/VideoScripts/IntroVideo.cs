using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public AudioClip introMusic;

    IEnumerator Start()
    {
        // VIDEO
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Application.streamingAssetsPath + "/intro_v2.mp4";

        // AUDIO
        audioSource.clip = introMusic;

        // Prepare video
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null;

        // Shared start time
        double startTime = AudioSettings.dspTime + 0.2;

        // Schedule audio
        audioSource.PlayScheduled(startTime);

        // Schedule video
        videoPlayer.Play();

        // Small delay compensation
        videoPlayer.time = -1;
    }
}