using UnityEngine;

public class ParentUp : MonoBehaviour
{
    private GameObject parent;
    public string ParName;
    void Start()
    {
        parent = GameObject.Find(ParName);
        transform.parent = parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
