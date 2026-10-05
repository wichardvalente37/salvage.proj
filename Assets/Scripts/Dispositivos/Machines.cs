using UnityEngine;
using UnityEngine.InputSystem;

public class Machines : MonoBehaviour
{
    public GameObject Sonar;
    public float cooldown;
    public int Echolocators;
    public int EchoSensors;
    public int Soundbait;
    private GameObject mapCameraObject;
    private Camera mapCamera;
    private float nextSonar;
    private GameObject player;
 GameObject OBJsonar;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        // OpenCam may already have disabled this object in its Start.
        foreach (var candidate in FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (!candidate.CompareTag("CamCam")) continue;
            mapCameraObject = candidate.gameObject;
            mapCamera = candidate.GetComponentInChildren<Camera>(true);
            break;
        }
    }
   
    public bool FireSonar(Satelite satellite)
    {
        if (satellite == null || !satellite.hasEchoLocator || Sonar == null || Time.time < nextSonar ) return false;
        OBJsonar = Instantiate(Sonar, satellite.transform.position, Quaternion.identity);
        nextSonar = Time.time + Mathf.Max(0, cooldown);
        return true;
    }

    public bool ActivateBait(Satelite satellite)
    {
        if (satellite == null) return false;
        var bait = satellite.GetComponent<SoundBaitDevice>();
        return bait != null && bait.Activate();
    }

    private void Update()
    {
        if(Sonar != null && !mapCameraObject.activeSelf)
        {
            Destroy(OBJsonar);
        }
       
        var mouse = Mouse.current;
        if (mouse == null || mapCamera == null || mapCameraObject == null || !mapCameraObject.activeInHierarchy) return;
        bool sonar = mouse.leftButton.wasPressedThisFrame;
        bool bait = mouse.rightButton.wasPressedThisFrame;
        if (!sonar && !bait) return;
        Vector2 screen = mouse.position.ReadValue();
        // The inventory's buttons must not also activate a satellite underneath them.
       
        Vector2 world = mapCamera.ScreenToWorldPoint(screen);
        Satelite nearest = null;
        float distance = float.PositiveInfinity;
        foreach (var hit in Physics2D.OverlapPointAll(world))
        {
            var satellite = hit.GetComponentInParent<Satelite>();
            if (satellite == null || !satellite.hasEchoLocator) continue;
            float candidate = Vector2.Distance(world, satellite.transform.position);
            if (candidate < distance) { nearest = satellite; distance = candidate; }
        }
        if (nearest == null) return;
        if (sonar) FireSonar(nearest);
        if (bait) ActivateBait(nearest);
    }



    public void CraftLocator()
    {
        if(!player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator && Echolocators > 0 || EchoSensors > 0)
        {
            player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator = true;
        }
    }

    public void CraftSensorLocator()
    {
        if (player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator == false&& Echolocators > 0 || EchoSensors > 0)
        {
            player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasEchoLocator = true;
        }
        if (!player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasSensor &&EchoSensors > 0)
        {
            player.GetComponent<RepairSatelite>().currentSatelite.GetComponent<Satelite>().hasSensor = true;
        }
    }
}
