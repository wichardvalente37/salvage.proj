using System.Collections;
using UnityEngine;

public class Push : MonoBehaviour
{  public float Force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         Force /= 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(Force > 0)
        {
 StartCoroutine(wait());
           
             
        }
       
         
}
  
    private IEnumerator wait()
    {


        Debug.Log("shit");
        Force += 10 * Time.deltaTime;
        transform.position += Vector3.left*Force * Time.deltaTime;
        yield return new WaitForSeconds(0.8f);
       
        
        Force = 0;
    }
}
