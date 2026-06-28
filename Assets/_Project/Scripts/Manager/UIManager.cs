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
    [SerializeField] private TextMeshProUGUI _autoLvText;
    [SerializeField] private TextMeshProUGUI _autoCostText;

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
        _scoreText.text = $"{FormatNumber(_scoreManager.CurrentScore)} トースト";
    }

    private void UpdateUpgradeUI()
    {
        _critOddsText.text = $"{_damageCalculator.CritRate * 100f:0}%";
        _directOddsText.text = $"{_damageCalculator.DirectRate * 100f:0}%";

        _baseLvText.text = $"LV.{_upgradeManager.BaseLevel}";
        _baseCostText.text = $"{_upgradeManager.BaseCurrentCost:0}";

        _critLvText.text = $"LV.{_upgradeManager.CritLevel}";
        _critCostText.text = _damageCalculator.IsCritRateMax
        ? "MAX"
        : $"{_upgradeManager.CritCurrentCost:0}";

        _directLvText.text = $"LV.{_upgradeManager.DirectLevel}";
        _directCostText.text = _damageCalculator.IsDirectRateMax
        ? "MAX"
        : $"{_upgradeManager.DirectCurrentCost:0}";

        _autoLvText.text = $"LV.{_upgradeManager.ToasterLevel}";
        _autoCostText.text = $"{_upgradeManager.ToasterCurrentCost}";
    }

    private string FormatNumber(double value)
    {
        // 100万未満は普通に表示
        if (value < 1_000_000)
        {
            return value.ToString("0");
        }

        // 100万以上は 1.23e+6 みたいに表示
        return value.ToString("0.##e+0");
    }
}
