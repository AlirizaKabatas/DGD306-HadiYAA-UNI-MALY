using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "CharacterSelect"; // Hedef sahne

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer baðlý deðil!");
            return;
        }

        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play(); // Opsiyonel: Otomatik baþlatmazsa
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}


