using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public GameObject notificationPanel;
    public Text notificationText;
    public float displayTime = 3f;

    private Queue<string> notificationQueue = new Queue<string>();
    private bool isShowing = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        notificationQueue.Enqueue(message);
        if (!isShowing)
            StartCoroutine(ShowNextNotification());
    }

    IEnumerator ShowNextNotification()
    {
        isShowing = true;

        while (notificationQueue.Count > 0)
        {
            string message = notificationQueue.Dequeue();
            notificationText.text = message;
            notificationPanel.SetActive(true);

            yield return new WaitForSeconds(displayTime);
            notificationPanel.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }

        isShowing = false;
    }
}