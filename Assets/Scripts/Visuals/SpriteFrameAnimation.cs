using UnityEngine;

public class SpriteFrameAnimation : MonoBehaviour
{
    public SpriteRenderer target;
    public Sprite[] idleFrames;
    public Sprite[] repairFrames;
    [Min(1)] public float framesPerSecond = 8f;
    private float repairUntil;
    private float repairStarted;

    public void PlayRepair()
    {
        if (repairFrames == null || repairFrames.Length == 0) return;
        repairStarted = Time.time;
        repairUntil = repairStarted + repairFrames.Length / Mathf.Max(1f, framesPerSecond);
        ApplyFrame();
    }

    private void Awake()
    {
        if (target == null) target = GetComponentInChildren<SpriteRenderer>(true);
        ApplyFrame();
    }

    private void Update() => ApplyFrame();

    private void ApplyFrame()
    {
        if (target == null) return;
        bool repairing = Time.time < repairUntil && repairFrames != null && repairFrames.Length > 0;
        Sprite[] frames = repairing ? repairFrames : idleFrames;
        if (frames == null || frames.Length == 0) return;
        float elapsed = repairing ? Time.time - repairStarted : Time.time;
        int index = Mathf.FloorToInt(elapsed * Mathf.Max(1f, framesPerSecond)) % frames.Length;
        target.sprite = frames[index];
    }
}
