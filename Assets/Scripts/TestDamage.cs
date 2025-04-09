using UnityEngine;

public class TestDamage : MonoBehaviour
{
    public Transform player;
    PlayerHealth playerHealth; // Ссылка на компонент PlayerHealth

    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerHealth.TakeDamage(20); // Наносим 20 единиц урона при нажатии пробела
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth.Heal(10); // Лечим игрока на 10 единиц при нажатии H
        }
    }
}
