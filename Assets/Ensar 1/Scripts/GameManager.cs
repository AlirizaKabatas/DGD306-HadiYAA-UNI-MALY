using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public GameObject[] allCharacters; // 4 prefab
    public int player1Index = -1;
    public int player2Index = -1;

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
        if (scene.name == "GameScene")
        {
            if (player1SpawnPoint == null)
                player1SpawnPoint = GameObject.Find("Player1SpawnPoint")?.transform;
            if (player2SpawnPoint == null)
                player2SpawnPoint = GameObject.Find("Player2SpawnPoint")?.transform;

            if (player1Prefab != null && player1SpawnPoint != null)
                Instantiate(player1Prefab, player1SpawnPoint.position, Quaternion.identity);
            if (player2Prefab != null && player2SpawnPoint != null)
                Instantiate(player2Prefab, player2SpawnPoint.position, Quaternion.identity);
        }
    }
}