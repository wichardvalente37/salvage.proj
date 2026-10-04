using UnityEngine;

public class ShipSceneController : MonoBehaviour
{
    private Camera shipCamera;
    private void Start()
    {
        RunState.GetOrCreate();
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            if (player.GetComponent<DeviceWorkshop>() == null) player.AddComponent<DeviceWorkshop>();
            if (player.GetComponent<WorkshopHud>() == null) player.AddComponent<WorkshopHud>();
        }
        shipCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (shipCamera != null) shipCamera.orthographicSize = Mathf.Max(10f, 10f / Mathf.Max(0.1f, shipCamera.aspect));
    }

}
