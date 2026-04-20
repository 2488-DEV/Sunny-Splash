using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    // ตัวแปรนี้เอาไว้บอก Unity ว่าหน้าต่าง Setting คืออันไหน
    public GameObject settingPanel;

    // สั่งให้หน้าต่าง "แสดงผล"
    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }

    // สั่งให้หน้าต่าง "หายไป"
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }
}