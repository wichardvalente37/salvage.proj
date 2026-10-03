using UnityEngine;

public class Machines : MonoBehaviour
{
    private GameObject[] satelites;
    public GameObject Sonar;
    private GameObject CamCam;
    public float cooldown;
    private float lastClicked;
    public int Echolocators;
    public int EchoSensors;
    public int Soundbait;
    void Start()
    {
CamCam = GameObject.FindWithTag("CamCam");
        satelites = GameObject.FindGameObjectsWithTag("Satelite");
        lastClicked = -cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        canEcholocate();
    }
    
    void canEcholocate()
    {
        if(Time.time < lastClicked + cooldown)
        {
            return;
        }

  Echolocator();

    }


    void Echolocator()
    {
        if(CamCam != null)
        {
 if(CamCam.activeSelf)
        {
   foreach(GameObject satelite in satelites)
        {
            if (satelite.GetComponent<Satelite>().hasclicked&& satelite.GetComponent<Satelite>().hasEchoLocator)
            {
                Instantiate(Sonar,satelite.transform.position, Quaternion.identity);
                    lastClicked = Time.time;
            }
        }
        }
        }
       
     
    }

  
}
