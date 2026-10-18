using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class shipEntrance : MonoBehaviour
{
    public string scene;
    private GameObject[] Satelites;
    bool canGo = false;
    public int thing = 0;
    public int NumOfSatelites = 0;
    public GameObject text;
    public GameObject MainCanvas;

    void Start()
    {
        if(gameObject.scene == SceneManager.GetSceneByName("ShipInterior"))
        {
            canGo = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CanEnter();
        }
        if (collision.tag == "Player" && canGo)
        {
            if (GameObject.FindWithTag("InfoManager") != null && gameObject.scene == SceneManager.GetSceneByName("ShipInterior"))
                GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>().GetInfo(GameObject.FindWithTag("ShopManager").GetComponent<Shop>());
            StartCoroutine(Transition());
        }
        else if (collision.tag == "Player" && !canGo)
            Instantiate(text, transform.position, Quaternion.identity, MainCanvas.transform);
    }
 
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(2);
            SceneManager.LoadScene(scene);
    }

    void CanEnter()
    {
        Satelites = GameObject.FindGameObjectsWithTag("Satelite");
        if(gameObject.scene == SceneManager.GetSceneByName("MainGame"))
        {
            foreach(GameObject satlite in Satelites)
            {
                NumOfSatelites++;
                if (satlite.GetComponent<SateliteHealth>().health>=100)
                {
                    thing++;
                }
            }
            if(thing >= NumOfSatelites)
            {
                GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>().GetSateliteInfo();
                canGo = true;
 GameObject.FindWithTag("InfoManager").GetComponent<InfoManager>().UpStuff();
                thing = 0;
            }
            else
            {
                NumOfSatelites = 0;
                thing = 0;
            }

           
        }
    }
}
