using UnityEngine;

public class FlamePulse : MonoBehaviour
{
    public float pulseSpeed = 12f;
    public float minX = 0.85f;
    public float maxX = 1.2f;
    public float minY = 0.9f;
    public float maxY = 1.1f;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        float x = Mathf.Lerp(minX, maxX, t);
        float y = Mathf.Lerp(minY, maxY, 1f - t);

        transform.localScale = new Vector3(
            baseScale.x * x,
            baseScale.y * y,
            baseScale.z
        );
    }
}