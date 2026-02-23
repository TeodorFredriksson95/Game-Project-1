using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvas;

    private static bool cinematicPlayed = false;

    void Awake()
    {
        if(cinematicPlayed)
        {
            canvas.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        canvas.SetActive(true);
        cinematicPlayed = true;
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            videoPlayer.Stop();
            canvas.SetActive(true);
            cinematicPlayed = true;
        }
    }
}
