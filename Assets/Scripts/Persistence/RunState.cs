using System.Collections.Generic;
using UnityEngine;

// Stores values only: no references to objects belonging to an unloaded scene.
public class RunState : MonoBehaviour
{
    public static RunState Instance { get; private set; }
    public int Scrap { get; private set; }
    public int EchoLocators { get; private set; }
    public int EchoSensors { get; private set; }
    public int SoundBaits { get; private set; }

    private struct SatelliteState
    {
        public int health;
        public bool echoLocator, sensor, soundBait;
    }
    private readonly Dictionary<string, SatelliteState> satellites = new();

    public static RunState GetOrCreate()
    {
        if (Instance == null) new GameObject("RunState").AddComponent<RunState>();
        return Instance;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject); // Protects even a transition requested before Start.
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void AddScrap(int amount)
    {
        if (amount > 0) Scrap += amount;
    }

    public bool Craft(DeviceType device, int cost)
    {
        if (cost <= 0 || Scrap < cost || !IsValid(device)) return false;
        Scrap -= cost;
        switch (device)
        {
            case DeviceType.EchoLocator: EchoLocators++; break;
            case DeviceType.EchoSensor: EchoSensors++; break;
            case DeviceType.SoundBait: SoundBaits++; break;
        }
        return true;
    }

    public bool Consume(DeviceType device)
    {
        switch (device)
        {
            case DeviceType.EchoLocator:
                if (EchoLocators <= 0) return false;
                EchoLocators--; return true;
            case DeviceType.EchoSensor:
                if (EchoSensors <= 0) return false;
                EchoSensors--; return true;
            case DeviceType.SoundBait:
                if (SoundBaits <= 0) return false;
                SoundBaits--; return true;
            default: return false;
        }
    }

    public static bool IsValid(DeviceType device) =>
        device == DeviceType.EchoLocator || device == DeviceType.EchoSensor || device == DeviceType.SoundBait;

    public bool Restore(Satelite satellite)
    {
        if (string.IsNullOrWhiteSpace(satellite.Name) || !satellites.TryGetValue(satellite.Name, out var state)) return false;
        satellite.GetComponent<SateliteHealth>().health = state.health;
        satellite.hasEchoLocator = state.echoLocator;
        satellite.hasSensor = state.sensor;
        satellite.hasSoundBait = state.soundBait;
        return true;
    }

    public void Save(Satelite satellite)
    {
        if (string.IsNullOrWhiteSpace(satellite.Name)) return;
        var health = satellite.GetComponent<SateliteHealth>();
        if (health == null) return;
        satellites[satellite.Name] = new SatelliteState
        {
            health = health.health,
            echoLocator = satellite.hasEchoLocator,
            sensor = satellite.hasSensor,
            soundBait = satellite.hasSoundBait
        };
    }
}

public enum DeviceType { EchoLocator, EchoSensor, SoundBait }
