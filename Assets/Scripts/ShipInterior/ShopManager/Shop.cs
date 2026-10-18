using UnityEngine;

public class Shop : MonoBehaviour
{
    
    public int ESensorCost;
    public int ELocatorCost;
    public int Money;

    public int ESensorPurchases;
    public int ELocatorPurchases;
    private void Start()
    {
        GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>().SetInfo(null, GetComponent<Shop>());
    }
    public void BuyLocator()
    {
        if (Money >= ELocatorCost)
        {
   ELocatorPurchases++;
        Money-=ELocatorCost;
        }
         
    }

    public void BuySensor()
    {
        if (Money >= ESensorCost){
            ESensorPurchases++;
            Money -= ESensorCost;
        }
    }
}
