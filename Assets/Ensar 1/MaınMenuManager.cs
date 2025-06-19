using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string introSceneName = "IntroScene"; // Intro sahnesinin ismini buraya yaz
    public string optionsSceneName = "OptionsScene"; // Yeni sahne

    public string creditsSceneName = "CreditsScene"; // Yeni sahne

    public string mainMenuSceneName = "MainMenu"; // Ana menü sahne ismi
    public AudioSource clickSound; // Buton sesi varsa

    

    public void OnPlayPressed()
    {
        if (clickSound != null)
            clickSound.Play();

        SceneManager.LoadScene(introSceneName);
    }

    public void OnOptionsPressed()
    {
        if (clickSound != null)
            clickSound.Play();

        SceneManager.LoadScene(optionsSceneName);
    }
     public void OnCreditsPressed()
    {
        if (clickSound != null)
            clickSound.Play();

        SceneManager.LoadScene(creditsSceneName);
    }
    public void OnBackToMenuPressed()
    {
        if (clickSound != null)
            clickSound.Play();

        SceneManager.LoadScene(mainMenuSceneName);
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



