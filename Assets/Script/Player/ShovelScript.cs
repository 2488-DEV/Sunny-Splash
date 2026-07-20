using UnityEngine;
using TMPro;

public class ShovelScript : MonoBehaviour
{
    public bool IsInRange;
    private PlayerScript player;
    private SeedScript seed;
    private FoodScript food;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerScript>();
        }
        seed = FindFirstObjectByType<SeedScript>();
        food = FindFirstObjectByType<FoodScript>();
    }

    void OnEnable() 
    { 
        GameInput.OnPickUp += TryPickUpShovel; 
        GameInput.OnDrop += TryDropShovel; // ฟัง Event Drop
    }   

    void OnDisable() 
    { 
        GameInput.OnPickUp -= TryPickUpShovel; 
        GameInput.OnDrop -= TryDropShovel; 
    }   

    // ฟังก์ชันใหม่สำหรับการวางพลั่ว
    void TryDropShovel()
    {
        // เช็กว่าถือพลั่วอยู่ไหม ถ้าไม่ถือก็ไม่ต้อง Drop
        if (player != null && player.IsShovel)
        {
            transform.rotation = Quaternion.Euler(0, 0, 130f);
            player.IsShovel = false;
            IsInRange = true;

            // อัปเดต Visuals
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            Transform hl = transform.Find("Highlight");
            if (hl != null) hl.GetComponent<Renderer>().enabled = true;
        }
    }

    void Update()
    { 
        // ระบบติดตามตัวละครยังคงไว้ที่ Update เหมือนเดิมครับ
        if (player != null && player.IsShovel)
        { 
            if (player.isLeft)
            {
                transform.position = new Vector3(player.transform.position.x - 0.8f , player.transform.position.y - 0.475f , player.transform.position.z);
                transform.rotation = Quaternion.Euler(0, 0, -19f);
            }
            else if (player.isRight)
            {
                transform.position = new Vector3(player.transform.position.x + 0.8f , player.transform.position.y - 0.475f , player.transform.position.z);
                transform.rotation = Quaternion.Euler(0, 0, 100f);
            }
        }
    }

    // ฟังก์ชันนี้จะถูกเรียกเมื่อกดปุ่ม F หรือปุ่ม PickUp บน UI
    void TryPickUpShovel()
    {
        if (!IsInRange) return;
        
        // เช็ก Action อื่น
        if (PlayerActionManager.Instance != null && PlayerActionManager.Instance.IsPerformingAction) return;

        if (player != null && !player.IsShovel)
        {
            PlayerActionManager.Instance.TryStartAction(ActionType.PickUpItem, () =>
            {
                player.IsShovel = true;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                Transform hl = transform.Find("Highlight");
                if (hl != null) hl.GetComponent<Renderer>().enabled = false;
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = true;
            Debug.Log("Enter");
            transform.Find("Highlight").GetComponent<Renderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = false;
            Debug.Log("Exit");
            transform.Find("Highlight").GetComponent<Renderer>().enabled = false;
        }
    }
}
