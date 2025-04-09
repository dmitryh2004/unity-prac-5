using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    public Transform player; // —сылка на скрипт здоровь€ игрока
    PlayerScore playerScore;
    public Text scoreText; // —сылка на Text

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScore = player.GetComponent<PlayerScore>();
    }

    // Update is called once per frame
    void Update()
    {
        // ќбновл€ем текст
        scoreText.text = "Score: " + playerScore.GetCurrentScore();
    }
}
