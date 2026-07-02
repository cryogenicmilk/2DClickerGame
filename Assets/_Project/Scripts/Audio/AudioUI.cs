using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private GameObject _audioPanel = default;

    [SerializeField] private Button _toggleAudioPanel = default;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioPanel.SetActive(false);

        _toggleAudioPanel.onClick.AddListener(ToggleAudioPanel);
    }

    private void ToggleAudioPanel()
    {
        _audioPanel.SetActive(!_audioPanel.activeSelf);
    }
}
