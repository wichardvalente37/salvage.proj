using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shipEntrance : MonoBehaviour
{
    public string scene;
    bool cantrack = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(Transition());
        }
    }
    private void Update()
    {
        if (cantrack)
        {
GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>().GetInfo();
        }
    }
    IEnumerator Transition()
    {
        cantrack = true;
        yield return new WaitForSeconds(2);
            SceneManager.LoadScene(scene);
    }
}
