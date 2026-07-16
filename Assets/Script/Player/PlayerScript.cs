using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem; // จำเป็นต้องมีเพื่อใช้ Coroutine กวัก!

public class PlayerScript : MonoBehaviour
{
    public bool IsShovel;
    public bool isEgg;
    private VNDialogue dialogueManager;

    [Header("Level Settings")]
    [Tooltip("ใส่เลขด่านปัจจุบัน เช่น ด่าน 1 ใส่เลข 1 กวัก")]
    public int currentLevelIndex;

    [Header("Status Settings")]
    public int seed;
    public TextMeshProUGUI seedCount;

    public int tree;
    public TextMeshProUGUI treeCount;

    public int egg;
    public TextMeshProUGUI eggCount;
    private Vector3 originalPosition;
    public GameObject eggBullet;

    [Header("Victory Settings")]
    public GameObject victoryPanel;

    [Header("Movement State")]
    public bool isLeft;
    public bool isRight;

    [Header("Player Health")]
    public int playerHp;
    public int playerMaxHp = 3;
    public GameObject deathPanel;
    public PlayerSound playerSound;
    public GameObject player;
    public GameObject enemy; // ตัวเชื่อมกับศัตรูเพื่อปิดเมื่อผู้เล่นตาย


    private WaterRefillSystem waterSystem;
    private bool isWaitingForVictory = false; // ป้องกันการเรียก Coroutine ซ้ำกวัก

    [SerializeField] public Texture2D cursorTexture;
    private Vector2 cursorHotSpot;

    void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        dialogueManager = FindFirstObjectByType<VNDialogue>();

        originalPosition = eggCount.rectTransform.localPosition;

        Time.timeScale = 1f;
        waterSystem = GetComponent<WaterRefillSystem>();
        player = GameObject.FindWithTag("Player");
        playerHp = playerMaxHp;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        RefreshAllUI();
    }

    void Update()
    {
        if (playerHp > playerMaxHp) playerHp = playerMaxHp;
        
        float move = Input.GetAxisRaw("Horizontal");
        if (move != 0)
        {
            isLeft = (move == -1);
            isRight = (move == 1);
        }

        EquipEgg();
        ShootEgg();
    }

    public void UpdateSeedCount() { if (seedCount != null) seedCount.text = "Seed : " + seed; }

    public void UpdateEggCount() { if (eggCount != null) eggCount.text = "Egg : " + egg; }
    public void UseSeed() { if (seed > 0) { seed--; UpdateSeedCount(); } }  
    
    public void DecreaseTree()
    {
        if (tree > 0)
        {
            tree -= 1;
            UpdateTreeCount();

            if (tree <= 0)
            {
                // ถ้าเป็นด่าน 3 และยังไม่ได้เริ่มรอ ให้เริ่มรอ 10 วิกวัก!
                if (currentLevelIndex == 4 && !isWaitingForVictory)
                {
                    StartCoroutine(WaitBeforeWin(10f));
                }
                else if (currentLevelIndex != 3)
                {
                    WinGame();
                }
            }
        }
    }

    public void UpdateTreeCount() { if (treeCount != null) treeCount.text = "Remaining : " + tree; }
    public void RefreshAllUI() { UpdateSeedCount(); UpdateTreeCount(); UpdateEggCount(); }

    public void EquipEgg()
    { 
        if (dialogueManager.isDialogue) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isEgg)
            {
                Debug.Log("EggMode : On");
                isEgg = true;
                cursorHotSpot = new Vector2(cursorTexture.width / 2 , cursorTexture.height / 2);
                Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);
            }
            else
            {
                Debug.Log("EggMode : Off");
                isEgg = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
    }
    public void TriggerShakeEffect()
    {
        StartCoroutine(ShakeAndRed());
    }

    IEnumerator ShakeAndRed()
    {
        eggCount.color = Color.red;

        float duration = 0.2f;
        float elapsed = 0f;
        float intensity = 5f; 

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            eggCount.rectTransform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        eggCount.rectTransform.localPosition = originalPosition;
        eggCount.color = Color.white;
    }

    public void ShootEgg()
    {
        if (isEgg)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (egg == 0)
                {
                    TriggerShakeEffect();
                }
                else
                {   
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0; 

                    Vector3 direction = (mousePos - player.transform.position).normalized;

                    GameObject newBullet = Instantiate(eggBullet, player.transform.position, Quaternion.identity);
                    SpriteRenderer eggSR = newBullet.GetComponent<SpriteRenderer>();

                    eggSR.enabled = true;

                    Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = direction * 20f;
                        rb.angularVelocity = -500f;
                    }

                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                    if (hit.collider != null && hit.collider.CompareTag("LockedArea"))
                    {
                        Debug.Log("คลิกโดนพื้นที่กำหนดไว้แล้ว!");
                    }

                    egg--;
                    UpdateEggCount();
                }
            }
        }
    }

    // --- ฟังก์ชันพิเศษสำหรับด่านสุดท้ายกวัก ---
    IEnumerator WaitBeforeWin(float seconds)
    {
        isWaitingForVictory = true;
        Debug.Log("รดน้ำครบแล้ว! อีก " + seconds + " วินาทีจะจบเกมกวัก...");

        // (Option) ถ้านายมี SunSystem ในเป็ด นายอาจจะสั่งปิดเพื่อให้เป็ดอมตะช่วงนี้กวัก
        // GetComponent<SunSystem>().enabled = false; 

        yield return new WaitForSeconds(seconds);
        WinGame();
    }

    void WinGame()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        if (levelReached <= currentLevelIndex)
        {
            PlayerPrefs.SetInt("levelReached", currentLevelIndex + 1);
            PlayerPrefs.Save();
            Debug.Log("ปลดล็อกด่านถัดไปเรียบร้อยกวัก!");
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        playerHp -= damage;

        Debug.Log(playerHp);

        if (playerHp <= 0)
        {
            if (playerSound != null) playerSound.PlayActionSound("Die");
                deathPanel.SetActive(true);
                player.SetActive(false);
                enemy.SetActive(false);
        }
        Debug.Log("Player took " + damage + " damage!");
    }
}