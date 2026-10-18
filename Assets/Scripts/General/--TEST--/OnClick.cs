using UnityEngine;

public class OnClick : MonoBehaviour
{
    public GameObject thing;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(thing, mousepos, Quaternion.identity);
        }
    }
}
