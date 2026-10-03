using System.Collections.Generic;
using UnityEngine;

public class SateliteManager : MonoBehaviour
{ int[] lastpoint = new int[20];
    private GameObject[] Satelites;
    private GameObject[] MonPoints;
    private void Start()
    {
        
        var state = RunState.GetOrCreate();
        if (GetComponent<RunTransition>() == null) gameObject.AddComponent<RunTransition>();
        var player = GameObject.FindWithTag("Player");
        if (player != null && player.GetComponent<DeviceWorkshop>() == null)
            player.AddComponent<DeviceWorkshop>();
        if (player != null && player.GetComponent<WorkshopHud>() == null)
            player.AddComponent<WorkshopHud>();

        var names = new HashSet<string>();
        foreach (var satellite in FindObjectsByType<Satelite>(FindObjectsSortMode.None))
        {
            if (string.IsNullOrWhiteSpace(satellite.Name) || !names.Add(satellite.Name))
            {
                Debug.LogError($"Satélite {satellite.gameObject.name}: Name tem de ser preenchido e único.", satellite);
                continue;
            }
            if (satellite.GetComponent<SoundBaitDevice>() == null) satellite.gameObject.AddComponent<SoundBaitDevice>();
            var health = satellite.GetComponent<SateliteHealth>();
            if (health == null) continue;
            if (!state.Restore(satellite))
            {
                // Initial damage happens once, never overwrites the previous run.
                health.health = Mathf.Clamp(Random.Range(health.health - 29, health.health - 10), 0, health.maxHealth);
                state.Save(satellite);
            }
        }
 int thing = 0;
        OrganizeSatelites(thing);
    }

    void OrganizeSatelites(int thing)
    {
        Satelites = GameObject.FindGameObjectsWithTag("Satelite");
        MonPoints = GameObject.FindGameObjectsWithTag("SatelitePoint");
        foreach (GameObject satelite in Satelites)
        {

            int rand = Random.Range(0, MonPoints.Length);
            foreach (int num in lastpoint)
            {
                while (num == rand)
                {
                    rand = Random.Range(0, MonPoints.Length);
                }
            }
            lastpoint[thing] = rand;
            satelite.transform.position = MonPoints[rand].transform.position;
            thing++;


        }

    }
}
