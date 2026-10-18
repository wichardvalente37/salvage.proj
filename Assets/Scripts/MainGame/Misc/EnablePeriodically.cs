using System.Collections;
using UnityEngine;

public class EnablePeriodically : MonoBehaviour
{
    public GameObject Obj;
    public float cooldown;
    public float TimeEnabled;
    private float lastEnabled;
    void Start()
    {
        Obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < cooldown + lastEnabled)
        {
          
            return;
        }


        StartCoroutine(enable());

        lastEnabled = Time.time;
    }


    IEnumerator enable()
    {
        Obj.SetActive(true);
        yield return new WaitForSeconds(TimeEnabled);
   cooldown = Random.Range(cooldown - 7f, cooldown + 9f);
        Obj.SetActive(false);
    }
}
