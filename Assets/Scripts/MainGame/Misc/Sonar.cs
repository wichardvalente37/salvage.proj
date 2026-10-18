using UnityEngine;

public class Sonar : MonoBehaviour
{
    public GameObject Monsterpoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            Instantiate(Monsterpoint, collision.transform.position, Quaternion.identity);
        }
    }
}
