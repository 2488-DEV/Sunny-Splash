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

    private int index;
    private bool isTyping;

    void Start()
    {
        dialogueBox.SetActive(true);
        StartCoroutine(TypeText(sentences[index]));
    }

    void Update()
    {
        // แก้ไขตรงนี้: ลบ Input.GetMouseButtonDown(0) ออก เหลือแค่ Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                contentText.text = sentences[index];
                isTyping = false;
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

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        contentText.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            contentText.text += line[i];

            if (line[i] != ' ' && typingSound != null && audioSource != null)
            {
                if (i % 2 == 0 && !audioSource.isPlaying)
                {
                    audioSource.clip = typingSound;
                    audioSource.Play();
                }
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            StartCoroutine(TypeText(sentences[index]));
        }
        else
        {
            dialogueBox.SetActive(false);
        }
    }
}