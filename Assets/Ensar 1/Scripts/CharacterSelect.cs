using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    public void SelectCharacterForPlayer1(int index)
    {
        GameManager.Instance.player1Index = index;
        Debug.Log("Player 1 seçti: " + index);
    }

    public void SelectCharacterForPlayer2(int index)
    {
        GameManager.Instance.player2Index = index;
        Debug.Log("Player 2 seçti: " + index);
    }

    public void StartGame()
    {
        if (GameManager.Instance.player1Index != -1 && GameManager.Instance.player2Index != -1)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogWarning("İki oyuncu da karakter seçmeli!");
        }
    }
}

