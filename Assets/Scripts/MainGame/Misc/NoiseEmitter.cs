using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
   
    private GameObject SusSound;
    public GameObject SusObj;
    
   public void EmitSound(float SusAmount)
    {
 SusSound = Instantiate(SusObj, transform.position, Quaternion.identity);
        SusSound.GetComponent<SoundObject>().SusAmount = SusAmount;
    }
}
