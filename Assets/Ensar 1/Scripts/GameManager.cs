using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public GameObject[] allCharacters; // 4 karakter prefab'ý
    public int player1Index = -1;
    public int player2Index = -1;

    public GameObject player1Instance;
    public GameObject player2Instance;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        if (scene.name == "Level2")
        {
            // Seçilen index'lere göre prefab'larý belirle
            if (player1Index >= 0 && player1Index < allCharacters.Length)
                player1Prefab = allCharacters[player1Index];
            if (player2Index >= 0 && player2Index < allCharacters.Length)
                player2Prefab = allCharacters[player2Index];

            // Spawn noktalarýný bul
            if (player1SpawnPoint == null)
                player1SpawnPoint = GameObject.Find("Player1SpawnPoint")?.transform;
            if (player2SpawnPoint == null)
                player2SpawnPoint = GameObject.Find("Player2SpawnPoint")?.transform;

            // Karakterleri spawn et
            if (player1Prefab != null && player1SpawnPoint != null)
                player1Instance = Instantiate(player1Prefab, player1SpawnPoint.position, Quaternion.identity);
            if (player2Prefab != null && player2SpawnPoint != null)
                player2Instance = Instantiate(player2Prefab, player2SpawnPoint.position, Quaternion.identity);

            // Kamera hedeflerini ayarla
            MultiTargetCamera multiCam = Camera.main.GetComponent<MultiTargetCamera>();
            if (multiCam != null)
            {
                List<Transform> camTargets = new List<Transform> {
                    player1Instance.transform,
                    player2Instance.transform
                };
                multiCam.SetTargets(camTargets);
            }
        }
    }
}
    

