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
            Debug.LogError("VideoPlayer ba�l� de�il!");
            return;
        }

        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play(); // Opsiyonel: Otomatik ba�latmazsa
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}


