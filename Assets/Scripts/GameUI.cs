using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    public Button exitToMenuButton;

    private void Start()
    {
        if (exitToMenuButton != null)
        {
            exitToMenuButton.onClick.AddListener(ExitToMenu);
        }
    }

    private void Update()
    {
        if (ScoreManager.Instance == null || GameManager.Instance == null) return;

        if (GameManager.Instance.isWaitingForPlayers.Value)
        {
            timeText.text = "Esperando al Jugador 2...";
        }
        else
        {
            int totalSeconds = GameManager.Instance.timeRemaining.Value;
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        p1ScoreText.text = $"Jugador Azul: {ScoreManager.Instance.scorePlayer1.Value}";
        p2ScoreText.text = $"Jugador Rojo: {ScoreManager.Instance.scorePlayer2.Value}";
    }

    private void ExitToMenu()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }
        SceneManager.LoadScene(0);
    }
}