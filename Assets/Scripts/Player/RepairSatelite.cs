using UnityEngine;
using UnityEngine.InputSystem;

public class RepairSatelite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int repairAmount = 10;

    private SateliteHealth currentSatelite;
    public GameObject soundPrefab;
    public GameObject Ebutton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            
            if (currentSatelite != null)
            {

                if (currentSatelite.health >= currentSatelite.maxHealth) return;
                currentSatelite.GetComponent<GenerateScrap>().Spawn();
                currentSatelite.Repair(Random.Range(repairAmount-7,repairAmount));
                var animation = GetComponent<SpriteFrameAnimation>();
                if (animation != null) animation.PlayRepair();
                Instantiate(
soundPrefab,
currentSatelite.transform.position,
Quaternion.identity
);
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
