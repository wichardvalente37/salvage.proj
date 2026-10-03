using UnityEngine;
using UnityEngine.InputSystem;

public class RepairSatelite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int repairAmount = 10;

    private SateliteHealth currentSatelite;
    public GameObject soundPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            
            if (currentSatelite != null)
            {

                currentSatelite.Repair(repairAmount);
                Instantiate(
soundPrefab,
currentSatelite.transform.position,
Quaternion.identity
);
            }

        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        SateliteHealth satelite = collision.GetComponent<SateliteHealth>();

        if (satelite != null)
        {

            currentSatelite = satelite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SateliteHealth satelite =
            collision.GetComponent<SateliteHealth>();

        if (satelite == currentSatelite)
        {
            currentSatelite = null;
        }
    }
}
