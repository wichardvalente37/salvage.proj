using System.Collections;
using UnityEngine;

public class MonsterBrain : MonoBehaviour
{
    private ChaseBehaviour chase;
    private PatrolBehaviour patrol;
    private InvestigateBehaviour investigate;
    public bool hasTouched;
    public float SpaceRadius;
    public float LosingDistance;
    private bool canChase;
    public LayerMask layer;
    private Transform player;
    public float DeaggroTime;
    private float lastchased;
    private float RealDeagroTime;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        chase = GetComponent<ChaseBehaviour>();
        patrol = GetComponent<PatrolBehaviour>();
        investigate = GetComponent<InvestigateBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        HasSight();

        if (canChase)
        {
            chase.Chase();
 LosingSight();
        }

         if(!canChase && !investigate.isInvestigating)
        {
            patrol.canSeek();
           
        }

        if (hasTouched)
        {
            canChase = true;
            investigate.isInvestigating = false;
            investigate.SusAmount = 0;
        }


    }

    void LosingSight()
    {

        if(Vector2.Distance(transform.position, player.position)>LosingDistance)
        {
          if(Time.time < lastchased + DeaggroTime)
            {
                return;
            }
            canChase = false;
        }
        else
        {
            lastchased = Time.time;
            RealDeagroTime = Random.Range(DeaggroTime - 1.2f, DeaggroTime + 2.2f);
        }
    }
    void HasSight()
    {
        hasTouched = Physics2D.OverlapCircle(transform.position, SpaceRadius, layer);
    }
}
