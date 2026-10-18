using UnityEngine;
using UnityEngine.InputSystem;

public class Machines : MonoBehaviour
{
    public GameObject Sonar;
    public float cooldown;
    public int Echolocators;
    public int EchoSensors;
    public int Soundbait;
    public GameObject mapCameraObject;
    private Camera mapCamera;
    private float nextSonar;
    private GameObject player;
 GameObject OBJsonar;
    public float NoiseEmission;
    private GameObject[] Satelites;
    private GameObject[] Cammeds;
    private void Start()
    {

       Satelites = GameObject.FindGameObjectsWithTag("Satelite");
       
        player = GameObject.FindWithTag("Player");
           InfoManager manager = GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>();
        manager.SetInfo(GetComponent<Machines>(),null);
        // OpenCam may already have disabled this object in its Start.
       
    }
   
    public void FireSonar(Satelite satellite)
    {
        if (satellite == null || !satellite.hasEchoLocator || Sonar == null || Time.time < nextSonar ) return;
        OBJsonar = Instantiate(Sonar, satellite.transform.position, Quaternion.identity);
        if (GameAudio.Instance != null) GameAudio.Instance.PlaySonar();
        nextSonar = Time.time + cooldown;
        satellite.hasclicked = false;
    }

  

    private void Update()
    {
        if(Sonar != null && !mapCameraObject.activeSelf)
        {
            Destroy(OBJsonar);
        }
       
 
        if (mapCameraObject == null || !mapCameraObject.activeInHierarchy) return;
 Cammeds = GameObject.FindGameObjectsWithTag("CamSatelite");

        if(Cammeds!= null)
        {
 foreach(GameObject cam in Cammeds)
        {
            if(cam.GetComponent<CammedSat>().hasclicked)
            {
                foreach(GameObject sat in Satelites)
                {
                    if (Vector2.Distance(sat.transform.position, cam.transform.position)<0.3f)
                    {
   FireSonar(sat.GetComponent<Satelite>());
                        cam.GetComponent<CammedSat>().hasclicked = false;
                    }
                }
             
            }
        }
        }
      
     

    }



    public void CraftLocator()
    {
        if(!player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator && Echolocators > 0)
        {
            player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator = true;
            Echolocators--;
            GetComponent<NoiseEmitter>().EmitSound(NoiseEmission);
        }
    }

    public void CraftSensorLocator()
    {
        if (player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator == false &&EchoSensors > 0)
        {
            player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator = true;
        }
        if (!player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasSensor &&EchoSensors > 0)
        {
            player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasSensor = true;
            EchoSensors--;
            GetComponent<NoiseEmitter>().EmitSound(NoiseEmission);
        }
    }
}
