using UnityEngine;

public class BeaconBlink : MonoBehaviour
{
    [SerializeField] private SpriteRenderer beaconRenderer;

    [Header("Blink Settings")]
    [SerializeField] private float blinkSpeed = 5f;
    [SerializeField, Range(0f, 1f)] private float minimumAlpha = 0.25f;
    [SerializeField, Range(0f, 1f)] private float maximumAlpha = 1f;

    private void Awake()
    {
        if (beaconRenderer == null)
        {
            beaconRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        if (beaconRenderer == null)
        {
            return;
        }

        float blinkValue =
            (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f;

        Color currentColor = beaconRenderer.color;

        currentColor.a = Mathf.Lerp(
            minimumAlpha,
            maximumAlpha,
            blinkValue
        );

        beaconRenderer.color = currentColor;
    }
}