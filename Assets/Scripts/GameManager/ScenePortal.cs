using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    public string destinationScene;
    public string actionLabel = "Entrar na nave";
    [Min(0.1f)] public float interactionRange = 2f;
    private Transform player;
    private bool loading;

    private void Start()
    {
        var obj = GameObject.FindWithTag("Player");
        if (obj != null) player = obj.transform;
    }

    private bool Nearby => player != null && Vector2.Distance(player.position, transform.position) <= interactionRange;

    public bool Travel()
    {
        if (loading || !Nearby) return false;
        if (!Application.CanStreamedLevelBeLoaded(destinationScene))
        {
            Debug.LogWarning($"A cena {destinationScene} precisa estar no Build Profile.");
            return false;
        }
        var state = RunState.GetOrCreate();
        foreach (var satellite in FindObjectsByType<Satelite>(FindObjectsSortMode.None)) state.Save(satellite);
        loading = true;
        SceneManager.LoadSceneAsync(destinationScene);
        return true;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) Travel();
    }

    private void OnGUI()
    {
        if (!Nearby) return;
        float width = Mathf.Min(260, Screen.width - 24);
        if (GUI.Button(new Rect((Screen.width - width) / 2, Screen.height - 48, width, 32), loading ? "A carregar..." : actionLabel + "  [E]")) Travel();
    }
}
