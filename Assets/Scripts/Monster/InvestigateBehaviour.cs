using UnityEngine;

public class InvestigateBehaviour : MonoBehaviour
{
    public float InvestigateSpeed;
    public float SusAmount;
    public bool isInvestigating;
    private Vector2 suspiciousPosition;
    private int searchedPoints;
    private float nextSearchTime;

    // Noise supplies its own position; no temporary object/tag lookup is needed.
    public void HearSound(Vector2 position, float amount)
    {
        if (amount <= 0) return;
        SusAmount += amount;
        if (SusAmount < 10) return;
        suspiciousPosition = position;
        isInvestigating = true;
        searchedPoints = 0;
        nextSearchTime = 0;
    }

    private void Update()
    {
        var brain = GetComponent<MonsterBrain>();
        if (brain != null && brain.IsChasing)
        {
            isInvestigating = false;
            SusAmount = 0;
            return;
        }
        if (!isInvestigating) return;
        if (Vector2.Distance(transform.position, suspiciousPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, suspiciousPosition,
                InvestigateSpeed * Time.deltaTime);
            nextSearchTime = Time.time + 3f;
            return;
        }
        if (Time.time < nextSearchTime) return;
        if (++searchedPoints > 4)
        {
            SusAmount = 0;
            isInvestigating = false;
            return;
        }
        suspiciousPosition += Random.insideUnitCircle * 3f;
        nextSearchTime = Time.time + Random.Range(3f, 8f);
    }
}
