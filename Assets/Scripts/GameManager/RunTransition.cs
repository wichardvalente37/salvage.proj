using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RunTransition : MonoBehaviour
{
    private bool loading;
    public void NextRun()
    {
      
       
   
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame) NextRun();
    }

}
