using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeductionBoard : MonoBehaviour
{
    public static DeductionBoard Instance;

    [Header("=== UI ЭЛЕМЕНТЫ ===")]
    public GameObject boardPanel;           // главная панель доски
    public Transform suspectsContainer;     // контейнер для кнопок подозреваемых
    public GameObject suspectButtonPrefab;  // шаблон кнопки подозреваемого
    public Transform cluesContainer;        // контейнер для списка улик
    public GameObject clueTextPrefab;       // шаблон текста улики
    public Button accuseButton;             // кнопка обвинения
    public Text selectedSuspectText;        // текст "Выбран: ..."

    [Header("=== ПОДОЗРЕВАЕМЫЕ ===")]
    public string[] suspectNames = { "Шеф-повар", "Су-шеф", "Официант" };

    private int selectedSuspectIndex = -1;  // -1 = никто не выбран

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Скрываем панель при старте игры
        if (boardPanel != null)
            boardPanel.SetActive(false);

        CreateSuspectButtons();

        if (accuseButton != null)
            accuseButton.onClick.AddListener(OnAccuseButton);
    }

    void CreateSuspectButtons()
    {
        if (suspectsContainer == null)
        {
            Debug.LogError("SuspectsContainer не назначен!");
            return;
        }

        if (suspectButtonPrefab == null)
        {
            Debug.LogError("SuspectButtonPrefab не назначен!");
            return;
        }

        // Очищаем старые кнопки
        foreach (Transform child in suspectsContainer)
            Destroy(child.gameObject);

        // Создаём кнопку для каждого подозреваемого
        for (int i = 0; i < suspectNames.Length; i++)
        {
            int index = i; // важно для замыкания
            GameObject btn = Instantiate(suspectButtonPrefab, suspectsContainer);
            btn.GetComponentInChildren<Text>().text = suspectNames[i];
            btn.GetComponent<Button>().onClick.AddListener(() => SelectSuspect(index));
        }

        Debug.Log($"Создано {suspectNames.Length} кнопок подозреваемых");
    }

    public void SelectSuspect(int index)
    {
        selectedSuspectIndex = index;
        if (selectedSuspectText != null)
            selectedSuspectText.text = $"Выбран: {suspectNames[index]}";

        Debug.Log($"Выбран подозреваемый: {suspectNames[index]} (индекс {index})");
    }

    void UpdateCluesList()
    {
        if (cluesContainer == null || clueTextPrefab == null) return;

        // Очищаем старый список
        foreach (Transform child in cluesContainer)
            Destroy(child.gameObject);

        // Получаем все улики из инвентаря
        List<string> clues = InventoryManager.Instance.GetAllClues();

        // Создаём строку для каждой улики
        foreach (string clue in clues)
        {
            GameObject clueTextObj = Instantiate(clueTextPrefab, cluesContainer);
            clueTextObj.GetComponent<Text>().text = $"• {clue}";
        }
    }

    void Update()
    {
        // Q — открывает/закрывает ДОСКУ РАССЛЕДОВАНИЯ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (boardPanel != null)
            {
                bool isOpen = boardPanel.activeSelf;
                boardPanel.SetActive(!isOpen);

                if (!isOpen)
                    UpdateCluesList();
            }
        }
    }

    public void OnAccuseButton()
    {
        Debug.Log($"OnAccuseButton вызван. Выбранный индекс: {selectedSuspectIndex}");

        if (selectedSuspectIndex == -1)
        {
            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification("Выберите подозреваемого!");
            else
                Debug.Log("Выберите подозреваемого!");
            return;
        }

        CheckAccusation();
    }

    void CheckAccusation()
    {
        Debug.Log($"Проверка обвинения. Индекс: {selectedSuspectIndex}");

        // Официант — индекс 2 (правильный ответ)
        if (selectedSuspectIndex == 2)
        {
            bool hasPoisonClue = InventoryManager.Instance.HasClue("Белый порошок в десерте");
            bool hasPlateClue = InventoryManager.Instance.HasClue("Тарелки перепутаны");

            if (hasPoisonClue || hasPlateClue)
            {
                string ending ="       ХОРОШАЯ КОНЦОВКА\n" +
                               "Официант признался в содеянном.\n\n" +
                               "Он работал на родственников наследницы,\n" +
                               "но перепутал тарелки.\n\n" +
                               "Критик выжил. Наследница в безопасности.\n\n" +
                               "Шеф благодарит вас и угощает десертом.\n\n" +
                               "ВЫ РАСКРЫЛИ ДЕЛО!\n\n" +
                               "Нажмите кнопку, чтобы сыграть снова.";

                if (EndingManager.Instance != null)
                    EndingManager.Instance.ShowEnding(ending);
                else
                    Debug.Log(ending);
            }
            else
            {
                string ending ="      ПЛОХАЯ КОНЦОВКА \n" +
                               "Вы обвинили официанта,\n" +
                               "но улик было недостаточно.\n\n" +
                               "Суд оправдал его.\n\n" +
                               "Наследница погибла через месяц...\n\n" +
                               "Нажмите кнопку, чтобы попробовать снова.";

                if (EndingManager.Instance != null)
                    EndingManager.Instance.ShowEnding(ending);
                else
                    Debug.Log(ending);
            }
        }
        else if (selectedSuspectIndex == 0) // Шеф
        {
            string ending ="          ПЛОХАЯ КОНЦОВКА \n" +
                           "Вы обвинили шеф-повара.\n\n" +
                           "Но он был невиновен.\n\n" +
                           "Ресторан закрылся.\n" +
                           "Наследница погибла.\n\n" +
                           "ПРАВДА НЕ ВОСТОРЖЕСТВОВАЛА.\n\n" +
                           "Нажмите кнопку, чтобы попробовать снова.";

            if (EndingManager.Instance != null)
                EndingManager.Instance.ShowEnding(ending);
            else
                Debug.Log(ending);
        }
        else if (selectedSuspectIndex == 1) // Су-шеф
        {
            string ending ="        ПЛОХАЯ КОНЦОВКА \n" +
                           "Вы обвинили су-шефа.\n\n" +
                           "Он потерял работу и семью\n" +
                           "из-за ложного обвинения.\n\n" +
                           "А настоящий убийца\n" +
                           "продолжает творить зло.\n\n" +
                           "ВЫ ОШИБЛИСЬ.\n\n" +
                           "Нажмите кнопку, чтобы попробовать снова.";

            if (EndingManager.Instance != null)
                EndingManager.Instance.ShowEnding(ending);
            else
                Debug.Log(ending);
        }
    }

    // Метод для внешнего вызова (например, из твиста)
    public void ForceOpenBoard()
    {
        if (boardPanel != null)
        {
            boardPanel.SetActive(true);
            UpdateCluesList();
        }
    }

    // Метод для проверки, открыта ли доска
    public bool IsBoardOpen()
    {
        if (boardPanel != null)
            return boardPanel.activeSelf;
        return false;
    }
}