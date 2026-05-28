using UnityEngine;

public class PlotTwist : MonoBehaviour
{
    private bool twistTriggered = false;

    void Update()
    {
        // Когда собраны ключевые улики, срабатывает твист
        if (!twistTriggered && InventoryManager.Instance != null)
        {
            bool hasPoison = InventoryManager.Instance.HasClue("Белый порошок в десерте");
            bool hasPlate = InventoryManager.Instance.HasClue("Тарелки перепутаны");

            if (hasPoison || hasPlate)
            {
                twistTriggered = true;
                TriggerTwist();
            }
        }
    }

    void TriggerTwist()
    {
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowNotification("Шеф подбегает к вам!");
            NotificationManager.Instance.ShowNotification("Шеф: 'Я видел! Официант сыпал что-то в десерт перед подачей!'");
        }

        // Добавляем улику
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.AddClue("Шеф видел официанта за подсыпанием порошка");
    }
}