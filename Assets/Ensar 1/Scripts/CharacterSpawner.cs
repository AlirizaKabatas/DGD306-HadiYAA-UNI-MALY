using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    void Start()
    {
        var gm = GameManager.Instance;
        Instantiate(gm.allCharacters[gm.player1Index], spawnPoint1.position, Quaternion.identity);
        Instantiate(gm.allCharacters[gm.player2Index], spawnPoint2.position, Quaternion.identity);
    }
}

