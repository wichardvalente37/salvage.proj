using UnityEngine;

[RequireComponent(typeof(Satelite))]
public class SoundBaitDevice : MonoBehaviour
{
    [Min(0.1f)] public float cooldown = 10f;
    [Min(0.1f)] public float attractionRadius = 25f;
    [Min(1)] public float suspicion = 10f;
    private float nextActivation;
    private Satelite satellite;

    private void Awake() => satellite = GetComponent<Satelite>();

    public bool Activate()
    {
        if (!satellite.hasSoundBait || Time.time < nextActivation) return false;
        nextActivation = Time.time + cooldown;
        foreach (var monster in FindObjectsByType<InvestigateBehaviour>(FindObjectsSortMode.None))
        {
            if (Vector2.Distance(transform.position, monster.transform.position) <= attractionRadius)
                monster.HearSound(transform.position, suspicion);
        }
        Debug.Log($"Isca sonora ativada em {satellite.Name}.");
        return true;
    }
}
