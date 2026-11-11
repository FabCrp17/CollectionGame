using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    private void Update()
    {
        if (ScoreManager.Instance == null || GameManager.Instance == null) return;

        int totalSeconds = GameManager.Instance.timeRemaining.Value;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        p1ScoreText.text = $"Jugador Azul: {ScoreManager.Instance.scorePlayer1.Value}";
        p2ScoreText.text = $"Jugador Rojo: {ScoreManager.Instance.scorePlayer2.Value}";
    }
}