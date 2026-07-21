using UnityEngine;
using System.Collections; // จำเป็นต้องใช้สำหรับการทำงานของ Coroutine

public class ButtonSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clickSfx; // เสียงคลิกทั่วไป / เสียงเป็ด
    public AudioClip backSfx;  // เสียงตอนกด "ย้อนกลับ"
    
    // ตัวแปรเก็บค่าขนาดเริ่มต้นของเป็ดเอาไว้ เพื่อให้ยืดกลับมาเท่าเดิมได้อย่างแม่นยำ
    private Vector3 originalScale;
    private bool isScaling = false;

    private void Start()
    {
        // บันทึกขนาดเริ่มต้นของ UI เอาไว้ตอนเริ่มเกม
        originalScale = transform.localScale;
    }

    // ฟังก์ชันนี้จะถูกเรียกใช้งานผ่าน Event OnClick() ของ Button เป็ด
    public void OnDuckClicked()
    {
        // 1. เล่นเสียงเป็ด
        PlayClick();

        // 2. สั่งย่อและขยายขนาดเป็ด (ถ้ายังไม่ได้กำลังทำงานอยู่)
        if (!isScaling)
        {
            StartCoroutine(DuckScaleRoutine());
        }
    }

    // Coroutine ช่วยจับเวลา 0.2 วินาทีได้อย่างแม่นยำ
    private IEnumerator DuckScaleRoutine()
    {
        isScaling = true;

        // ย่อขนาดแกน Y ลงเหลือ 0.7 (แกน X และ Z เท่าเดิม)
        transform.localScale = new Vector3(originalScale.x, 0.7f, originalScale.z);

        // รอเวลาเป็นเวลา 0.2 วินาทีกวัก!
        yield return new WaitForSeconds(0.2f);

        // คืนค่ากลับสู่ขนาดเริ่มต้น
        transform.localScale = originalScale;

        isScaling = false;
    }

    // ฟังก์ชันเล่นเสียงคลิกมาตรฐาน
    public void PlayClick()
    {
        if (source != null && clickSfx != null)
        {
            source.PlayOneShot(clickSfx);
        }
    }

    // ฟังก์ชันเล่นเสียงกดย้อนกลับ
    public void PlayBack()
    {
        if (source != null && backSfx != null)
        {
            source.PlayOneShot(backSfx);
        }
    }

    // ฟังก์ชันพิเศษ: เล่นเสียงอะไรก็ได้ที่ส่งเข้ามา
    public void PlayCustomSound(AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.PlayOneShot(clip);
        }
    }
}
