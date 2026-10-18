using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource MonsChaseSong;
    public AudioSource SkinWlkChaseSong;
    private AudioSource currentmusic;
    private AudioSource lastmusic;
    void Start()
    {
        
    }

    public void MonsChase() {
        currentmusic = MonsChaseSong;
        lastmusic = currentmusic;

        if(currentmusic.volume != 1)
        {
            currentmusic.volume += 1 * Time.deltaTime;
           
        }
 if(lastmusic != MonsChaseSong)
            {
                lastmusic.volume -= 1 * Time.deltaTime;
            }
    }

    public void DisableMusic()
    {
        if(currentmusic != null)
        {
 if (currentmusic.volume != 0)
        {
            currentmusic.volume -= 0.7f * Time.deltaTime;

        }
        if (lastmusic != currentmusic && lastmusic.volume != 0)
        {
            lastmusic.volume -= 0.7f * Time.deltaTime;
        }
        }
       
    }
}
