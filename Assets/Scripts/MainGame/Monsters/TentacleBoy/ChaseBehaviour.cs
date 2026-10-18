using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseBehaviour : MonoBehaviour
{
    private Transform player;
    public float ChaseSpeed;
    public float maxChaseSpeed;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
       maxChaseSpeed = ChaseSpeed;
    }

    // Update is called once per frame
    public void Chase()
    {
       
        if(player != null)
        {
transform.position = Vector2.MoveTowards(transform.position, player.position, ChaseSpeed * Time.deltaTime);
        }
            
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(collision.gameObject);
           
        }
    }

   
}
