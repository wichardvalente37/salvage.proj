using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoManager : MonoBehaviour
{


    int Locators;
    int Sensors;
    bool canUp = false;
    public int Stuff;
    public int scrap;
    bool[] hasSensor = new   bool[20];
    bool[] hasLocator=new   bool[20];
    int thing = 0;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("ShopManager") != null && !canUp)
        {
           
            canUp = true;
        }
     
    }
    public void SetInfo(Machines gameman,Shop shop)
    {
        if(shop != null)
        {
 shop.Money = scrap;
        scrap -= shop.Money;
        }
       
        if(gameman != null)
        {
  gameman.Echolocators = Locators;
        gameman.EchoSensors = Sensors;
        Locators -= gameman.Echolocators;
        Sensors -= gameman.EchoSensors;
        }
      
   
   
    }
    public void GetInfo(Shop shop) {

        Locators = shop.ELocatorPurchases;
       Sensors = shop.ESensorPurchases;

          
        
       
    
    }


    public void GetSateliteInfo()
    {
        GameObject[] Satelites = GameObject.FindGameObjectsWithTag("Satelite").OrderBy(go=> go.transform.GetSiblingIndex()).ToArray();
        if (Satelites != null)
        {
            foreach(GameObject satlite in Satelites)
            {
                hasLocator[thing]= satlite.GetComponent<Satelite>().hasEchoLocator;
                hasSensor[thing] = satlite.GetComponent<Satelite>().hasSensor;
                thing++;
            }
            thing = 0;
        }
    }
    public void SetSateliteInfo()
    {
        GameObject[] Satelites = GameObject.FindGameObjectsWithTag("Satelite").OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
        if (Satelites != null)
        {
            foreach (GameObject satlite in Satelites)
            {
                 satlite.GetComponent<Satelite>().hasEchoLocator= hasLocator[thing];
               satlite.GetComponent<Satelite>().hasSensor  = hasSensor[thing];
                thing++;
            }
            thing = 0;
        }
    }

    public void UpStuff() {
        if (canUp)
        {
         Stuff++;
            canUp = false;
        }
       
    }
}
