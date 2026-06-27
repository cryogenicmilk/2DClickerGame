using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private DamageCalculator _damageCalculator;
    [SerializeField] private UIManager _uiManager;

    [Header("ボタン")]
    [SerializeField] private Button _baseUpgradeButton;
    [SerializeField] private Button _critUpgradeButton;
    [SerializeField] private Button _directUpgradeButton;

    [Header("基本アップ")]
    [SerializeField] private int _baseLv = 1;
    [SerializeField] private double _baseStartCost = 50;
    [SerializeField] private double _baseCostGrowh = 1.15;
    private double _baseCurrentCost;

    [Header("クリティカル率")]
    [SerializeField] private int _critLv = 1;
    [SerializeField] private double _critStartCost = 100;
    [SerializeField] private double _critCostGrowh = 1.2;
    [SerializeField] private float _critAddRate = 0.01f; // 1%

    private double _critCurrentCost;

    [Header("ダイレクト率")]
    [SerializeField] private int _directLv = 1;
    [SerializeField] private double _directStartCost = 100;
    [SerializeField] private double _directCostGrowh = 1.2;
    [SerializeField] private float _directAddRate = 0.01f;
    private double _directCurrentCost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _baseUpgradeButton.onClick.AddListener(OnClickBaseUpgrade);
        _critUpgradeButton.onClick.AddListener(OnClickCritUpgrade);
        _directUpgradeButton.onClick.AddListener(OnClickDirectUpgrade);

        _baseCurrentCost = GetCost(_baseStartCost, _baseCostGrowh, _baseLv);
        _critCurrentCost = GetCost(_critStartCost, _critCostGrowh, _critLv);
        _directCurrentCost = GetCost(_directStartCost, _directCostGrowh, _directLv);

        _uiManager.UpdateUI();
    }
    //====================================================================
    //各ボタン処理
    //====================================================================
    private void OnClickBaseUpgrade()
    {
        if (!CanBuy(_baseCurrentCost)) return;

        SpendScore(_baseCurrentCost);

        _baseLv++;
        _damageCalculator.AddBase();

        _baseCurrentCost = GetCost(_baseStartCost, _baseCostGrowh, _baseLv);
        _uiManager.UpdateUI();
    }

    private void OnClickCritUpgrade()
    {

        if (!CanBuy(_critCurrentCost)) return;
        SpendScore(_critCurrentCost);

        _critLv++;
        _damageCalculator.AddCrit(_critAddRate);

        _critCurrentCost = GetCost(_critStartCost, _critCostGrowh, _critLv);
        _uiManager.UpdateUI();
    }

    private void OnClickDirectUpgrade()
    {
        if (!CanBuy(_directCurrentCost)) return;
        SpendScore(_directCurrentCost);

        _directLv++;
        _damageCalculator.AddDirect(_directAddRate);

        _directCurrentCost = GetCost(_directStartCost, _directCostGrowh, _directLv);
        _uiManager.UpdateUI();
    }

    //====================================================================
    //BGM処理
    //====================================================================
    private double GetCost(double startCost, double growth, int level)
    {
        return System.Math.Floor(startCost * System.Math.Pow(growth, level - 1));
    }

    private bool CanBuy(double cost)
    {
        return _scoreManager.CurrentScore >= cost;
    }

    private void SpendScore(double cost)
    {
        _scoreManager.SpendScore(cost);
    }

    public int BaseLevel => _baseLv;
    public int CritLevel => _critLv;
    public int DirectLevel => _directLv;

    public double BaseCurrentCost => _baseCurrentCost;
    public double CritCurrentCost => _critCurrentCost;
    public double DirectCurrentCost => _directCurrentCost;
}
