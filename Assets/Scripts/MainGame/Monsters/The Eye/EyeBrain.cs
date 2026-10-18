using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EyeBrain : MonoBehaviour
{
    private Transform player;
    public float SnapDistance;
    private ChaseBehaviour chase;
    Light2D lighting = new Light2D();
    Light2D[] Lights;
    bool can = false;
    void Start()
    {
        Lights = GameObject.FindObjectsByType<Light2D>();
       foreach(Light2D i in Lights)
        {
            if (i.gameObject.GetComponent<RestoreLight>() == null)
                i.gameObject.AddComponent<RestoreLight>();
        }
  
        player = GameObject.FindWithTag("Player").transform;
        chase = GetComponent<ChaseBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        chase.Chase();
        if (Vector2.Distance(transform.position, player.position) <= SnapDistance+7)
        {
            if (can)
            {
               player.GetComponent<PlayerMovement>().dashForce -= 2 * Time.deltaTime;
            }
           
        }
        if (Vector2.Distance(transform.position,player.position)< SnapDistance)
        {
           flickerLights();
            can = true;
            chase.ChaseSpeed = chase.maxChaseSpeed;
        }
        else if(Vector2.Distance(transform.position, player.position) > SnapDistance)
        {

            chase.ChaseSpeed = 5f;
        }
        if (Vector2.Distance(transform.position, player.position) < 0.4f)
        {
            RestoreLights();
            player.GetComponent<PlayerMovement>().dashForce = player.GetComponent<PlayerMovement>().maxdash;
            Destroy(gameObject);
        }
    }

    void flickerLights()
    {
         
        foreach(var lights in Lights)
        {
           
StartCoroutine(changeLight(lights));
           
        }
        
            
       
    }
 
    IEnumerator changeLight(Light2D light)
    {

        lighting.intensity = light.intensity;
        lighting.intensity -= 3f;
        light.intensity = lighting.intensity;
        light.color = Color.red;
       
        yield return new WaitForSeconds(0.1f);


        light.GetComponent<RestoreLight>().restore(false);

       
        
    }

    void RestoreLights()
    {
       
        foreach(var lights in Lights)
        {
            lights.GetComponent<RestoreLight>().restore(true);
        }
    }
}
