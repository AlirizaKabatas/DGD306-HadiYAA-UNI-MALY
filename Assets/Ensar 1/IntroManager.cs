using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "CharacterSelect"; // Geçiþ yapýlacak sahne

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; // Video bittiðinde tetiklenir
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); // Karakter seçimine geç
    }
}

