using Unity.VisualScripting;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public float SusAmount;
    public GameObject suspos;
    private Transform Monster;
    void Start()
    {
 Monster = GameObject.FindWithTag("Monster").transform;
        float Value = Monster.GetComponent<InvestigateBehaviour>().SusAmount;
        Value += SusAmount;
        if(Value>= 10)
        {
            Instantiate(suspos, transform.position, Quaternion.identity);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Monster.position, SusAmount+2 * Time.deltaTime);
        if (Vector2.Distance(transform.position, Monster.position) < 0.3f)
        {
Monster.GetComponent<InvestigateBehaviour>().SusAmount += SusAmount;
            Destroy(gameObject);
        }
      
    }

 
}
