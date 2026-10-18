using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    private Transform player;
    public float time;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("ShipInterior");
    }
}
