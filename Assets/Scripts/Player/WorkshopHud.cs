using UnityEngine;

// Compatibility bridge for existing scene scripts. All UI is handled by ShipShop's Canvas.
[RequireComponent(typeof(DeviceWorkshop))]
public class WorkshopHud : MonoBehaviour
{
<<<<<<< HEAD
    public static bool IsPointerOver(Vector2 screenPosition) => ShipShop.IsPointerOver(screenPosition);
=======
    private static WorkshopHud active;
    private DeviceWorkshop workshop;
    private bool expanded;
    private int tab;
    private Texture2D background, surface, accent;
    private GUIStyle title, text, muted, button, selected;
    private const float Width = 270f;
    private float Scale => Mathf.Min(1f, Screen.width / 960f, Screen.height / 640f);
    private Rect Panel => new Rect(12, 12, Width, expanded ? 240 : 42);

    private void Awake() => workshop = GetComponent<DeviceWorkshop>();
    private void OnEnable() => active = this;
    private void OnDisable() { if (active == this) active = null; }

    public static bool IsPointerOver(Vector2 screenPosition)
    {
        if (active == null) return false;
        var point = new Vector2(screenPosition.x, Screen.height - screenPosition.y) / active.Scale;
        return active.Panel.Contains(point);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame) expanded = !expanded;
    }

    private Texture2D ColorTexture(Color color)
    {
        var texture = new Texture2D(1, 1);
        texture.hideFlags = HideFlags.HideAndDontSave;
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private void PrepareStyles()
    {
        if (background != null) return;
        background = ColorTexture(new Color(0.04f, 0.065f, 0.10f, 0.96f));
        surface = ColorTexture(new Color(0.10f, 0.15f, 0.21f, 1));
        accent = ColorTexture(new Color(0.13f, 0.77f, 0.82f, 1));
        title = new GUIStyle(GUI.skin.label) { fontSize = 14, fontStyle = FontStyle.Bold };
        title.normal.textColor = new Color(0.85f, 0.96f, 1);
        text = new GUIStyle(GUI.skin.label) { fontSize = 12 };
        text.normal.textColor = new Color(0.86f, 0.9f, 0.95f);
        muted = new GUIStyle(text) { fontSize = 11, wordWrap = true };
        muted.normal.textColor = new Color(0.55f, 0.68f, 0.78f);
        button = new GUIStyle(GUI.skin.button) { fontSize = 12, alignment = TextAnchor.MiddleCenter };
        button.normal.background = surface;
        button.normal.textColor = text.normal.textColor;
        button.hover.background = accent;
        button.hover.textColor = Color.black;
        button.active.background = accent;
        button.active.textColor = Color.black;
        selected = new GUIStyle(button);
        selected.normal.background = accent;
        selected.normal.textColor = Color.black;
    }

    private void OnGUI()
    {
        PrepareStyles();
        var state = RunState.GetOrCreate();
        var previousMatrix = GUI.matrix;
        var previousColor = GUI.color;
        bool previousEnabled = GUI.enabled;
        GUI.matrix = Matrix4x4.Scale(new Vector3(Scale, Scale, 1));
        GUI.color = Color.white;
        GUI.DrawTexture(Panel, background);
        GUI.DrawTexture(new Rect(12, 12, 3, Panel.height), accent);
        GUI.Label(new Rect(24, 22, 120, 24), $"SUCATA  {state.Scrap}", title);
        if (GUI.Button(new Rect(162, 20, 108, 26), expanded ? "Fechar  [I]" : "Oficina  [I]", button)) expanded = !expanded;
        if (expanded)
        {
            string[] tabs = { "Construir", "Satélite", "Run" };
            for (int i = 0; i < tabs.Length; i++)
                if (GUI.Button(new Rect(24 + i * 82, 62, 78, 28), tabs[i], tab == i ? selected : button)) tab = i;
            if (tab == 0) DrawCrafting(state);
            else if (tab == 1) DrawSatellite(state);
            else DrawRun();
        }
        GUI.enabled = previousEnabled;
        GUI.color = previousColor;
        GUI.matrix = previousMatrix;
    }

    private void DrawCrafting(RunState state)
    {
        if (!workshop.CanCraftHere)
        {
            GUI.Label(new Rect(24, 105, 246, 70), "Para fabricar, entra na nave e aproxima-te da bancada da oficina.", muted);
            GUI.Label(new Rect(24, 184, 246, 45), $"Inventário: {state.EchoLocators} localizadores · {state.EchoSensors} sensores · {state.SoundBaits} iscas", muted);
            return;
        }
        CraftRow(102, "EchoLocator", state.EchoLocators, workshop.echoLocatorCost, state.Scrap, 1);
        CraftRow(142, "EchoSensor", state.EchoSensors, workshop.echoSensorCost, state.Scrap, 2);
        CraftRow(182, "Isca sonora", state.SoundBaits, workshop.soundBaitCost, state.Scrap, 3);
        GUI.Label(new Rect(24, 225, 246, 22), "1 / 2 / 3: construir junto da bancada.", muted);
    }

    private void CraftRow(float y, string name, int count, int cost, int scrap, int key)
    {
        GUI.Label(new Rect(24, y, 150, 20), $"{name}   ×{count}", text);
        GUI.Label(new Rect(24, y + 18, 150, 18), $"{cost} sucatas · tecla {key}", muted);
        GUI.enabled = workshop.CanCraftHere && cost > 0 && scrap >= cost;
        if (GUI.Button(new Rect(181, y + 2, 89, 30), "Construir", button))
        {
            if (key == 1) workshop.CraftEchoLocator();
            else if (key == 2) workshop.CraftEchoSensor();
            else workshop.CraftSoundBait();
        }
        GUI.enabled = true;
    }

    private void DrawSatellite(RunState state)
    {
        Satelite nearest = null;
        float distance = workshop.installationRange;
        foreach (var satellite in FindObjectsByType<Satelite>(FindObjectsSortMode.None))
        {
            float candidate = Vector2.Distance(transform.position, satellite.transform.position);
            if (candidate <= distance) { nearest = satellite; distance = candidate; }
        }
        if (nearest == null)
        {
            GUI.Label(new Rect(24, 105, 246, 70), "Aproxima-te de um satélite para instalar os dispositivos ou ativar a isca.", muted);
            return;
        }
        var health = nearest.GetComponent<SateliteHealth>();
        GUI.Label(new Rect(24, 99, 246, 22), nearest.Name + (health != null ? $"  ·  Vida {health.health}/{health.maxHealth}" : ""), text);
        InstallButton(new Rect(24, 127, 246, 27), nearest.hasEchoLocator ? "EchoLocator instalado" : "Instalar EchoLocator  [F]", state.EchoLocators > 0 && !nearest.hasEchoLocator, nearest, DeviceType.EchoLocator);
        InstallButton(new Rect(24, 158, 246, 27), nearest.hasSensor ? "EchoSensor instalado" : "Instalar EchoSensor  [G]", state.EchoSensors > 0 && !nearest.hasSensor, nearest, DeviceType.EchoSensor);
        InstallButton(new Rect(24, 189, 246, 27), nearest.hasSoundBait ? "Isca sonora instalada" : "Instalar isca sonora  [H]", state.SoundBaits > 0 && !nearest.hasSoundBait, nearest, DeviceType.SoundBait);
        GUI.enabled = nearest.hasSoundBait;
        if (GUI.Button(new Rect(24, 220, 246, 24), "Ativar isca  [B]", button)) workshop.ActivateNearestBait();
        GUI.enabled = true;
    }

    private void InstallButton(Rect rect, string label, bool enabled, Satelite satellite, DeviceType device)
    {
        GUI.enabled = enabled;
        if (GUI.Button(rect, label, button)) workshop.TryInstall(satellite, device);
        GUI.enabled = true;
    }

    private void DrawRun()
    {
        GUI.Label(new Rect(24, 106, 246, 60), "Recomeça a cena e mantém a vida dos satélites, os dispositivos e o inventário.", muted);
        var transition = FindFirstObjectByType<RunTransition>();
        GUI.enabled = transition != null;
        if (GUI.Button(new Rect(24, 176, 246, 32), "Próxima run  [N]", button)) transition.NextRun();
        GUI.enabled = true;
        GUI.Label(new Rect(24, 218, 246, 24), "Tab: CamCam  ·  E: reparar", muted);
    }

    private void OnDestroy()
    {
        if (background != null) Destroy(background);
        if (surface != null) Destroy(surface);
        if (accent != null) Destroy(accent);
    }
>>>>>>> b0f3b9a75058e085856ef6d48a1be873b8f6a605
}
