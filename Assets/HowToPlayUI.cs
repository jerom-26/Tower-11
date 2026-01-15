using UnityEngine;

public class HowToPlayUI : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayPanel;

    public void Open()
    {
        if (howToPlayPanel == null) return;
        howToPlayPanel.SetActive(true);
    }

    public void Close()
    {
        if (howToPlayPanel == null) return;
        howToPlayPanel.SetActive(false);
    }
}
