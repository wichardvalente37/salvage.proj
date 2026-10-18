using UnityEngine;

public class SateliteManager : MonoBehaviour
<<<<<<< Updated upstream:Assets/Scripts/GameManager/SateliteManager.cs
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
=======
{ int[] lastpoint = new int[20];
    private GameObject[] Satelites;
    private GameObject[] MonPoints;
    private void Start()
    { 
        Satelites = GameObject.FindGameObjectsWithTag("Satelite");
        InfoManager manager = GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>();
        
      
        
        foreach (var satellite in Satelites)
        {
            var health = satellite.GetComponent<SateliteHealth>();
           
         
                health.health = Random.Range(health.health - 85, health.health - 12);
            health.StartUp();
            
>>>>>>> Stashed changes:Assets/Scripts/MainGame/Managers/GameManager/SateliteManager.cs
        }
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream:Assets/Scripts/GameManager/SateliteManager.cs
        
=======
       
        MonPoints = GameObject.FindGameObjectsWithTag("SatelitePoint");
       
        foreach (GameObject satelite in Satelites)
        {
            int rand = 0;

            do
            {

                rand = Random.Range(0, MonPoints.Length);

            }
            while (System.Array.Exists(lastpoint, num => num == rand));
            lastpoint[thing] = rand;
            satelite.transform.position = MonPoints[rand].transform.position;
            thing++;


        }

>>>>>>> Stashed changes:Assets/Scripts/MainGame/Managers/GameManager/SateliteManager.cs
    }
}
