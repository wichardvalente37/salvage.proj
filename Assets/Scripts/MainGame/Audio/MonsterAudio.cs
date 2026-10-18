using UnityEngine;

[RequireComponent(typeof(MonsterBrain))]
public class MonsterAudio : MonoBehaviour
{
    [Range(0, 1)] public float breathingVolume = 0.18f;
    [Range(0, 1)] public float growlVolume = 0.55f;
    [Min(0)] public float growlCooldown = 2f;
    [Min(1)] public float breathingRadius = 15f;
    public Vector2 breathingInterval = new Vector2(20, 40);
    private MonsterBrain brain;
    private Transform player;
    private AudioSource breathing, voice;
    private AudioClip growl;
    private bool wasChasing;
    private float nextGrowl, nextBreath;

    private AudioSource Source()
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0.85f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 2;
        source.maxDistance = 25;
        source.dopplerLevel = 0;
        return source;
    }

    private void Awake()
    {
        brain = GetComponent<MonsterBrain>();
        var obj = GameObject.FindWithTag("Player");
        if (obj != null) player = obj.transform;
        breathing = Source();
        breathing.clip = Resources.Load<AudioClip>("GameAudio/monster_breathing");
        breathing.loop = false;
        voice = Source();
        growl = Resources.Load<AudioClip>("GameAudio/monster_growl");
    }

    private float BreathPause() => Random.Range(Mathf.Max(1, breathingInterval.x),
        Mathf.Max(Mathf.Max(1, breathingInterval.x), breathingInterval.y));

    private void OnEnable() { nextBreath = Time.time + Random.Range(3f, 8f); wasChasing = false; }
    private void OnDisable() { breathing.Stop(); voice.Stop(); }

    private void Update()
    {
        breathing.volume = breathingVolume;
        bool chasing = brain.IsChasing;
        if (chasing)
        {
            breathing.Stop();
            if (!wasChasing) nextGrowl = Time.time;
            if (Time.time >= nextGrowl && !voice.isPlaying && growl != null)
            {
                voice.PlayOneShot(growl, growlVolume);
                // A short pause after the complete clip, never overlapping growls.
                nextGrowl = Time.time + growl.length + Mathf.Max(0, growlCooldown);
            }
        }
        else
        {
            if (wasChasing)
            {
                voice.Stop();
                nextBreath = Time.time + BreathPause();
            }
            bool nearby = player != null && Vector2.Distance(player.position, transform.position) <= breathingRadius;
            if (!nearby) breathing.Stop();
            else if (!breathing.isPlaying && Time.time >= nextBreath && breathing.clip != null)
            {
                breathing.Play();
                nextBreath = Time.time + breathing.clip.length + BreathPause();
            }
        }
        wasChasing = chasing;
    }
}
