using UnityEngine;
<<<<<<< Updated upstream:Assets/Scripts/GameManager/OpenCam.cs
=======
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
>>>>>>> Stashed changes:Assets/Scripts/MainGame/Managers/GameManager/OpenCam.cs

public class OpenCam : MonoBehaviour
{
    private GameObject CamCam;
    private GameObject PlayerCam;
    public GameObject CammedAstronaut;
    public GameObject CammedSatelite;
<<<<<<< Updated upstream:Assets/Scripts/GameManager/OpenCam.cs
    void Start()
=======
    public GameObject canvasDoMain;
  
    private readonly Dictionary<SpriteRenderer, bool> originalVisibility = new();
    private readonly Dictionary<Image, bool> originalVisibil = new();
    private readonly Dictionary<Satelite, GameObject> satelliteMarkers = new();
    private readonly Dictionary<Light2D, bool> light = new();
    private GameObject playerMarker;
    private Transform player;
    private bool mapOpen;

    private void Start()
>>>>>>> Stashed changes:Assets/Scripts/MainGame/Managers/GameManager/OpenCam.cs
    {
        CamCam = GameObject.FindWithTag("CamCam");
        PlayerCam = GameObject.FindWithTag("MainCamera");
        CamCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)&&PlayerCam.activeSelf){

<<<<<<< Updated upstream:Assets/Scripts/GameManager/OpenCam.cs
            PlayerCam.SetActive(false);
            CamCam.SetActive(true);
            Hide();
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !PlayerCam.activeSelf)
        {
=======
    private void Hide()
    {
        originalVisibility.Clear();
        canvasDoMain.SetActive(false);
        foreach (var renderer in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
        {
            originalVisibility.Add(renderer, renderer.enabled);
            renderer.enabled = false;
        }
        foreach(var ligh in FindObjectsByType<Light2D>(FindObjectsSortMode.None))
        {
            if(ligh.lightType != Light2D.LightType.Sprite&&ligh.gameObject.tag != "Sonar")
            {
   light.Add(ligh, ligh.enabled);
            ligh.enabled = false;
            }
         
        }
        foreach(Image image in FindObjectsByType<Image>())
        {
            
            originalVisibil.Add(image, image.enabled);
            image.enabled = false;
        }
     
      
        if (CammedAstronaut != null && player != null)
            playerMarker = Instantiate(CammedAstronaut, player.position, Quaternion.identity);
        
    }
>>>>>>> Stashed changes:Assets/Scripts/MainGame/Managers/GameManager/OpenCam.cs

            PlayerCam.SetActive(true);
            CamCam.SetActive(false);
            Show();
        }

    }
    GameObject[] cammeds = new GameObject[20];
  int thing = 0;
    void Hide()
    {
<<<<<<< Updated upstream:Assets/Scripts/GameManager/OpenCam.cs
       
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
            if(satlite.GetComponent<Satelite>().hasEchoLocator)
            {
              thing++;
            cammeds[thing] = Instantiate(CammedSatelite, satlite.transform.position, Quaternion.identity);
            }
           
        }

    }
    void Show()
    {
        GameObject[] Stuff = GameObject.FindObjectsByType<GameObject>();

        foreach (GameObject Obj in Stuff)
        {
            if (Obj.GetComponent<SpriteRenderer>() != null && Obj.tag != "Satelite")
            {
                Obj.GetComponent<SpriteRenderer>().enabled = true;
            }
           
        }
        foreach(GameObject cammed in cammeds)
        {
            Destroy(cammed);
        }
        thing = 0;
=======


        canvasDoMain.SetActive(true);
        foreach (var entry in light)
            if (entry.Key != null) entry.Key.enabled = entry.Value;
        light.Clear();
        foreach (var entry in originalVisibil)
            if (entry.Key != null) entry.Key.enabled = entry.Value;
        originalVisibil.Clear();
        foreach (var entry in originalVisibility)
            if (entry.Key != null) entry.Key.enabled = entry.Value;
        originalVisibility.Clear();
        if (playerMarker != null) Destroy(playerMarker);
        foreach (var marker in satelliteMarkers.Values) if (marker != null) Destroy(marker);
        satelliteMarkers.Clear();
        
>>>>>>> Stashed changes:Assets/Scripts/MainGame/Managers/GameManager/OpenCam.cs
    }
}
