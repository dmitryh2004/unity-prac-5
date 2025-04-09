using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    public Transform player; // —сылка на скрипт здоровь€ игрока
    PlayerHealth playerHealth;
    public Text healthText; // —сылка на Text

    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();    
    }
    void Update()
    {
        // ќбновл€ем текст
        healthText.text = "HP: " + playerHealth.GetCurrentHealth() + " / " + playerHealth.maxHealth;
    }
}
