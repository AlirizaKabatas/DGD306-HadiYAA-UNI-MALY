using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string introSceneName = "IntroScene"; // Intro sahnesinin ismini buraya yaz
    public AudioSource clickSound; // Buton sesi varsa

    public void OnPlayPressed()
    {
        if (clickSound != null)
            clickSound.Play();

        SceneManager.LoadScene(introSceneName);
    }

    public void OnQuitPressed()
    {
        if (clickSound != null)
            clickSound.Play();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}


