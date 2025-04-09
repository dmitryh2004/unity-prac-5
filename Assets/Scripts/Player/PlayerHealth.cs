using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public AudioSource damageAudioSource;
    public AudioSource deathAudioSource;
    public Image interfaceBorderImage;
    public int maxHealth = 100; // Максимальное количество здоровья
    private int currentHealth; // Текущее количество здоровья
    public GameObject UICanvas;
    public GameObject gameOverCanvas; // Панель проигрыша
    public Text gameOverScoreText;
    public Button restartButton; // Кнопка перезапуска

    void Start()
    {
        // Устанавливаем текущее здоровье равным максимальному при старте игры
        currentHealth = maxHealth;
        gameOverCanvas.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);

        // Обновляем элемент интерфейса
        UpdateInterfaceBorder();
    }

    public void TakeDamage(int damage)
    {
        damageAudioSource.Play();
        // Уменьшаем текущее здоровье на величину урона
        currentHealth -= damage;

        // Проверяем, если здоровье меньше или равно 0
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Убедимся, что здоровье не уходит в отрицательные значения
            GameOver(); // Вызываем метод смерти игрока
        }

        // Обновляем элемент интерфейса
        UpdateInterfaceBorder();
    }

    public void Heal(int amount)
    {
        // Увеличиваем здоровье игрока на указанное значение
        currentHealth += amount;

        // Убедимся, что здоровье не превышает максимум
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Обновляем элемент интерфейса
        UpdateInterfaceBorder();
    }

    void UpdateInterfaceBorder()
    {
        Color color = interfaceBorderImage.color;
        color.a = 1f - (((float) currentHealth) / maxHealth);
        interfaceBorderImage.color = color;
    }

    public int GetCurrentHealth()
    {
        // Метод для получения текущего здоровья (например, для отображения на UI)
        return currentHealth;
    }

    private void GameOver()
    {
        deathAudioSource.Play();
        UICanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0; // Остановка времени для паузы
        
        int score = gameObject.GetComponent<PlayerScore>().GetCurrentScore();
        gameOverScoreText.text = "Total Score: " + score;
        Cursor.lockState = CursorLockMode.None;
    }

    private void RestartGame()
    {
        Time.timeScale = 1; // Восстановление времени
        gameOverCanvas.SetActive(false);
        currentHealth = maxHealth; // Восстановление HP

        // Перезапуск сцены
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
