using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public bool IsShovel;

    public int water_gauge = 100;
    public Slider waterBar;
    public int seed;
    public TextMeshProUGUI seedCount;
    public TextMeshProUGUI treeCount;
    public int tree;
    public float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waterBar.value = water_gauge;
        seedCount.text = "Seed : " + seed.ToString();
        treeCount.text = "Tree : " + tree.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateWater()
    {
        waterBar.value = water_gauge;
    }
    public void UpdateSeedCount()
    {
        seedCount.text = "Seed : " + seed.ToString();
    }
    public void UpdateTreeCount()
    {
        treeCount.text = "Tree : " + tree.ToString();
    }
}
