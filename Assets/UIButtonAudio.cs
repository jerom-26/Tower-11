using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler
{
    [Header("UI Audio")]
    [SerializeField] private AudioSource uiAudioSource;

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)]
    private float hoverVolume = 0.4f;

    [SerializeField, Range(0f, 1f)]
    private float clickVolume = 0.6f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiAudioSource != null && hoverSound != null)
        {
            uiAudioSource.PlayOneShot(
                hoverSound,
                hoverVolume
            );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.PlayOneShot(
                clickSound,
                clickVolume
            );
        }
    }
}