using UnityEngine;

public class ExaminableObject : MonoBehaviour
{
    [Header("Информация об объекте")]
    public string objectName = "Тарелка с едой";
    public string clueName = "Название улики";
    [TextArea] public string clueDescription = "Подробное описание улики...";

    [Header("Второй осмотр (опционально)")]
    public bool hasSecondInspection = false;
    public string secondClueName = "";
    [TextArea] public string secondClueDescription = "";

    [Header("Визуальные подсказки")]
    public GameObject pressEHint;

    private bool inspected = false;
    private bool secondInspectionUsed = false; // чтобы не было повторного второго осмотра
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (pressEHint != null)
            pressEHint.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (pressEHint != null)
                pressEHint.SetActive(true);

            if (spriteRenderer != null)
                spriteRenderer.color = new Color(1f, 1f, 0.8f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (pressEHint != null)
                pressEHint.SetActive(false);

            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Inspect();
        }
    }

    void Inspect()
    {
        // Первый осмотр
        if (!inspected)
        {
            // Добавляем улику
            InventoryManager.Instance.AddClue(clueName, clueDescription);
            inspected = true;

            
            Debug.Log($"🔍 Осмотр: {objectName} → Улика: {clueName}");
            return;
        }

        // Второй осмотр (только если он включён и ещё не использован)
        if (hasSecondInspection && !secondInspectionUsed)
        {
            InventoryManager.Instance.AddClue(secondClueName, secondClueDescription);
            secondInspectionUsed = true;

            if (NotificationManager.Instance != null)
                NotificationManager.Instance.ShowNotification($"📌 {secondClueName}");

            Debug.Log($"🔍 Повторный осмотр: {objectName} → Улика: {secondClueName}");
            return;
        }

        // Если уже всё осмотрели
        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification($"Вы уже осмотрели {objectName}");
    }
}