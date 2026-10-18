using Unity.VisualScripting;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public int Events = 0;
    bool canAct = true;
    private int[] lastpoint = new int[20];
    Transform player;
    GameObject[] Satelites;
   [Header("Variaveis do Evento LightESpawn")]
    public GameObject RedLight;
    [Header("Variaveis do Evento SpawnMonster")]
    public GameObject Spawner;
        bool canSpawn = true;
    [Header("Variaveis do Evento SpawnMines")]
    public GameObject Mine;
    public Transform[] MinePoints;
    public int NumOfMines;
    bool Spawn = false;
    [Header("Variaveis do Evento SpawnEye")]
    public GameObject Eye;
    bool canSpawnEye;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Satelites = GameObject.FindGameObjectsWithTag("Satelite");
        GameObject.FindGameObjectWithTag("InfoManager").GetComponent<InfoManager>().SetSateliteInfo();
        Events = GameObject.FindGameObjectWithTag("InfoManager").GetComponent<InfoManager>().Stuff;
        if(Events == 3)
        {
            canSpawn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
      
        if (canAct)
        {
            Light();
            SpawnEye();
        } 
        SpawnMonster();
        SpawnMines();
    }



    GameObject[] Redlights = new GameObject[20];
    int thing = 0;

    /// <summary>
    /// Primeiro Evento
    /// Aparecem luzes nos Satélites, eas luzes tocam música, e spawnam o Monstro pela primeira vez
    /// </summary>
    void Light()
    {
        if(Events == 1)
        {
  

        foreach(GameObject satlite in Satelites)
        {
                Redlights[thing]=Instantiate(RedLight, satlite.transform.position, Quaternion.identity);
                thing++;
        }
        canAct = false;
          
    }
        }

    public void DestoryLights()
    {
        foreach(GameObject light in Redlights)
        {
            Destroy(light);
        }
    }
    /////////Primeiro Evento\\\\\\\\\\\\\\\\\\\\
    
    /// <summary>
    /// Depois do Primeiro Evento, é necessário spawnar sempre o monstro
    /// Ñ mexe no bool canAct, porque não é o evento principal
    /// </summary>
    void SpawnMonster()
    {
        if (canSpawn && Events > 1)
        {
Instantiate(Spawner, transform.position, Quaternion.identity);
            Spawn = true;
            canSpawn = false;
        }
        
    }
    ////////////Término de SpawnMonster\\\\\\\\\\\\



    /// <summary>
    /// Evento de spawnar Minas,depois do primeiro Evento sempre spawna
    /// </summary>
   
    void SpawnMines()
    {
        if(Events >= 2 && Spawn)
        {
            thing = 0;
 for(int i = 0; i < NumOfMines; i++)
        {
            int rand = 0;

            do
            {

                rand = Random.Range(0, MinePoints.Length);

            }
            while (System.Array.Exists(lastpoint, num => num == rand));
            lastpoint[thing] = rand;
                thing++;
            Instantiate(Mine, MinePoints[rand].position, Quaternion.identity);
        }
            Spawn = false;
            System.Array.Clear(lastpoint, 0, lastpoint.Length);
        }
       
    }

    ////////////Término de SpawnMines\\\\\\\\\\\\


    /// <summary>
    /// Evento de Spawnar o OLHO
    /// 
    /// </summary>
    GameObject eye;
    int Thing = 0;
    bool can = false;
    void SpawnEye()
    {
        if(Events == 3)
        {
            if(eye == null&&!canSpawn && thing == 0)
            {
  foreach(GameObject salite in Satelites)
            {
               if (salite.GetComponent<SateliteHealth>().health >= 100)
                {
                    canSpawnEye = true;
                        thing++;
                }
            }
            }
          

        if (canSpawnEye)
        {
            eye = Instantiate(Eye, new Vector2(player.position.x,player.position.y+27), Quaternion.identity);
                can = true;
            canSpawnEye = false;
        }

            if(eye == null && can)
            {
                canSpawn = true;
                canAct = false;
            }
        }
       
      
    }
}
