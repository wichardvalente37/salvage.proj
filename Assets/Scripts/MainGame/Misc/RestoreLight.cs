using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RestoreLight : MonoBehaviour
{
    float value;
    Light2D lighting;
    Color color;
    void Start()
    {
       lighting = GetComponent<Light2D>();

       value= lighting.intensity  ;
      color  = lighting.color ;
    }

     public void restore(bool can)
    {
        lighting.intensity = value;
        if (can)
        {
  lighting.color =color ;
        }
      
    }
}
