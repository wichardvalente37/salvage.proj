using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenCam : MonoBehaviour
{
    private GameObject CamCam;
    private GameObject PlayerCam;
    public GameObject CammedAstronaut;
    public GameObject CammedSatelite;
    private readonly Dictionary<SpriteRenderer, bool> originalVisibility = new();
    private readonly Dictionary<Satelite, GameObject> satelliteMarkers = new();
    private GameObject playerMarker;
    private Transform player;
    private bool mapOpen;

    private void Start()
    {
        CamCam = GameObject.FindWithTag("CamCam");
        PlayerCam = GameObject.FindWithTag("MainCamera");
        var playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
        if (CamCam != null) CamCam.SetActive(false);
    }

    private void Update()
    {
        if (CamCam == null || PlayerCam == null) return;
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            mapOpen = !mapOpen;
            PlayerCam.SetActive(!mapOpen);
            CamCam.SetActive(mapOpen);
            if (mapOpen) Hide(); else Show();
        }
        if (!mapOpen) return;
        if (playerMarker != null && player != null) playerMarker.transform.position = player.position;
        foreach (var satellite in FindObjectsByType<Satelite>(FindObjectsSortMode.None))
        {
            if (!satellite.hasEchoLocator || satelliteMarkers.ContainsKey(satellite) || CammedSatelite == null) continue;
            satelliteMarkers.Add(satellite, Instantiate(CammedSatelite, satellite.transform.position, Quaternion.identity));
        }
        foreach (var entry in satelliteMarkers)
            if (entry.Key != null && entry.Value != null) entry.Value.transform.position = entry.Key.transform.position;
    }

    private void Hide()
    {
        originalVisibility.Clear();
        foreach (var renderer in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
        {
            originalVisibility.Add(renderer, renderer.enabled);
            renderer.enabled = false;
        }
        if (CammedAstronaut != null && player != null)
            playerMarker = Instantiate(CammedAstronaut, player.position, Quaternion.identity);
    }

    private void Show()
    {
        // Restore each renderer's previous state, including the new satellite art.
        foreach (var entry in originalVisibility)
            if (entry.Key != null) entry.Key.enabled = entry.Value;
        originalVisibility.Clear();
        if (playerMarker != null) Destroy(playerMarker);
        foreach (var marker in satelliteMarkers.Values) if (marker != null) Destroy(marker);
        satelliteMarkers.Clear();
    }
}
