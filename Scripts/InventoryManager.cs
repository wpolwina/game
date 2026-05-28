using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("=== UI ЭЛЕМЕНТЫ ===")]
    public GameObject cluesPanel;  // панель улик (CluesPanel)

    private List<string> clues = new List<string>();
    private List<string> clueDescriptions = new List<string>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (cluesPanel != null)
            cluesPanel.SetActive(false);
    }

    public void AddClue(string clueName, string description = "")
    {
        if (!clues.Contains(clueName))
        {
            clues.Add(clueName);
            clueDescriptions.Add(description);
            Debug.Log($"🔍 Улика добавлена: {clueName}");

            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification($"📌 {clueName}");
        }
    }

    public bool HasClue(string clueName)
    {
        return clues.Contains(clueName);
    }

    public List<string> GetAllClues()
    {
        return new List<string>(clues);
    }

    void Update()
    {
        // Tab — открывает/закрывает панель УЛИК
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cluesPanel != null)
            {
                cluesPanel.SetActive(!cluesPanel.activeSelf);
            }
        }
    }
}