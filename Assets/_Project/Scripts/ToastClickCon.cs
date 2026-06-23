using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToastClickCon : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] UIManager _uiManager;
    [SerializeField] DamageCalculator _damageCalculator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button.onClick.AddListener(OnClickToastButton);
    }

    private void OnClickToastButton()
    {
        DamageResult result = _damageCalculator.CalcDMG();

        _scoreManager.AddScore(result);
        _uiManager.UpdateUI();
    }
}
