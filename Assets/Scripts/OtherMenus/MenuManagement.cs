using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagement : MonoBehaviour
{
    public void restart()
    {
        StartCoroutine(Restart());
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("ShipInterior");
    }

    IEnumerator Menu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("ShipInterior");
    }
}
