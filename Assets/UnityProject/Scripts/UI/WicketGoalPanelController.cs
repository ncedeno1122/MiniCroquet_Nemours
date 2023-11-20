using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WicketGoalPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Team1ScoreText, m_Team2ScoreText, m_WicketsClearedText;
    public TextMeshProUGUI Team1ScoreText { get => m_Team1ScoreText; set => m_Team1ScoreText = value; }
    public TextMeshProUGUI Team2ScoreText { get => m_Team2ScoreText; set => m_Team2ScoreText = value; }
    public TextMeshProUGUI WicketsClearedText { get => m_WicketsClearedText; set => m_WicketsClearedText = value; }

    private void Awake()
    {
        
    }

    // + + + + | Functions | + + + + 

    public void UpdateGoalPanelUI(int player1Score, int player2Score, int wicketsCleared, int totalWickets) // TODO: This last field is redundant...
    {
        Team1ScoreText.text = player1Score.ToString();
        Team2ScoreText.text = player2Score.ToString();
        WicketsClearedText.text = $"{wicketsCleared} / {totalWickets}";
    }
}
