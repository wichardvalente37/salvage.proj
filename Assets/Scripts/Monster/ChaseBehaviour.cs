using UnityEngine;

public class ChaseBehaviour : MonoBehaviour
{
    private Transform player;
    public float ChaseSpeed;
   
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
       
    }

    // Update is called once per frame
    public void Chase()
    {
       

            transform.position = Vector2.MoveTowards(transform.position, player.position, ChaseSpeed * Time.deltaTime);
      
    }
}
