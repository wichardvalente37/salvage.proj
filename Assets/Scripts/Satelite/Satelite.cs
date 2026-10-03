using Unity.VisualScripting;
using UnityEngine;

public class Satelite : MonoBehaviour
{
    public bool hasclicked;
    public bool hasEchoLocator;
    public bool hasSensor;
    public bool hasSoundBait;
    public string Name;
    [Header("---------Cenas do Sensor---------")]
    public float Radius;
    public LayerMask LayerDoMonstro;
    public float cooldown;
    private float lastSensed;
    void Start()
    {
        lastSensed = -cooldown;
    }

    bool hastouched;
    void Update()
    {
        if (hasSensor)
        {
            hastouched = Physics2D.OverlapCircle(transform.position, Radius, LayerDoMonstro);
        }

        if (hastouched)
        {
            if(Time.time < lastSensed + cooldown)
            {
                return;
            }

            Debug.Log("Motion Detected in " + Name);
            lastSensed = Time.time;
        }
    }

    private void OnMouseDown()
    {
        hasclicked = true;
    }

    private void OnMouseUp()
    {
        hasclicked = false;
    }
}
