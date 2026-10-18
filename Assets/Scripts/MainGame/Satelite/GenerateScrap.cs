using System.Collections;
using UnityEngine;

public class GenerateScrap : MonoBehaviour
{
    private SateliteHealth health;
    public int ScrapAmount;
    public GameObject scrapObj;
    bool hasSpawned; 
  
    void Start()
    {
        health = GetComponent<SateliteHealth>();

        if(health.health > health.maxHealth - 20)
        {
            ScrapAmount = Random.Range(3, 5);
        }
        else
        {
            ScrapAmount = Random.Range(0, 2);
        }
    }
    GameObject scrap;
   
   
  public void Spawn()
    {
       
        if (!hasSpawned&&health.health >=health.maxHealth)
        {
  scrap = Instantiate(scrapObj, transform.position, Quaternion.identity);
        scrap.GetComponent<ScrapPickup>().amount = ScrapAmount;
            hasSpawned = true;
        }
       
      
    }

   
}
