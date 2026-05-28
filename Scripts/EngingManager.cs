using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance;

    [Header("=== UI ЭЛЕМЕНТЫ ===")]
    public GameObject endingPanel;      // чёрная панель
    public Text endingText;             // текст концовки
    public Button restartButton;        // кнопка

    private string currentSceneName;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (endingPanel != null)
            endingPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void ShowEnding(string endingTextString)
    {
        Debug.Log("ShowEnding вызван!");
        Debug.Log("Текст концовки: " + endingTextString);

        // Проверяем, что панель существует
        if (endingPanel == null)
        {
            Debug.LogError("EndingPanel не назначен в EndingManager!");
            return;
        }

        // Проверяем, что текст существует
        if (endingText == null)
        {
            Debug.LogError("EndingText не назначен в EndingManager!");
            return;
        }

        // Устанавливаем текст
        endingText.text = endingTextString;

        // Показываем панель
        endingPanel.SetActive(true);

        // Останавливаем игру (опционально)
        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        // Возвращаем время
        Time.timeScale = 1f;
        // Перезапускаем текущую сцену
        SceneManager.LoadScene(currentSceneName);
    }
}