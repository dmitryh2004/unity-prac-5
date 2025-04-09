using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int currentScore; // “екущее количество очков
    void Start()
    {

    }

    public void ModifyScore(int amount)
    {
        currentScore += amount;
    }

    public int GetCurrentScore()
    {
        // ћетод дл€ получени€ текущих очков (например, дл€ отображени€ на UI)
        return currentScore;
    }
}
