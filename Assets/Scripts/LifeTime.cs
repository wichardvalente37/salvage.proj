using UnityEngine;

public class LifeTime : MonoBehaviour
{
    public float lifetime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
    }
}
