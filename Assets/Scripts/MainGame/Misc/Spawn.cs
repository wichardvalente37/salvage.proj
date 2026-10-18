using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject obj;
    public Transform PlaceToSpawn;
    public bool can;
    public float time;
    float lastStuffed;
    void Start()
    {
        lastStuffed = Time.time;
        if (!can)
        {
 if (GameObject.FindWithTag(obj.tag) != null)
        {
         return;
        }

        if(obj.tag != "Monster")
        {
Instantiate(obj, PlaceToSpawn.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
        if(obj.tag == "Monster")
        {
            StartCoroutine(SpawnMonster());
        }
        }
       
    }

    private void Update()
    {
        if (can)
        {
      
        if (Time.time > time + lastStuffed)
            {
 Instantiate(obj, PlaceToSpawn.position, Quaternion.identity);
            Destroy(gameObject);
            }
           
        }
    }

    IEnumerator SpawnMonster()
    {
        PlaceToSpawn = GameObject.Find("MonsterPoint").transform;
        yield return new WaitForSeconds(Random.Range(8, 14));
        Instantiate(obj, PlaceToSpawn.position, Quaternion.identity);
        GameAudio audio = GameObject.FindAnyObjectByType<GameAudio>();
        audio.Acess();
        Destroy(gameObject);

    }
}
