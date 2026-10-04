using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RunTransition : MonoBehaviour
{
    private bool loading;
    public void NextRun()
    {
        if (loading) return;
        var scene = gameObject.scene;
        if (scene.buildIndex < 0)
        {
            Debug.LogWarning("Adiciona esta cena ao Build Profile antes de iniciar a próxima run.");
            return;
        }
        // Save before unloading even if a satellite's health was changed by other scripts.
        var state = RunState.GetOrCreate();
        foreach (var satellite in FindObjectsByType<Satelite>(FindObjectsSortMode.None)) state.Save(satellite);
        loading = true;
        SceneManager.LoadSceneAsync(scene.buildIndex);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame) NextRun();
    }

}
