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
        else if(health.health > health.maxHealth - 40 && health.health < health.maxHealth - 20)
        {
            ScrapAmount = Random.Range(0, 2);
        }
    }
    GameObject scrap;
   
   
  public void Spawn()
    {
        if (health.health < health.maxHealth - 10)
            return;
        if (!hasSpawned)
        {
  scrap = Instantiate(scrapObj, transform.position, Quaternion.identity);
        scrap.GetComponent<ScrapPickup>().amount = ScrapAmount;
            hasSpawned = true;
        }
       
      
    }

   
}
