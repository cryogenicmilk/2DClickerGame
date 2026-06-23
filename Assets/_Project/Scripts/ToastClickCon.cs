using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToastClickCon : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] UIManager _uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button.onClick.AddListener(OnClickToastButton);
    }

    private void OnClickToastButton()
    {
        _scoreManager.AddBaseScore();
        _uiManager.UpdateUI();
    }
}
