using TMPro;
using UnityEngine;

public class GetName : MonoBehaviour
{
    public TMP_Text text;
    private GameObject[] Satelites;
    void Start()
    {
        Satelites = GameObject.FindGameObjectsWithTag("Satelite");
       
        foreach(GameObject satelite in Satelites)
        {
            if(Vector2.Distance(satelite.transform.position, transform.position) < 0.1f)
            {
                  text.text= satelite.GetComponent<Satelite>().Name;
            }
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
