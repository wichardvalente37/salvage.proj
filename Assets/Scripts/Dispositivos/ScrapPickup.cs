using Unity.VisualScripting;
using UnityEngine;


public class ScrapPickup : MonoBehaviour
{
    [Min(1)] public int amount = 1;
    private bool collected;

   
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || amount <= 0) return;

        GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>().scrap += amount;
        collected = true;
        
        Destroy(gameObject);
    }
  
    
  
}
