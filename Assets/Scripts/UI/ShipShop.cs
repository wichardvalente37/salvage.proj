using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

// Native uGUI controls, with serialized references for replacing the default layout.
public class ShipShop : MonoBehaviour
{
    public Canvas shopCanvas;
    public GameObject shopPanel;
    public Text balanceText;
    public Text inventoryText;
    public Text promptText;
    public Text feedbackText;
    public Button echoLocatorButton;
    public Button echoSensorButton;
    public Button soundBaitButton;
    public Button closeButton;
    private DeviceWorkshop workshop;
    private string feedback;
    private float feedbackUntil;
    private bool wired;
    private static ShipShop active;

    public bool IsOpen => shopPanel != null && shopPanel.activeSelf;

    private void Awake() => active = this;

    private void Start()
    {
        CreateCanvasUI();
        EnsureEventSystem();
        var player = GameObject.FindWithTag("Player");
        if (player != null) workshop = player.GetComponent<DeviceWorkshop>();
        WireButtons();
        Close();
    }

    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null) return;
        var obj = new GameObject("Shop EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        obj.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
    }

    private void WireButtons()
    {
        if (wired) return;
        echoLocatorButton.onClick.AddListener(BuyEchoLocator);
        echoSensorButton.onClick.AddListener(BuyEchoSensor);
        soundBaitButton.onClick.AddListener(BuySoundBait);
        closeButton.onClick.AddListener(Close);
        wired = true;
    }

    public bool Open()
    {
        ResolveWorkshop();
        if (workshop == null || !workshop.CanCraftHere) return false;
        shopPanel.SetActive(true);
        feedback = "";
        Refresh();
        return true;
    }

    public void Close()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    public void BuyEchoLocator() => Buy(DeviceType.EchoLocator);
    public void BuyEchoSensor() => Buy(DeviceType.EchoSensor);
    public void BuySoundBait() => Buy(DeviceType.SoundBait);

    private void Buy(DeviceType device)
    {
        ResolveWorkshop();
        if (!IsOpen || workshop == null || !workshop.CanCraftHere) return;
        bool bought = device == DeviceType.EchoLocator ? workshop.CraftEchoLocator() :
            device == DeviceType.EchoSensor ? workshop.CraftEchoSensor() : workshop.CraftSoundBait();
        feedback = bought ? "Dispositivo adicionado ao inventário." : "Sucata insuficiente.";
        feedbackUntil = Time.unscaledTime + 3f;
        Refresh();
    }

    private void ResolveWorkshop()
    {
        if (workshop != null) return;
        var player = GameObject.FindWithTag("Player");
        if (player != null) workshop = player.GetComponent<DeviceWorkshop>();
    }

    private void Update()
    {
        ResolveWorkshop();
        bool nearby = workshop != null && workshop.CanCraftHere;
        if (!nearby) Close();
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.iKey.wasPressedThisFrame) { if (IsOpen) Close(); else Open(); }
            if (keyboard.escapeKey.wasPressedThisFrame) Close();
        }
        Refresh();
    }

    private void Refresh()
    {
        if (shopCanvas == null) return;
        var state = RunState.GetOrCreate();
        inventoryText.text = $"Sucata: {state.Scrap}   |   EchoLocator: {state.EchoLocators}   EchoSensor: {state.EchoSensors}   Isca: {state.SoundBaits}";
        bool nearby = workshop != null && workshop.CanCraftHere;
        promptText.text = nearby && !IsOpen ? "Bancada · Abrir loja [I]" : "";
        if (!IsOpen || workshop == null) return;
        balanceText.text = $"Saldo: {state.Scrap} sucatas";
        SetProduct(echoLocatorButton, "EchoLocator", workshop.echoLocatorCost, state.Scrap);
        SetProduct(echoSensorButton, "EchoSensor", workshop.echoSensorCost, state.Scrap);
        SetProduct(soundBaitButton, "Isca sonora", workshop.soundBaitCost, state.Scrap);
        feedbackText.text = Time.unscaledTime < feedbackUntil ? feedback : "";
    }

    private void SetProduct(Button button, string name, int price, int scrap)
    {
        button.GetComponentInChildren<Text>().text = $"Comprar {name} — {price} sucatas";
        button.interactable = price > 0 && scrap >= price && workshop.CanCraftHere;
    }

    public static bool IsPointerOver(Vector2 screenPosition)
    {
        return active != null && active.IsOpen && RectTransformUtility.RectangleContainsScreenPoint(
            active.shopPanel.GetComponent<RectTransform>(), screenPosition, null);
    }

    // Can be run from the Inspector before Play to save/edit the real Canvas hierarchy.
    [ContextMenu("Criar Canvas da loja")]
    public void CreateCanvasUI()
    {
        if (shopCanvas != null) return; // Preserve an existing customized interface.
        var root = new GameObject("Ship Shop Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        root.transform.SetParent(transform, false);
        shopCanvas = root.GetComponent<Canvas>();
        shopCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        shopCanvas.sortingOrder = 20;
        var scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720);
        scaler.matchWidthOrHeight = 0.5f;
        inventoryText = Label("Inventory", root.transform, "", 14, TextAnchor.UpperLeft);
        Rect(inventoryText.gameObject, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(16, -16), new Vector2(760, 32));
        promptText = Label("Bench prompt", root.transform, "", 16, TextAnchor.MiddleCenter);
        Rect(promptText.gameObject, new Vector2(.5f, 0), new Vector2(.5f, 0), new Vector2(.5f, 0), new Vector2(0, 24), new Vector2(360, 32));
        shopPanel = new GameObject("Shop Panel", typeof(RectTransform), typeof(Image));
        shopPanel.transform.SetParent(root.transform, false);
        Rect(shopPanel, Vector2.one * .5f, Vector2.one * .5f, Vector2.one * .5f, Vector2.zero, new Vector2(380, 320));
        shopPanel.GetComponent<Image>().color = new Color(.15f, .15f, .15f, .98f);
        var title = Label("Title", shopPanel.transform, "LOJA DA NAVE", 20, TextAnchor.MiddleLeft);
        AtTop(title.gameObject, 20, -18, 260, 30);
        balanceText = Label("Scrap balance", shopPanel.transform, "", 15, TextAnchor.MiddleLeft);
        AtTop(balanceText.gameObject, 20, -52, 340, 24);
        echoLocatorButton = ProductButton("EchoLocator", shopPanel.transform, -88);
        echoSensorButton = ProductButton("EchoSensor", shopPanel.transform, -138);
        soundBaitButton = ProductButton("SoundBait", shopPanel.transform, -188);
        feedbackText = Label("Purchase feedback", shopPanel.transform, "", 14, TextAnchor.MiddleLeft);
        AtTop(feedbackText.gameObject, 20, -240, 340, 28);
        closeButton = ProductButton("Close", shopPanel.transform, -276);
        closeButton.GetComponentInChildren<Text>().text = "Fechar [I / Esc]";
        AtTop(closeButton.gameObject, 20, -276, 340, 28);
        shopPanel.SetActive(false);
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
#endif
    }

    private static void Rect(GameObject obj, Vector2 min, Vector2 max, Vector2 pivot, Vector2 pos, Vector2 size)
    {
        var r = obj.GetComponent<RectTransform>();
        r.anchorMin = min; r.anchorMax = max; r.pivot = pivot; r.anchoredPosition = pos; r.sizeDelta = size;
    }

    private static void AtTop(GameObject obj, float x, float y, float w, float h) =>
        Rect(obj, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(x, y), new Vector2(w, h));

    private Text Label(string name, Transform parent, string value, int size, TextAnchor alignment)
    {
        var obj = new GameObject(name, typeof(RectTransform), typeof(Text));
        obj.transform.SetParent(parent, false);
        var label = obj.GetComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.fontSize = size; label.text = value; label.alignment = alignment;
        label.color = Color.white; label.raycastTarget = false;
        return label;
    }

    private Button ProductButton(string name, Transform parent, float y)
    {
        var obj = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent, false);
        AtTop(obj, 20, y, 340, 42);
        obj.GetComponent<Image>().color = new Color(.3f, .3f, .3f, 1);
        var button = obj.GetComponent<Button>();
        button.targetGraphic = obj.GetComponent<Image>();
        var label = Label("Label", obj.transform, name, 16, TextAnchor.MiddleCenter);
        Rect(label.gameObject, Vector2.zero, Vector2.one, Vector2.one * .5f, Vector2.zero, Vector2.zero);
        return button;
    }

    private void OnDestroy() { if (active == this) active = null; }
}
