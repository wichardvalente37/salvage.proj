using UnityEngine;

public class InfoManager : MonoBehaviour
{
    private GameObject[] Satelites;
    public bool[] hasLocator = new bool[8];
    public bool[] hasSensor = new bool[8];
    public int[] health = new int[8];
    int thing;
   public int snitch = 0;
    public bool canKill = false;
    public int scrap;
    void Start()
    {
        snitch++;
    }
    public void SetInfo()
    {
        Satelites = GameObject.FindGameObjectsWithTag("Satelite");
        GameObject thingo = GameObject.FindWithTag("InfoManager");
        if( thingo!= null && thingo.GetComponent<InfoManager>().snitch> 0 && canKill)
        {
            Debug.Log("ahsdijasod");
            Destroy(thingo);
        }
        foreach (GameObject satelite in Satelites)
        {

            satelite.GetComponent<SateliteHealth>().health = health[thing];
            satelite.GetComponent<Satelite>().hasEchoLocator = hasLocator[thing];
            satelite.GetComponent<Satelite>().hasSensor = hasSensor[thing];
            thing++;
        }
 thing = 0;
    }
    public void GetInfo() {
       
if(Satelites != null)
        {
 foreach (GameObject satelite in Satelites) {
if(satelite != null)
                {
 health[thing] =satelite.GetComponent<SateliteHealth>().health  ;
           hasLocator[thing]=  satelite.GetComponent<Satelite>().hasEchoLocator ;
            hasSensor[thing] = satelite.GetComponent<Satelite>().hasSensor;
                thing++;
                }
          
        }
            snitch=0;
            canKill = true;
            thing = 0;
        }
       
    
    }
}
