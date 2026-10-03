using UnityEngine;

public class SateliteManager : MonoBehaviour
{
    private GameObject[] Satelite;
    void Start()
    {
        Satelite = GameObject.FindGameObjectsWithTag("Satelite");
        foreach(GameObject satelite in Satelite)
        {
            int health = satelite.GetComponent<SateliteHealth>().health;
            health = Random.Range(health - 29,health-10);
            satelite.GetComponent<SateliteHealth>().health = health;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
