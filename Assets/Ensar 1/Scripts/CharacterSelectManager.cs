using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public Sprite[] characterSprites;
    public GameObject[] characterPrefabs;

    public GameObject[] highlightObjects;

    public Image player1Image;
    public Image player2Image;

    public Button startGameButton;
    public Image[] characterButtons; // Butonların Image componentleri

    private int currentIndex = 0;
    private int player1Index = -1;
    private int player2Index = -1;

    private bool player1Locked = false;
    private bool player2Locked = false;

    private void Start()
    {
        HighlightCurrent();
        startGameButton.interactable = false;
    }

    private void Update()
    {
        if (player1Locked && player2Locked) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentIndex = (currentIndex + 1) % characterSprites.Length;
            HighlightCurrent();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentIndex = (currentIndex - 1 + characterSprites.Length) % characterSprites.Length;
            HighlightCurrent();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // ENTER tuşu
        {
            SelectCharacter(currentIndex);
        }
    }

    void HighlightCurrent()
{
    for (int i = 0; i < characterButtons.Length; i++)
    {
        // Arka plandaki highlight paneli aç/kapat
        highlightObjects[i].SetActive(i == currentIndex);

        // Ek olarak seçilmiş karakterleri gri yap
        if (i == player1Index || i == player2Index)
            characterButtons[i].color = Color.gray;
        else
            characterButtons[i].color = Color.white;

        // Hafif büyütme efekti
        characterButtons[i].transform.localScale = (i == currentIndex) ? Vector3.one * 1.2f : Vector3.one;
    }
}


    void SelectCharacter(int index)
    {
        if (!player1Locked)
        {
            player1Index = index;
            player1Image.sprite = characterSprites[index];
            player1Image.color = Color.white;
            player1Locked = true;
            Debug.Log("Player 1 seçti: " + index);
        }
        else if (!player2Locked && index != player1Index)
        {
            player2Index = index;
            player2Image.sprite = characterSprites[index];
            player2Image.color = Color.white;
            player2Locked = true;
            Debug.Log("Player 2 seçti: " + index);
        }

        CheckStart();
    }

    void CheckStart()
    {
        startGameButton.interactable = (player1Locked && player2Locked);
    }

    public void StartGame()
    {
        GameManager.Instance.player1Prefab = characterPrefabs[player1Index];
        GameManager.Instance.player2Prefab = characterPrefabs[player2Index];
        SceneManager.LoadScene("GameScene");
    }
}

