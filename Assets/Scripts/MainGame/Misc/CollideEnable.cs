using UnityEngine;

public class CollideEnable : MonoBehaviour
{
    public GameObject canvas;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            canvas.SetActive(false);
    }
}
