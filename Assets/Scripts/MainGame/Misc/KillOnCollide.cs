using UnityEngine;

public class KillOnCollide : MonoBehaviour
{
    public string Objtag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Objtag)
        {
            Destroy(collision.gameObject);
        }

    }
}
