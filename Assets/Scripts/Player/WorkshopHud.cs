using UnityEngine;

// Compatibility bridge for existing scene scripts. All UI is handled by ShipShop's Canvas.
[RequireComponent(typeof(DeviceWorkshop))]
public class WorkshopHud : MonoBehaviour
{
    public static bool IsPointerOver(Vector2 screenPosition) => ShipShop.IsPointerOver(screenPosition);
}
