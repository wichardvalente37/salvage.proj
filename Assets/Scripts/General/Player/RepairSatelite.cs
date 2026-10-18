using UnityEngine;
using UnityEngine.InputSystem;

public class RepairSatelite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int repairAmount = 10;
<<<<<<< Updated upstream:Assets/Scripts/Player/RepairSatelite.cs

    private SateliteHealth currentSatelite;
    public GameObject soundPrefab;
    public GameObject Ebutton;
=======
    public float NoiseEmission;
    public SateliteHealth currentSatelite;
    public GameObject soundPrefab;
    public GameObject Ebutton;
    public GameObject canvas;
    public float cooldown;
    private float lastRepaired;
>>>>>>> Stashed changes:Assets/Scripts/General/Player/RepairSatelite.cs
    void Start()
    {
        lastRepaired = -cooldown;
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream:Assets/Scripts/Player/RepairSatelite.cs
        if (Keyboard.current.eKey.wasPressedThisFrame)
=======
        if (Time.time < lastRepaired + cooldown)
            return;
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
>>>>>>> Stashed changes:Assets/Scripts/General/Player/RepairSatelite.cs
        {
            
            if (currentSatelite != null)
            {
<<<<<<< Updated upstream:Assets/Scripts/Player/RepairSatelite.cs

                currentSatelite.Repair(repairAmount);
=======
                canvas.SetActive(true);
currentSatelite.GetComponent<GenerateScrap>().Spawn();
                if (currentSatelite.health >= currentSatelite.maxHealth) return;
                
                currentSatelite.Repair(Random.Range(repairAmount-7,repairAmount));
                if (GameAudio.Instance != null) GameAudio.Instance.PlayRepair();
                var animation = GetComponent<SpriteFrameAnimation>();
                if (animation != null) animation.PlayRepair();
                GetComponent<NoiseEmitter>().EmitSound(NoiseEmission);
>>>>>>> Stashed changes:Assets/Scripts/General/Player/RepairSatelite.cs
                Instantiate(
soundPrefab,
currentSatelite.transform.position,
Quaternion.identity
);
                lastRepaired = Time.time;
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
        if (satelite != null)
       

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
