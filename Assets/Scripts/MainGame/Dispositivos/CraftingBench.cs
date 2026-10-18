using UnityEngine;

public class CraftingBench : MonoBehaviour
{
    [Min(0.1f)] public float interactionRange = 2.25f;

    public bool CanUse(Transform player)
    {
        if (!isActiveAndEnabled || player == null || gameObject.scene.name != "ShipInterior" ||
            player.gameObject.scene != gameObject.scene ||
            Vector2.Distance(player.position, transform.position) > interactionRange) return false;
        var hit = Physics2D.Linecast(player.position, transform.position, 1 << gameObject.layer);
        return hit.collider == null || hit.collider.gameObject == gameObject;
    }
}
