using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 4)] public string text;
}

public class NPCDialogue : MonoBehaviour
{
    [Header("Информация о персонаже")]
    public string characterName = "Шеф-повар";
    public Sprite characterPortrait;

    [Header("Диалоговые линии")]
    public DialogueLine[] dialogueLines;

    [Header("Улики, которые даёт персонаж")]
    public string[] cluesToGive;

    private bool hasTalked = false;
    private GameObject talkIcon;

    void Start()
    {
        talkIcon = transform.Find("TalkIcon").gameObject;
        if (talkIcon != null)
            talkIcon.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (talkIcon != null)
                talkIcon.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (talkIcon != null)
                talkIcon.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        DialogueManager.Instance.StartDialogue(this);
    }

    public void GiveClues()
    {
        if (!hasTalked)
        {
            // Проверяем, существует ли InventoryManager
            if (InventoryManager.Instance == null)
            {
                Debug.LogError("InventoryManager не найден в сцене!");
                return;
            }

            foreach (string clue in cluesToGive)
            {
                if (!string.IsNullOrEmpty(clue))
                {
                    InventoryManager.Instance.AddClue(clue, clue);
                    Debug.Log($"Улика добавлена: {clue}");
                }
            }
            hasTalked = true;
        }
    }
}
