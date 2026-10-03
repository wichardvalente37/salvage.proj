using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    public GameObject soundPrefab;

    public void MakeNoise(Vector2 position, float susAmount)
    {
        GameObject sound = Instantiate(
            soundPrefab,
            position,
            Quaternion.identity
        );

        SoundObject soundObject = sound.GetComponent<SoundObject>();

        if (soundObject != null)
        {
            soundObject.SusAmount = susAmount;
        }
    }
}