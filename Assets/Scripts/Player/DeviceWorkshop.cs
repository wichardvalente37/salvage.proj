using UnityEngine;
using UnityEngine.InputSystem;

// Attached automatically by SateliteManager. Public methods also support UI buttons.
public class DeviceWorkshop : MonoBehaviour
{
    [Min(1)] public int echoLocatorCost = 5;
    [Min(1)] public int echoSensorCost = 8;
    [Min(1)] public int soundBaitCost = 3;
    [Min(0.1f)] public float installationRange = 3f;

    public bool CraftEchoLocator() => Craft(DeviceType.EchoLocator, echoLocatorCost);
    public bool CraftEchoSensor() => Craft(DeviceType.EchoSensor, echoSensorCost);
    public bool CraftSoundBait() => Craft(DeviceType.SoundBait, soundBaitCost);
    public bool InstallEchoLocator() => InstallNearest(DeviceType.EchoLocator);
    public bool InstallEchoSensor() => InstallNearest(DeviceType.EchoSensor);
    public bool InstallSoundBait() => InstallNearest(DeviceType.SoundBait);

    public bool CanCraftHere
    {
        get
        {
            foreach (var bench in FindObjectsByType<CraftingBench>(FindObjectsSortMode.None))
                if (bench.CanUse(transform)) return true;
            return false;
        }
    }

    private bool Craft(DeviceType device, int cost)
    {
        if (!CanCraftHere)
        {
            Debug.Log("Só podes construir junto da bancada, na oficina da nave.");
            return false;
        }
        bool result = RunState.GetOrCreate().Craft(device, cost);
        Debug.Log(result ? $"Construído: {device}" : "Sucata insuficiente ou custo inválido.");
        return result;
    }

    public bool TryInstall(Satelite satellite, DeviceType device)
    {
        if (satellite == null || !satellite.isActiveAndEnabled || !RunState.IsValid(device) ||
            Vector2.Distance(transform.position, satellite.transform.position) > installationRange) return false;
        if ((device == DeviceType.EchoLocator && satellite.hasEchoLocator) ||
            (device == DeviceType.EchoSensor && satellite.hasSensor) ||
            (device == DeviceType.SoundBait && satellite.hasSoundBait)) return false;
        if (!RunState.GetOrCreate().Consume(device)) return false;

        switch (device)
        {
            case DeviceType.EchoLocator: satellite.hasEchoLocator = true; break;
            case DeviceType.EchoSensor:
                satellite.hasEchoLocator = true;
                satellite.hasSensor = true;
                break;
            case DeviceType.SoundBait: satellite.hasSoundBait = true; break;
        }
        RunState.Instance.Save(satellite);
        Debug.Log($"{device} instalado em {satellite.Name}.");
        return true;
    }

    private bool InstallNearest(DeviceType device)
    {
        Satelite nearest = null;
        float distance = installationRange;
        foreach (var satellite in FindObjectsByType<Satelite>(FindObjectsSortMode.None))
        {
            float candidate = Vector2.Distance(transform.position, satellite.transform.position);
            if (candidate <= distance) { nearest = satellite; distance = candidate; }
        }
        bool result = TryInstall(nearest, device);
        if (!result) Debug.Log("Instalação recusada: aproxima-te de um satélite, verifica o inventário e os dispositivos instalados.");
        return result;
    }

    public bool ActivateNearestBait()
    {
        Satelite nearest = null;
        float distance = installationRange;
        foreach (var satellite in FindObjectsByType<Satelite>(FindObjectsSortMode.None))
        {
            float candidate = Vector2.Distance(transform.position, satellite.transform.position);
            if (candidate <= distance) { nearest = satellite; distance = candidate; }
        }
        var bait = nearest != null ? nearest.GetComponent<SoundBaitDevice>() : null;
        bool result = bait != null && bait.Activate();
        if (!result) Debug.Log("Aproxima-te de um satélite com isca instalada e aguarda o cooldown.");
        return result;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        if (keyboard.digit1Key.wasPressedThisFrame) CraftEchoLocator();
        if (keyboard.digit2Key.wasPressedThisFrame) CraftEchoSensor();
        if (keyboard.digit3Key.wasPressedThisFrame) CraftSoundBait();
        if (keyboard.fKey.wasPressedThisFrame) InstallEchoLocator();
        if (keyboard.gKey.wasPressedThisFrame) InstallEchoSensor();
        if (keyboard.hKey.wasPressedThisFrame) InstallSoundBait();
        if (keyboard.bKey.wasPressedThisFrame) ActivateNearestBait();
    }
}
