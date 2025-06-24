using UnityEngine;
using UnityEngine.Video;

public class CutsceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public int nextSceneIndex = 1;

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (SceneLoaderr.Instance != null)
        {
            SceneLoaderr.Instance.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("SceneLoader instance is not available. Cannot load next scene.");
        }
    }
}
