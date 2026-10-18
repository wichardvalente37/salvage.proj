using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance { get; private set; }
    [Range(0, 1)] public float effectsVolume = 0.65f;
    [Range(0, 1)] public float suspenseVolume = 0.3f;
    public float suspenseInterval = 20;
    private AudioSource effects;
    private AudioSource suspense;
    private AudioClip dash, sonar;
    private readonly List<AudioClip> ambienceClips = new();
    private MonsterBrain[] monsters;
    private bool wasChasing;
    private AudioClip[] repairs;
    private int repairIndex;
    private float nextScare;

   
  
    public void Acess()
    {
        EnsureAudio();
    }
 
   
 

    public static void EnsureAudio()
    {
        if(FindObjectsByType<MonsterBrain>() != null)
        {
 Instance.monsters = FindObjectsByType<MonsterBrain>();
        }
       
        if(Instance.monsters != null)
        foreach (var monster in Instance.monsters)
            if (monster.GetComponent<MonsterAudio>() == null) monster.gameObject.AddComponent<MonsterAudio>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        effects = gameObject.AddComponent<AudioSource>();
        suspense = gameObject.AddComponent<AudioSource>();
        effects.playOnAwake = suspense.playOnAwake = false;
        effects.spatialBlend = suspense.spatialBlend = 0;
        dash = Resources.Load<AudioClip>("GameAudio/dash");
        sonar = Resources.Load<AudioClip>("GameAudio/sonar_ping");
        foreach (var name in new[] { "scare", "suspense_1", "suspense_2", "suspense_3" })
        {
            var clip = Resources.Load<AudioClip>("GameAudio/" + name);
            if (clip != null) ambienceClips.Add(clip);
        }
        repairs = new[] { Resources.Load<AudioClip>("GameAudio/repair_1"), Resources.Load<AudioClip>("GameAudio/repair_2") };
        ScheduleScare();
    }

    private void ScheduleScare() => nextScare = Time.time + Random.Range(
        suspenseInterval,suspenseInterval+20f);

    private void Update()
    {
        bool chasing = false;
        if (monsters != null)
            foreach (var monster in monsters)
                if (monster != null && monster.IsChasing) { chasing = true; break; }
        if (chasing || SceneManager.GetActiveScene().name != "MainGame")
        {
            if (suspense.isPlaying) suspense.Stop();
            wasChasing = chasing;
            return;
        }
        if (wasChasing)
        {
            wasChasing = false;
            ScheduleScare(); // No immediate scare when pursuit ends.
        }
        if (Time.time < nextScare || suspense.isPlaying || ambienceClips.Count == 0) return;
        int index = Random.Range(0, ambienceClips.Count);
        suspense.PlayOneShot(ambienceClips[index], suspenseVolume);
        ScheduleScare();
    }

    public void PlayDash() { if (dash != null) effects.PlayOneShot(dash, effectsVolume); }
    public void PlaySonar() { if (sonar != null) effects.PlayOneShot(sonar, effectsVolume); }
    public void PlayRepair()
    {
        var clip = repairs[repairIndex];
        repairIndex = (repairIndex + 1) % repairs.Length;
        if (clip != null) effects.PlayOneShot(clip, effectsVolume);
    }

    private void OnDestroy() { if (Instance == this) Instance = null; }
}
