using UnityEngine;

public class InvestigateBehaviour : MonoBehaviour
{
    public float InvestigateSpeed;
    public float SusAmount;
    public bool isInvestigating;
    private GameObject SusPosition;
    private float ticks;
    Vector2 SusPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SusAmount >= 10 && !isInvestigating)
        {
SusPosition = GameObject.FindWithTag("SusPosition");
   SusPos = SusPosition.transform.position;
            isInvestigating = true;
        }
        if (isInvestigating)
        {
            Investigate();
        }
        else
        {
            ticks = 0;
        }
    }
    private float lastinvestigated;
    void Investigate()
    {
        if(ticks > 4)
        {
SusAmount = 0;
            isInvestigating = false;
            
        }
     

        if (Vector2.Distance(transform.position, SusPos) < 0.1f)
        {
            if(Time.time < Random.Range(3f,8f) + lastinvestigated)
            {
                return;
            }
            SusPos.y += Random.Range(Random.Range(-3f,5f), Random.Range(-4f, 3f));
            SusPos.x+= Random.Range(Random.Range(-5f, 2f), Random.Range(-2f, 5f));
            ticks++;
        }
        else
        {

          transform.position = Vector2.MoveTowards(transform.position, SusPos, InvestigateSpeed * Time.deltaTime);
            lastinvestigated = Time.time;
        }
    }
}
