using UnityEngine;
using TMPro;
using System.Collections;

public class VNDialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI contentText;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip typingSound;
    public AudioClip nextClickSound;

    [Header("Text Settings")]
    [TextArea(3, 10)]
    public string[] sentences;
    public float typingSpeed = 0.04f;

    [Header("Trigger Settings")]
    public bool playOnStart = false;
    private bool hasPlayed = false;
    private Coroutine typingCoroutine;

    private int index;
    private bool isTyping;

    void Start()
    {
        dialogueBox.SetActive(false);
        isTyping = false;
        index = 0;

        if (playOnStart)
        {
            StartConversation();
        }
    }

    // --- ฟังก์ชันใหม่สำหรับรับข้อมูลจากสคริปต์ DialogueTrigger (บ่อน้ำ) ---
    public void StartTriggerDialogue(string name, string[] newSentences)
    {
        nameText.text = name;
        sentences = newSentences;

        hasPlayed = true;
        index = 0;
        dialogueBox.SetActive(true);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(sentences[index]));
    }

    void Update()
    {
        if (dialogueBox.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                FinishLineImmediately();
            }
            else
            {
                if (nextClickSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(nextClickSound);
                }
                NextSentence();
            }
        }
    }

    // ใช้สำหรับ Trigger ภายในตัวเอง (ถ้ามี)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasPlayed && !dialogueBox.activeInHierarchy)
        {
            StartConversation();
            if (GetComponent<BoxCollider2D>() != null)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    void StartConversation()
    {
        hasPlayed = true;
        index = 0;
        dialogueBox.SetActive(true);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(sentences[index]));
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        contentText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            if (!dialogueBox.activeInHierarchy) yield break;

            contentText.text += letter;

            if (letter != ' ' && typingSound != null && audioSource != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(typingSound);
                }
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void FinishLineImmediately()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        contentText.text = sentences[index];
        isTyping = false;
    }

    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(sentences[index]));
        }
        else
        {
            dialogueBox.SetActive(false);
        }
    }
}