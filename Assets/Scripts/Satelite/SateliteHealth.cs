using UnityEngine;

public class SateliteHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public void Repair(int amount)
    {
        health += amount;
        if(health>maxHealth)
        {
            health = maxHealth;
        }
        var satellite = GetComponent<Satelite>();
        if (satellite != null && RunState.Instance != null) RunState.Instance.Save(satellite);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
