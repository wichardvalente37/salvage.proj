using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public float SusAmount;
    public GameObject suspos; // Kept to preserve the existing prefab's serialized field.
    [Min(0.1f)] public float propagationSpeed = 8f;
    private InvestigateBehaviour monster;
    private Vector2 origin;

    private void Start()
    {
        origin = transform.position;
        var target = GameObject.FindWithTag("Monster");
        if (target != null) monster = target.GetComponent<InvestigateBehaviour>();
        if (monster == null) Destroy(gameObject);
    }

    private void Update()
    {
        if (monster == null) { Destroy(gameObject); return; }
        transform.position = Vector2.MoveTowards(transform.position, monster.transform.position,
            propagationSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, monster.transform.position) < 0.3f)
        {
            monster.HearSound(origin, SusAmount);
            Destroy(gameObject);
        }
    }
}
