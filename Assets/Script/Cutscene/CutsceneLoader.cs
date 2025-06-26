using UnityEngine;
using UnityEngine.Video;

public class CutsceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public int nextSceneIndex = 3;

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
    }
}
