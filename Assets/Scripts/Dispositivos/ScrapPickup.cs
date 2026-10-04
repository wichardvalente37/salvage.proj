using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScrapPickup : MonoBehaviour
{
    [Min(1)] public int amount = 1;
    private bool collected;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || amount <= 0) return;
        var player = other.GetComponentInParent<DeviceWorkshop>();
        if (player == null) return;
        collected = true;
        RunState.GetOrCreate().AddScrap(amount);
        Destroy(gameObject);
    }
}
