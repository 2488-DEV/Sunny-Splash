using UnityEngine;
using UnityEngine.Localization;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Content")]
    public LocalizedString speakerName;
    public LocalizedString[] sentences;

    [Header("Reference")]
    public VNDialogue dialogueManager; // ลาก DialogueManage มาใส่ช่องนี้

    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // เช็คว่าใช่ Player, ยังไม่เคยเล่น และ DialogueManager ต้องว่างอยู่
        if (collision.CompareTag("Player") && !hasPlayed && dialogueManager != null)
        {
            hasPlayed = true;
            dialogueManager.isDialogue = true;

            // ส่งข้อมูลไปให้ตัวหลักสั่งทำงาน
            dialogueManager.StartTriggerDialogue(speakerName, sentences);

            // ปิดตัวเองทิ้งไปเลยกันเหนียว
            if (GetComponent<BoxCollider2D>() != null)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }
}