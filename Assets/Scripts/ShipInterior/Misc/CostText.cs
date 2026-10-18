using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CostText : MonoBehaviour
{
    private TMP_Text text;
    private string thing;
   public  bool Elocator;
    public bool ESensor;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        if (Elocator)
        {
text.text = "Cost: " +GameObject.FindWithTag("ShopManager").GetComponent<Shop>().ELocatorCost+"x Scraps";
        }
        if (ESensor)
        {
            text.text = "Cost: " + GameObject.FindWithTag("ShopManager").GetComponent<Shop>().ESensorCost + "x Scraps";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
