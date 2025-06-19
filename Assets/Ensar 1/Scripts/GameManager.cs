using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public GameObject gameOverImage;


    public GameObject[] allCharacters; // 4 karakter prefab'ı
    public int player1Index = -1;
    public int player2Index = -1;

    public GameObject player1Instance;
    public GameObject player2Instance;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    private int deadPlayerCount = 0;
    private string currentSceneName;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ✅ Sahne adı her durumda kaydediliyor
        currentSceneName = scene.name;


        if (scene.name == "AnaLevel1")
        {

            
            if (gameOverImage != null)
                gameOverImage.SetActive(false); // sahne yüklendiğinde gizli olsun

            // Seçilen index'lere göre prefab'ları belirle
            if (player1Index >= 0 && player1Index < allCharacters.Length)
                player1Prefab = allCharacters[player1Index];
            if (player2Index >= 0 && player2Index < allCharacters.Length)
                player2Prefab = allCharacters[player2Index];

            // Spawn noktalarını bul
            if (player1SpawnPoint == null)
                player1SpawnPoint = GameObject.Find("Player1SpawnPoint")?.transform;
            if (player2SpawnPoint == null)
                player2SpawnPoint = GameObject.Find("Player2SpawnPoint")?.transform;

            // Karakterleri spawn et
            if (player1Prefab != null && player1SpawnPoint != null)
                player1Instance = Instantiate(player1Prefab, player1SpawnPoint.position, Quaternion.identity);
            if (player2Prefab != null && player2SpawnPoint != null)
                player2Instance = Instantiate(player2Prefab, player2SpawnPoint.position, Quaternion.identity);

            // PLAYER 1 CANVAS AYARI
            if (player1Instance != null)
            {
                Transform canvas1 = player1Instance.transform.Find("Canvas1");
                Transform canvas2 = player1Instance.transform.Find("Canvas2");

                if (canvas1 != null) canvas1.gameObject.SetActive(true);
                if (canvas2 != null) canvas2.gameObject.SetActive(false);
            }

            // PLAYER 2 CANVAS AYARI
            if (player2Instance != null)
            {
                Transform canvas1 = player2Instance.transform.Find("Canvas1");
                Transform canvas2 = player2Instance.transform.Find("Canvas2");

                if (canvas1 != null) canvas1.gameObject.SetActive(false);
                if (canvas2 != null) canvas2.gameObject.SetActive(true);
            }

            // ✨ PLAYER 1: Movement & Shooting ✨
            if (player1Instance != null)
            {
                var p1Move = player1Instance.GetComponent<Player1Movement>();
                var p2Move = player1Instance.GetComponent<Player2Movement>();
                var p1Shoot = player1Instance.GetComponent<Player1Shooting>();
                var p2Shoot = player1Instance.GetComponent<Player2Shooting>();

                if (p1Move != null) p1Move.enabled = true;
                if (p2Move != null) p2Move.enabled = false;

                if (p1Shoot != null) p1Shoot.enabled = true;
                if (p2Shoot != null) p2Shoot.enabled = false;
            }

            // ✨ PLAYER 2: Movement & Shooting ✨
            if (player2Instance != null)
            {
                var p1Move = player2Instance.GetComponent<Player1Movement>();
                var p2Move = player2Instance.GetComponent<Player2Movement>();
                var p1Shoot = player2Instance.GetComponent<Player1Shooting>();
                var p2Shoot = player2Instance.GetComponent<Player2Shooting>();

                if (p1Move != null) p1Move.enabled = false;
                if (p2Move != null) p2Move.enabled = true;

                if (p1Shoot != null) p1Shoot.enabled = false;
                if (p2Shoot != null) p2Shoot.enabled = true;
            }

            // Kamera hedeflerini ayarla
            MultiTargetCamera cam = Camera.main.GetComponent<MultiTargetCamera>();
            if (cam != null)
            {
                cam.targets.Clear();
                cam.targets.Add(player1Instance.transform);
                cam.targets.Add(player2Instance.transform);
            }


            deadPlayerCount = 0; // yeniden say
        }
    }


    // 🎮 Oyuncu öldüğünde çağrılır
    public void PlayerDied()
    {
        deadPlayerCount++;

        if (deadPlayerCount >= 2)
        {
            if (gameOverImage != null)
            {
                gameOverImage.SetActive(true);
            }

            Invoke(nameof(RestartScene), 4f); // resmi gösterdikten sonra 2 saniye bekleyip restart
        }
    }


    void RestartScene()
    {
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            SceneManager.LoadScene(currentSceneName);
        }
        else
        {
            // Güvenlik için fallback
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}



