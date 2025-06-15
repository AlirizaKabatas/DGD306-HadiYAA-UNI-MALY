using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "CharacterSelect"; // Ge�i� yap�lacak sahne

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; // Video bitti�inde tetiklenir
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); // Karakter se�imine ge�
    }
}

