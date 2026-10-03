using UnityEngine;

public class OpenCam : MonoBehaviour
{
    private GameObject CamCam;
    private GameObject PlayerCam;
    public GameObject CammedAstronaut;
    public GameObject CammedSatelite;
    void Start()
    {
        CamCam = GameObject.FindWithTag("CamCam");
        PlayerCam = GameObject.FindWithTag("MainCamera");
        CamCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&PlayerCam.activeSelf){

            PlayerCam.SetActive(false);
            CamCam.SetActive(true);
            Hide();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !PlayerCam.activeSelf)
        {

            PlayerCam.SetActive(true);
            CamCam.SetActive(false);
            Show();
        }

    }
    GameObject[] cammeds = new GameObject[20];
  int thing = 0;
    void Hide()
    {
       
        GameObject[] Stuff = GameObject.FindObjectsByType<GameObject>();
        GameObject[] Satelites = GameObject.FindGameObjectsWithTag("Satelite");
        foreach (GameObject Obj in Stuff)
        {
            if(Obj.GetComponent<SpriteRenderer>() != null)
            {
                Obj.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
       
        cammeds[thing] = Instantiate(CammedAstronaut, GameObject.FindWithTag("Player").transform.position, Quaternion.identity);
        
        foreach(GameObject satlite in Satelites)
        {
            thing++;
            cammeds[thing] = Instantiate(CammedSatelite, satlite.transform.position, Quaternion.identity);
        }

    }
    void Show()
    {
        GameObject[] Stuff = GameObject.FindObjectsByType<GameObject>();

        foreach (GameObject Obj in Stuff)
        {
            if (Obj.GetComponent<SpriteRenderer>() != null)
            {
                Obj.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        foreach(GameObject cammed in cammeds)
        {
            Destroy(cammed);
        }
    }
}
