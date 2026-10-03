using UnityEngine;
using UnityEngine.InputSystem;

public class RepairSatelite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int repairAmount = 10;

    private SateliteHealth currentSatelite;
    public GameObject soundPrefab;
    public GameObject Ebutton;
   
    public float repairSusAmount = 2f;
    private NoiseEmitter noiseEmitter;
    
    void Awake()
    {
        noiseEmitter = GetComponent<NoiseEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            
            if (currentSatelite != null)
            {

                currentSatelite.Repair(repairAmount);
                noiseEmitter.MakeNoise(currentSatelite.transform.position, repairSusAmount);
            }

        }


    }
    private GameObject ebutt;
    bool hasSpawned = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        SateliteHealth satelite = collision.GetComponent<SateliteHealth>();

        if (satelite != null)
        {

            currentSatelite = satelite;
            if (!hasSpawned)
            {
  ebutt = Instantiate(Ebutton, currentSatelite.transform.position+Vector3.up, Quaternion.identity);
                hasSpawned = true;
            }
          
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SateliteHealth satelite =
            collision.GetComponent<SateliteHealth>();

        if (satelite == currentSatelite)
        {
            if (hasSpawned)
            {
                Destroy(ebutt );
                hasSpawned = false;
            }
            currentSatelite = null;
        }
    }
}
