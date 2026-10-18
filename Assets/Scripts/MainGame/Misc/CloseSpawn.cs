using UnityEngine;

public class CloseSpawn : MonoBehaviour
{
    public float Dist;
    private Transform player;
    public GameObject obj;
    public GameObject Music;
   
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    GameObject music;
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) <= Dist)
        {
 Instantiate(obj, transform.position, Quaternion.identity);
            if (GameObject.FindWithTag("EerieMusic") != null)
            {

                Destroy(GameObject.FindWithTag("EerieMusic"));
            }
            GameObject.FindAnyObjectByType<EventsManager>().DestoryLights();
        }
        if (Vector2.Distance(transform.position, player.position) > Dist + 15)
        {
            if (GameObject.FindWithTag("EerieMusic") == null)
            {
               
                return;
            }

            Destroy(music);

        }
        if (Vector2.Distance(transform.position, player.position) <= Dist+15 && Vector2.Distance(transform.position, player.position)> Dist+8 )
        {
            if (GameObject.FindWithTag("EerieMusic") != null)
            {
                music.GetComponent<AudioSource>().volume = .5f;
                return;
            }
              
            music = Instantiate(Music, transform.position, Quaternion.identity);
           
        }
        if (Vector2.Distance(transform.position, player.position) <= Dist + 8)
        {
            music.GetComponent<AudioSource>().volume = 1f;

        }

    }
}
