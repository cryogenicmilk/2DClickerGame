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

    [Header("最大時の見た目")]
    [SerializeField] private GameObject _critMaxOverlay;
    [SerializeField] private GameObject _directMaxOverlay;

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

        RefreshButtonView();
        _uiManager.UpdateUI();
    }
    //====================================================================
    //各ボタン処理
    //====================================================================
    private void OnClickBaseUpgrade()
    {
        if (!CanBuy(_baseCurrentCost))
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }

        AudioPlayer.Instance.PlaySE(5);

        SpendScore(_baseCurrentCost);

        _baseLv++;
        _damageCalculator.AddBase();

        _baseCurrentCost = GetCost(_baseStartCost, _baseCostGrowh, _baseLv);
        RefreshButtonView();
        _uiManager.UpdateUI();
    }

    private void OnClickCritUpgrade()
    {
        // コストが足りてない
        if (!CanBuy(_critCurrentCost))
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }
        // Max
        if (_damageCalculator.IsCritRateMax)
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }

        AudioPlayer.Instance.PlaySE(5);

        SpendScore(_critCurrentCost);

        _critLv++;
        _damageCalculator.AddCrit(_critAddRate);

        _critCurrentCost = GetCost(_critStartCost, _critCostGrowh, _critLv);

        RefreshButtonView();
        _uiManager.UpdateUI();
    }

    private void OnClickDirectUpgrade()
    {
        // コストが足りてない
        if (!CanBuy(_directCurrentCost))
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }
        // Max
        if (_damageCalculator.IsDirectRateMax)
        {
            AudioPlayer.Instance.PlaySE(0);
            return;
        }

        AudioPlayer.Instance.PlaySE(5);

        SpendScore(_directCurrentCost);

        _directLv++;
        _damageCalculator.AddDirect(_directAddRate);

        _directCurrentCost = GetCost(_directStartCost, _directCostGrowh, _directLv);

        RefreshButtonView();
        _uiManager.UpdateUI();
    }

    //====================================================================
    //レベルごとコストが上がる計算
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

    //====================================================================
    //ビジュアル、SE
    //====================================================================
    private void RefreshButtonView()
    {
        if (_critMaxOverlay != null)
        {
            _critMaxOverlay.SetActive(_damageCalculator.IsCritRateMax);
        }

        if (_directMaxOverlay != null)
        {
            _directMaxOverlay.SetActive(_damageCalculator.IsDirectRateMax);
        }
    }

}
