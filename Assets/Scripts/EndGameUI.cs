using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public GameObject endGamePanel;
    public TextMeshProUGUI winnerText;
    public Button menuButton;

    private void Start()
    {
        menuButton.onClick.AddListener(ReturnToMenu);
        endGamePanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.timeRemaining.Value <= 0 && !endGamePanel.activeSelf && !GameManager.Instance.isGameActive.Value)
        {
            ShowWinner();
        }
    }

    private void ShowWinner()
    {
        endGamePanel.SetActive(true);
        int p1Score = ScoreManager.Instance.scorePlayer1.Value;
        int p2Score = ScoreManager.Instance.scorePlayer2.Value;

        if (p1Score > p2Score) winnerText.text = "¡GANADOR! JUGADOR AZUL";
        else if (p2Score > p1Score) winnerText.text = "¡GANADOR! JUGADOR ROJO";
        else winnerText.text = "¡EMPATE!";
    }

    private void ReturnToMenu()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}