using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;
    private bool isMuted = false;
    private float previousVolume = 0.5f;

    [Header("UI")]
    public Button muteButton;
    public Text muteButtonText;
    public string soundOnText = "🔊";
    public string soundOffText = "🔇";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();

        // Подключаем кнопку
        if (muteButton != null)
            muteButton.onClick.AddListener(ToggleMute);

        UpdateButtonText();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (audioSource != null)
        {
            if (isMuted)
            {
                previousVolume = audioSource.volume;
                audioSource.volume = 0f;
            }
            else
            {
                audioSource.volume = previousVolume;
            }
        }

        UpdateButtonText();
        Debug.Log($"Звук: {(isMuted ? "ВЫКЛЮЧЕН" : "ВКЛЮЧЕН")}");
    }

    public void SetVolume(float volume)
    {
        if (!isMuted && audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
            previousVolume = audioSource.volume;
        }
    }

    void UpdateButtonText()
    {
        if (muteButtonText != null)
        {
            muteButtonText.text = isMuted ? soundOffText : soundOnText;
        }
    }
}