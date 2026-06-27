using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class UIManager : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ScoreManager _scoreManager;

    [Header("Upgrade")]
    [SerializeField] private DamageCalculator _damageCalculator;
    [SerializeField] private UpgradeManager _upgradeManager;

    [Header("oddsHint")]
    [SerializeField] private TextMeshProUGUI _critOddsText;
    [SerializeField] private TextMeshProUGUI _directOddsText;

    [Header("base")]
    [SerializeField] private TextMeshProUGUI _baseLvText;
    [SerializeField] private TextMeshProUGUI _baseCostText;

    [Header("crit")]
    [SerializeField] private TextMeshProUGUI _critLvText;
    [SerializeField] private TextMeshProUGUI _critCostText;

    [Header("direct")]
    [SerializeField] private TextMeshProUGUI _directLvText;
    [SerializeField] private TextMeshProUGUI _directCostText;

    [Header("Auto")]
    [SerializeField] private TextMeshProUGUI _autoLvText = null;
    [SerializeField] private TextMeshProUGUI _autoCostText = null;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        UpdateScoreUI();
        UpdateUpgradeUI();
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = $"Score : {_scoreManager.CurrentScore:0.0}";
    }

    private void UpdateUpgradeUI()
    {
        _critOddsText.text = $"{_damageCalculator.CritRate * 100f:0}%";
        _directOddsText.text = $"{_damageCalculator.DirectRate * 100f:0}%";

        _baseLvText.text = $"LV.{_upgradeManager.BaseLevel}";
        _baseCostText.text = $"{_upgradeManager.BaseCurrentCost:0}";

        _critLvText.text = $"LV.{_upgradeManager.CritLevel}";
        _critCostText.text = $"{_upgradeManager.CritCurrentCost:0}";

        _directLvText.text = $"LV.{_upgradeManager.DirectLevel}";
        _directCostText.text = $"{_upgradeManager.DirectCurrentCost:0}";

        //_autoLvText.text = $"LV.{_upgradeManager.AutoLevel}";
        //_autoCostText.text = $"{_upgradeManager.AutoCurrentCost}";
    }
}
