using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI элементы (перетащи сюда из сцены)")]
    public GameObject dialoguePanel;
    public Text speakerNameText;
    public Text dialogueText;
    public Button nextButton;

    private NPCDialogue currentNPC;
    private int currentLineIndex;
    private bool isDialogueActive = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    public void StartDialogue(NPCDialogue npc)
    {
        currentNPC = npc;
        currentLineIndex = 0;
        isDialogueActive = true;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        ShowCurrentLine();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = false;
        }
    }

    void ShowCurrentLine()
    {
        if (currentLineIndex < currentNPC.dialogueLines.Length)
        {
            var line = currentNPC.dialogueLines[currentLineIndex];
            if (speakerNameText != null)
                speakerNameText.text = line.speakerName;
            if (dialogueText != null)
                dialogueText.text = line.text;
        }
        else
        {
            EndDialogue();
        }
    }

    public void NextLine()
    {
        currentLineIndex++;
        ShowCurrentLine();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (currentNPC != null)
            currentNPC.GiveClues();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = true;
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartConfessionDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (speakerNameText != null)
            speakerNameText.text = "Официант";

        if (dialogueText != null)
            dialogueText.text = "Ладно... Я признаюсь. Это я подсыпал яд в десерт. Мне заплатили родственники наследницы. Но я не хотел убивать критика! Я перепутал тарелки... Простите.";

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = false;
        }

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        StartCoroutine(AutoCloseConfession());
    }

    IEnumerator AutoCloseConfession()
    {
        yield return new WaitForSeconds(5);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (nextButton != null)
            nextButton.gameObject.SetActive(true);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = true;
        }
    }
}